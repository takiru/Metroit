using System;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// アプリケーションミューテックスを行うときに使用する値のセットを提供します。
    /// </summary>
    public class ApplicationMutexInfo
    {
        private string _name;

        /// <summary>
        /// ミューテックスの名前を設定または取得します。
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Name));
                }
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The mutex name is empty.", nameof(Name));
                }
                _name = value;
            }
        }

        /// <summary>
        /// ミューテックスがロックできなかったときの振る舞いを設定または取得します。
        /// 既定は <see cref="ApplicationMutexBehavior.Shutdown"/> です。
        /// </summary>
        public ApplicationMutexBehavior CanNotLockedBehavior { get; set; } = ApplicationMutexBehavior.Shutdown;

        /// <summary>
        /// <see cref="CanNotLockedBehavior"/> が <see cref="ApplicationMutexBehavior.Shutdown"/> のとき、シャットダウン前に行う制御を取得または設定します。
        /// </summary>
        public Action ShuttingDown { get; set; } = null;

        /// <summary>
        /// <see cref="CanNotLockedBehavior"/> が <see cref="ApplicationMutexBehavior.Shutdown"/> のときの返却コードを取得または設定します。
        /// 既定は 1 です。
        /// </summary>
        public int ShutdownExitCode { get; set; } = 1;

        /// <summary>
        /// <see cref="CanNotLockedBehavior"/> が <see cref="ApplicationMutexBehavior.BringToFront"/> で既存のアプリケーションを前面に表示できたときの返却コードを取得または設定します。
        /// 既定は 2 です。
        /// </summary>
        public int BringToFrontExitCode { get; set; } = 2;

        /// <summary>
        /// <see cref="CanNotLockedBehavior"/> が <see cref="ApplicationMutexBehavior.BringToFront"/> で既存のアプリケーションを前面に表示できなかったときの返却コードを取得または設定します。
        /// 既定は 3 です。
        /// </summary>
        public int BringToFrontFailedExitCode { get; set; } = 3;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="name">ミューテックス名。</param>
        public ApplicationMutexInfo(string name)
        {
            Name = name;
        }
    }
}
