using System;
using System.Collections.Generic;
using System.Text;

namespace Metroit.Win32.DesktopAppUi.MenuRc
{
    /// <summary>
    /// システムコマンドのタイプを提供します。
    /// </summary>
    public static class SystemCommandTypes
    {
        /// <summary>
        /// ウィンドウを閉じます。
        /// </summary>
        public const int SC_CLOSE = 0xF060;

        /// <summary>
        /// カーソルをポインタ付きの疑問符に変更します。次に、ユーザーがダイアログボックスでコントロールをクリックすると、コントロールはWM_HELPメッセージを受信します。
        /// </summary>
        public const int SC_CONTEXTHELP = 0xF180;

        /// <summary>
        /// デフォルトのアイテムを選択します。ユーザーがウィンドウメニューをダブルクリックしました。
        /// </summary>
        public const int SC_DEFAULT = 0xF160;

        /// <summary>
        /// アプリケーション指定のホットキーに関連付けられているウィンドウをアクティブにします。lParamにはパラメータが有効にするには、ウィンドウを識別します。
        /// </summary>
        public const int SC_HOTKEY = 0xF150;

        /// <summary>
        /// 水平方向にスクロールします。
        /// </summary>
        public const int SC_HSCROLL = 0xF080;

        /// <summary>
        /// スクリーンセーバーが安全かどうかを示します。
        /// </summary>
        public const int SCF_ISSECURE = 0x00000001;

        /// <summary>
        /// キーストロークの結果としてウィンドウメニューを取得します。詳細については、「備考」セクションを参照してください。
        /// </summary>
        public const int SC_KEYMENU = 0xF100;

        /// <summary>
        /// ウィンドウを最大化します。
        /// </summary>
        public const int SC_MAXIMIZE = 0xF030;

        /// <summary>
        /// ウィンドウを最小化します。
        /// </summary>
        public const int SC_MINIMIZE = 0xF020;

        /// <summary>
        /// ディスプレイの状態を設定します。このコマンドは、バッテリー駆動のパーソナルコンピューターなど、省電力機能を備えたデバイスをサポートします。lParamにはパラメータは次の値を指定できます。
        ///   -1（ディスプレイの電源がオンになっています）
        ///    1（ディスプレイの電力が低下します）
        ///    2（ディスプレイがオフになっています）
        /// </summary>
        public const int SC_MONITORPOWER = 0xF170;

        /// <summary>
        /// マウスクリックの結果としてウィンドウメニューを取得します。
        /// </summary>
        public const int SC_MOUSEMENU = 0xF090;

        /// <summary>
        /// ウィンドウを移動します。
        /// </summary>
        public const int SC_MOVE = 0xF010;

        /// <summary>
        /// 次のウィンドウに移動します。
        /// </summary>
        public const int SC_NEXTWINDOW = 0xF040;

        /// <summary>
        /// 前のウィンドウに移動します。
        /// </summary>
        public const int SC_PREVWINDOW = 0xF050;

        /// <summary>
        /// ウィンドウを通常の位置とサイズに戻します。
        /// </summary>
        public const int SC_RESTORE = 0xF120;

        /// <summary>
        /// System.iniファイルの[boot]セクションで指定されたスクリーンセーバーアプリケーションを実行します。
        /// </summary>
        public const int SC_SCREENSAVE = 0xF140;

        /// <summary>
        /// ウィンドウのサイズを変更します。
        /// </summary>
        public const int SC_SIZE = 0xF000;

        /// <summary>
        /// スタートメニューをアクティブにします。
        /// </summary>
        public const int SC_TASKLIST = 0xF130;

        /// <summary>
        /// 垂直方向にスクロールします。
        /// </summary>
        public const int SC_VSCROLL = 0xF070;
    }
}
