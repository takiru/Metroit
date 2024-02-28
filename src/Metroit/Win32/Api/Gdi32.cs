using System;
using System.Runtime.InteropServices;

namespace Metroit.Win32.Api
{
    /// <summary>
    /// gdi32.dll を利用するWin32APIを提供します。
    /// </summary>
    public static class Gdi32
    {
        /// <summary>
        /// 宛先デバイスコンテキストに指定されたソースデバイスコンテキストからピクセルの矩形に対応する色データのビットブロック転送を行います。
        /// </summary>
        /// <param name="hdc">宛先デバイスコンテキストへのハンドル。</param>
        /// <param name="x">宛先長方形の左上隅のx座標（論理単位）。</param>
        /// <param name="y">宛先長方形の左上隅の論理単位でのy座標。</param>
        /// <param name="cx">ソース長方形と宛先長方形の幅（論理単位）。</param>
        /// <param name="cy">ソース長方形と宛先長方形の高さ（論理単位）。</param>
        /// <param name="hdcSrc">ソースデバイスコンテキストへのハンドル。</param>
        /// <param name="x1">ソース長方形の左上隅のx座標（論理単位）。</param>
        /// <param name="y1">ソース長方形の左上隅のy座標（論理単位）。</param>
        /// <param name="rop">ラスター操作コード。これらのコードは、ソース長方形のカラーデータを宛先長方形のカラーデータと組み合わせて最終的な色を実現する方法を定義します。</param>
        /// <returns>0:失敗, 0以外:成功。</returns>
        [DllImport("gdi32.dll")]
        public static extern int BitBlt(IntPtr hdc, int x, int y, int cx, int cy, IntPtr hdcSrc, int x1, int y1, int rop);

        /// <summary>
        /// 指定されたデバイスと互換性のあるメモリデバイスコンテキスト（DC）を作成します。
        /// </summary>
        /// <param name="hdc">既存のDCへのハンドル。このハンドルがNULLの場合、関数はアプリケーションの現在の画面と互換性のあるメモリDCを作成します。</param>
        /// <returns>null:失敗, メモリDCのハンドル:成功。</returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        /// <summary>
        /// 指定されたデバイスコンテキスト（DC）を削除します。
        /// </summary>
        /// <param name="hdc">デバイスコンテキストへのハンドル。</param>
        /// <returns>0:失敗, 0以外:成功。</returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        /// <summary>
        /// 指定されたデバイスコンテキスト（DC）に変換するオブジェクトを選択します。新しいオブジェクトは、同じタイプの以前のオブジェクトを置き換えます。
        /// </summary>
        /// <param name="hdc">DCへのハンドル。</param>
        /// <param name="hgdiobj">選択するオブジェクトへのハンドル。</param>
        /// <returns>選択したオブジェクトが領域ではなく、関数が成功した場合、戻り値は置き換えられるオブジェクトへのハンドルです。エラーが発生し、選択したオブジェクトがリージョンではない場合、戻り値はNULLです。それ以外の場合は、HGDI_ERRORです。</returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        /// <summary>
        /// オブジェクトに関連付けられているすべてのシステムリソースを解放し、論理ペン、ブラシ、フォント、ビットマップ、領域、またはパレットを削除します。オブジェクトが削除されると、指定されたハンドルは無効になります。
        /// </summary>
        /// <param name="hgdiobj">論理ペン、ブラシ、フォント、ビットマップ、領域、またはパレットへのハンドル。</param>
        /// <returns>関数が成功した場合、戻り値はゼロ以外です。指定されたハンドルが無効であるか、現在DCに選択されている場合、戻り値はゼロです。</returns>
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hgdiobj);
    }
}
