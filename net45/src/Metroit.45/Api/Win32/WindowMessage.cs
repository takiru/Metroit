using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroit.Api.Win32
{
    /// <summary>
    /// ウィンドウメッセージを提供します。
    /// </summary>
    public static class WindowMessage
    {
        /// <summary>
        /// キーの押下を定義します。
        /// </summary>
        public const int WM_KEYDOWN = 0x0100;

        /// <summary>
        /// キーのリリースを定義します。
        /// </summary>
        public const int WM_KEYUP = 0x0101;

        /// <summary>
        /// マウスホイール操作を定義します。
        /// </summary>
        public const int WM_MOUSEWHEEL = 0x020A;

        /// <summary>
        /// 貼り付けを定義します。
        /// </summary>
        public const int WM_PASTE = 0x0302;

        /// <summary>
        /// システムコマンドを定義します。
        /// </summary>
        public const int WM_SYSCOMMAND = 0x0112;

        /// <summary>
        /// ウインドウの非クライアント領域の描画を定義します。
        /// </summary>
        public const int WM_NCPAINT = 0x0085;
    }
}
