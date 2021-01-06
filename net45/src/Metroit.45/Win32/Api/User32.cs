using Metroit.Win32.Api.DisplayDeviceReference.WinDef;
using Metroit.Win32.Api.WindowsGdi.WinUser;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Metroit.Win32.Api
{
    /// <summary>
    /// user32.dll を利用するWin32APIを提供します。
    /// </summary>
    public static class User32
    {
        /// <summary>
        /// ウィンドウハンドルへメッセージを送信します。
        /// </summary>
        /// <param name="hWnd">ウィンドウハンドル。</param>
        /// <param name="Msg">メッセージ。</param>
        /// <param name="wParam">メッセージ特有の追加情報。</param>
        /// <param name="lParam">メッセージ特有の追加情報。</param>
        /// <returns>メッセージ処理の結果が返ります。この戻り値の意味は、送信されたメッセージにより異なります。</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// タイトルバー、メニュー、スクロールバーを含む、ウィンドウ全体のデバイスコンテキスト（DC）を取得します。
        /// </summary>
        /// <param name="hwnd">ウィンドウのハンドル。</param>
        /// <returns>null以外:ウィンドウのデバイスコンテキストのハンドル, null:取得エラー。</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);

        /// <summary>
        /// 指定されたウィンドウに対して描画の準備をします。
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル。</param>
        /// <param name="lpPaint">描画情報を持つ構造体へのポインタ。</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PaintStruct lpPaint);

        /// <summary>
        /// 指定されたウィンドウ内の描画の終わりを示します。
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル。</param>
        /// <param name="lpPaint">描画データ。</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, ref PaintStruct lpPaint);

        /// <summary>
        /// 指定されたウィンドウの外接する四角形の寸法を取得します。寸法は、画面の左上隅を基準にした画面座標で示されます。
        /// </summary>
        /// <param name="hWnd">ウィンドウへのハンドル。</param>
        /// <param name="lpRect">ウィンドウの左上隅と右下隅の画面座標を受け取るRECT構造体へのポインター 。</param>
        /// <returns>0:失敗, 0以外:成功。</returns>
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);
    }
}
