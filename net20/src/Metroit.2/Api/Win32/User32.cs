using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Metroit.Api.Win32
{
    /// <summary>
    /// user32.dll を利用するWin32APIを提供します。
    /// </summary>
    public class User32
    {
        /// <summary>
        /// ウィンドウハンドルへメッセージを送信します。
        /// </summary>
        /// <param name="hWnd">ウィンドウハンドル。</param>
        /// <param name="Msg">メッセージ。</param>
        /// <param name="wParam">メッセージ特有の追加情報。</param>
        /// <param name="lParam">メッセージ特有の追加情報。</param>
        /// <returns>メッセージ処理の結果が返ります。この戻り値の意味は、送信されたメッセージにより異なります。</returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, uint wParam, int lParam);
    }
}
