namespace Metroit.Win32.Api
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
        /// 切り取りを定義します。
        /// </summary>
        public const int WM_CUT = 0x0300;

        /// <summary>
        /// システムコマンドを定義します。
        /// </summary>
        public const int WM_SYSCOMMAND = 0x0112;

        /// <summary>
        /// ウィンドウのクライアント領域の描画を定義します。
        /// </summary>
        public const int WM_PAINT = 0x000F;

        /// <summary>
        /// ウインドウの非クライアント領域の描画を定義します。
        /// </summary>
        public const int WM_NCPAINT = 0x0085;

        /// <summary>
        /// IMEがキーストロークの結果としてコンポジション文字列を生成する直前に送信されるメッセージを定義します。
        /// </summary>
        public const int WM_IME_STARTCOMPOSITION = 0x010D;

        /// <summary>
        /// IMEが合成を終了すると送信されるメッセージを定義します。
        /// </summary>
        public const int WM_IME_ENDCOMPOSITION = 0x010E;

        /// <summary>
        /// IMEがキーストロークの結果として構成ステータスを変更すると送信されるメッセージを定義します。
        /// </summary>
        public const int WM_IME_COMPOSITION = 0x010F;

        /// <summary>
        /// イベントが発生したとき、またはコントロールが何らかの情報を必要とするときに、共通コントロールによってその親ウィンドウに送信されます。
        /// </summary>
        public const int WM_NOTIFY = 0x004E;
    }
}
