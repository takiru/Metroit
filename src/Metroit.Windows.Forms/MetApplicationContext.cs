using System;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// システムのアプリケーションコンテキストを提供します。
    /// </summary>
    public class MetApplicationContext : ApplicationContext
    {
        /// <summary>
        /// アプリケーションコンテキスト。
        /// </summary>
        private static MetApplicationContext _context = null;

        /// <summary>
        /// 指定した画面をメイン画面として新しいアプリケーションコンテキストを生成します。
        /// </summary>
        /// <param name="mainForm">メイン画面オブジェクト。</param>
        /// <returns>アプリケーションコンテキスト。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="mainForm"/> が <see langword="null"/> です。</exception>
        /// <exception cref="InvalidOperationException">すでにアプリケーションコンテキストは生成されています。</exception>
        public static ApplicationContext NewContext(Form mainForm)
        {
            if (mainForm == null)
            {
                throw new ArgumentNullException(nameof(mainForm));
            }
            if (_context != null)
            {
                throw new InvalidOperationException(ExceptionResources.GetString("MetApplicationContextAlreadyGenerated"));
            }

            _context = new MetApplicationContext(mainForm);
            return _context;
        }

        /// <summary>
        /// 新しいインスタンスを生成する。
        /// </summary>
        /// <param name="mainForm">メイン画面オブジェクト。</param>
        private MetApplicationContext(Form mainForm) : base(mainForm)
        {
            if (mainForm != null)
            {
                mainForm.FormClosed += FormClosed;
            }
        }

        /// <summary>
        /// 元の画面を閉じて新しい画面をメイン画面として開きます。
        /// </summary>
        /// <param name="form">新しい画面オブジェクト。</param>
        /// <exception cref="ArgumentNullException"><paramref name="form"/> が <see langword="null"/> です。</exception>
        /// <exception cref="InvalidOperationException">
        /// アプリケーションコンテキストが開始されていません。<br/>
        /// または<br/>
        /// すでに破棄された画面オブジェクトです。
        /// </exception>
        public static void NavigationMainWindow(Form form)
        {
            var oldForm = _context.MainForm;

            ShowNewWindow(form);

            if (oldForm != null)
            {
                oldForm.FormClosed -= _context.FormClosed;
                oldForm.Close();
                oldForm.Dispose();
            }
        }

        /// <summary>
        /// 元の画面を保持したまま新しい画面をメイン画面として開きます。
        /// </summary>
        /// <param name="form">新しい画面オブジェクト。</param>
        /// <exception cref="ArgumentNullException"><paramref name="form"/> が <see langword="null"/> です。</exception>
        /// <exception cref="InvalidOperationException">
        /// アプリケーションコンテキストが開始されていません。<br/>
        /// または<br/>
        /// すでに破棄された画面オブジェクトです。
        /// </exception>
        public static void NavigationWindowWithoutClose(Form form)
        {
            var oldForm = _context.MainForm;

            ShowNewWindow(form);

            if (oldForm != null)
            {
                oldForm.FormClosed -= _context.FormClosed;
            }
        }

        /// <summary>
        /// 新しい画面をメイン画面として表示する。
        /// </summary>
        /// <param name="form">新しい画面オブジェクト。</param>
        /// <exception cref="ArgumentNullException"><paramref name="form"/> が <see langword="null"/> です。</exception>
        /// <exception cref="InvalidOperationException">
        /// アプリケーションコンテキストが開始されていません。<br/>
        /// または<br/>
        /// すでに破棄された画面オブジェクトです。
        /// </exception>
        private static void ShowNewWindow(Form form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            if (_context == null)
            {
                throw new InvalidOperationException(ExceptionResources.GetString("MetApplicationContextNotStarted"));
            }

            if (form.IsDisposed)
            {
                throw new InvalidOperationException(ExceptionResources.GetString("MetApplicationContextFormDisposed"));
            }

            _context.MainForm = form;
            _context.MainForm.FormClosed += _context.FormClosed;
            _context.MainForm.Show();
        }

        /// <summary>
        /// メイン画面が終了したときにスレッドを終了してアプリケーションの終了を明示的に行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormClosed(object sender, FormClosedEventArgs e)
        {
            ExitThread();
        }

        /// <summary>
        /// アプリケーションコンテキストが破棄されたらメイン画面も破棄する。
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.MainForm?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
