using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Metroit.Win32.Api.WindowsControls.WinUser
{
    /// <summary>
    /// 通知メッセージに関する情報が含まれています。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NmHdr
    {
        /// <summary>
        /// メッセージを送信するコントロールへのウィンドウハンドル。
        /// </summary>
        public IntPtr hwndFrom;

        /// <summary>
        /// メッセージを送信するコントロールの識別子。
        /// </summary>
        public IntPtr idFrom;

        /// <summary>
        /// 通知コード。
        /// </summary>
        public int code;
    }
}
