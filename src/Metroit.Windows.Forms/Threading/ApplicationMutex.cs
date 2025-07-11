using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// アプリケーションミューテックスを提供します。
    /// </summary>
    public class ApplicationMutex
    {
        /// <summary>
        /// ミューテックスの設定を取得または設定します。
        /// </summary>
        public ApplicationMutexInfo MutexInfo { get; set; } = null;

        /// <summary>
        /// ミューテックスの実態。
        /// </summary>
        private Mutex _mutex = default;

        /// <summary>
        /// ロックが行われているかどうか。
        /// </summary>
        private bool _isLocked = false;

        /// <summary>
        /// 外部プロセスのメイン・ウィンドウを最前面にする。
        /// </summary>
        /// <param name="hWnd">ウィンドウハンドル。</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// ウィンドウの表示状態を変更する。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// ウィンドウが最小化されているかどうかを取得する。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        /// <summary>
        /// ウィンドウサイズを元に戻すコマンドの値。
        /// </summary>
        private static readonly int SW_RESTORE = 9;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public ApplicationMutex() { }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="name">ミューテックスの名前。</param>
        public ApplicationMutex(string name)
        {
            MutexInfo = new ApplicationMutexInfo(name);
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="mutexInfo">ミューテックスで使用する値。</param>
        public ApplicationMutex(ApplicationMutexInfo mutexInfo)
        {
            MutexInfo = mutexInfo;
        }

        /// <summary>
        /// アプリケーションの二重起動をロックします。<br/>
        /// ロックできなかったときには <see cref="MutexInfo"/> に従ってロックを中断します。
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="MutexInfo"/> が未設定か、アプリケーションがすでにロック済みです。</exception>
        public void Lock()
        {
            if (_isLocked)
            {
                throw new InvalidOperationException("The application is already locked.");
            }
            if (MutexInfo == null)
            {
                throw new InvalidOperationException("MutexInfo is not set.");
            }

            var createdNew = false;
            _mutex = new Mutex(true, MutexInfo.Name, out createdNew);
            if (createdNew)
            {
                _isLocked = true;
                return;
            }

            if (MutexInfo.CanNotLockedBehavior == ApplicationMutexBehavior.Shutdown)
            {
                MutexInfo.ShuttingDown?.Invoke();
                Environment.Exit(MutexInfo.ShutdownExitCode);
                return;
            }

            if (MutexInfo.CanNotLockedBehavior == ApplicationMutexBehavior.BringToFront)
            {
                var startedProcess = GetStartedProcess();
                if (startedProcess == null)
                {
                    Environment.Exit(MutexInfo.BringToFrontFailedExitCode);
                }
                WakeupWindow(startedProcess.MainWindowHandle);
                Environment.Exit(MutexInfo.BringToFrontExitCode);
                return;
            }
        }

        /// <summary>
        /// アプリケーションのロックを解除します。
        /// </summary>
        /// <exception cref="InvalidOperationException">アプリケーションはロックされていません。</exception>
        public void Unlock()
        {
            if (!_isLocked)
            {
                throw new InvalidOperationException("The application is not locked.");
            }

            _mutex.ReleaseMutex();
            _mutex.Close();
            _mutex.Dispose();
            _mutex = null;
            _isLocked = false;
        }

        /// <summary>
        /// 対象ウィンドウハンドルをアクティブにする。
        /// </summary>
        /// <param name="hWnd">ウィンドウハンドル。</param>
        private static void WakeupWindow(IntPtr hWnd)
        {
            // メイン・ウィンドウが最小化されていれば元に戻す
            if (IsIconic(hWnd))
            {
                ShowWindowAsync(hWnd, SW_RESTORE);
            }

            // メイン・ウィンドウを最前面に表示する
            SetForegroundWindow(hWnd);
        }

        /// <summary>
        /// 起動済みの同一プロセスを取得する。
        /// </summary>
        /// <returns></returns>
        private static Process GetStartedProcess()
        {
            var curProcess = Process.GetCurrentProcess();
            var allSameProcesses = Process.GetProcessesByName(curProcess.ProcessName)
                .Where(x => x.Id != curProcess.Id);

            var crypto = SHA256.Create();
            foreach (var sameProcess in allSameProcesses)
            {
                // exeのハッシュ値が同一の場合、同一アプリケーションとみなす
                byte[] byteValue = File.ReadAllBytes(sameProcess.MainModule.FileName);
                byte[] hashValue = crypto.ComputeHash(byteValue);
                var sameProcessSha = string.Join("", hashValue.Select(x => x.ToString("x2")).ToArray());

                byteValue = File.ReadAllBytes(curProcess.MainModule.FileName);
                hashValue = crypto.ComputeHash(byteValue);
                var curProcessSha = string.Join("", hashValue.Select(x => x.ToString("x2")).ToArray());

                if (sameProcessSha == curProcessSha)
                {
                    return sameProcess;
                }
            }

            return null;
        }
    }
}
