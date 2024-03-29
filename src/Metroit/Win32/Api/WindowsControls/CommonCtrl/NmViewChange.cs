﻿using Metroit.Win32.Api.WindowsControls.WinUser;
using System.Runtime.InteropServices;

namespace Metroit.Win32.Api.WindowsControls.CommonCtrl
{
    /// <summary>
    /// MCN_VIEWCHANGE通知コードの処理に必要な情報を格納します。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NmViewChange
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
