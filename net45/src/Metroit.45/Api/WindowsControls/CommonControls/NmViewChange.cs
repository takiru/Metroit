using Metroit.Api.WindowsControls.WinUser;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Metroit.Api.WindowsControls.CommonControls
{
    /// <summary>
    /// MCN_VIEWCHANGE通知コードの処理に必要な情報を格納します。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NMVIEWCHANGE
    {
        /// <summary>
        /// この通知コードに関する情報を含むNMHDR構造。
        /// </summary>
        public NmHdr nmhdr;

        /// <summary>
        /// 古いビュー。
        /// </summary>
        public int dwOldView;

        /// <summary>
        /// 新しいビュー。
        /// </summary>
        public int dwNewView;
    }
}
