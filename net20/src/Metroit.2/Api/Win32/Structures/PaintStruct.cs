using System;
using System.Runtime.InteropServices;

namespace Metroit.Api.Win32.Structures
{
    /// <summary>
    /// ウィンドウのクライアント領域の描画に使用できる情報が含まれています。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PAINTSTRUCT
    {
        /// <summary>
        /// 描画に使用するディスプレイ コンテキストを識別します。
        /// </summary>
        public IntPtr hdc;

        /// <summary>
        /// バックグラウンドが再描画されるようにする必要があるかどうかを指定します。 
        /// </summary>
        public bool fErase;

        /// <summary>
        /// 
        /// </summary>
        public RECT rcPaint;

        /// <summary>
        /// 
        /// </summary>
        public bool fRestore;

        /// <summary>
        /// 
        /// </summary>
        public bool fIncUpdate;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public byte[] rgbReserved;
    }
}
