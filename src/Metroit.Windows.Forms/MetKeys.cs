using Metroit.Windows.Forms.ComponentModel;
using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// キー コードと修飾子を指定します。
    /// この列挙体は、メンバー値のビットごとの組み合わせをサポートしています。
    /// </summary>
    [Flags]
    [TypeConverter(typeof(MetKeysConverter))]
    [Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
    public enum MetKeys
    {
        /// <summary>
        /// A キー。
        /// </summary>
        A = 65,

        /// <summary>
        /// Add キー
        /// </summary>
        Add = 107,

        /// <summary>
        /// Alt 修飾子キー
        /// </summary>
        Alt = 262144,

        /// <summary>
        /// アプリケーション キー (Microsoft Natural Keyboard)
        /// </summary>
        Apps = 93,

        /// <summary>
        /// Attn キー。
        /// </summary>
        Attn = 246,

        /// <summary>
        /// B キー。
        /// </summary>
        B = 66,

        /// <summary>
        /// BackSpace キー。
        /// </summary>
        Back = 8,

        /// <summary>
        /// ブラウザーの戻るキー。
        /// </summary>
        BrowserBack = 166,

        /// <summary>
        /// ブラウザーのお気に入りキー。
        /// </summary>
        BrowserFavorites = 171,

        /// <summary>
        /// ブラウザーの進むキー。
        /// </summary>
        BrowserForward = 167,

        /// <summary>
        /// ブラウザーのホーム キー。
        /// </summary>
        BrowserHome = 172,

        /// <summary>
        /// ブラウザーの更新キー。
        /// </summary>
        BrowserRefresh = 168,

        /// <summary>
        /// ブラウザーの検索キー。
        /// </summary>
        BrowserSearch = 170,

        /// <summary>
        /// ブラウザーの中止キー。
        /// </summary>
        BrowserStop = 169,

        /// <summary>
        /// C キー。
        /// </summary>
        C = 67,

        /// <summary>
        /// Cancel キー
        /// </summary>
        Cancel = 3,

        /// <summary>
        /// CAPS LOCK キー
        /// </summary>
        Capital = 20,

        /// <summary>
        /// CAPS LOCK キー
        /// </summary>
        CapsLock = 20,

        /// <summary>
        /// Clear キー。
        /// </summary>
        Clear = 12,

        /// <summary>
        /// Ctrl 修飾子キー
        /// </summary>
        Control = 131072,

        /// <summary>
        /// CTRL キー
        /// </summary>
        ControlKey = 17,

        /// <summary>
        /// Crsel キー。
        /// </summary>
        Crsel = 247,

        /// <summary>
        /// D キー。
        /// </summary>
        D = 68,

        /// <summary>
        /// 0 キー。
        /// </summary>
        D0 = 48,

        /// <summary>
        /// 1 キー。
        /// </summary>
        D1 = 49,

        /// <summary>
        /// 2 キー。
        /// </summary>
        D2 = 50,

        /// <summary>
        /// 3 キー。
        /// </summary>
        D3 = 51,

        /// <summary>
        /// 4 キー。
        /// </summary>
        D4 = 52,

        /// <summary>
        /// 5 キー。
        /// </summary>
        D5 = 53,

        /// <summary>
        /// 6 キー。
        /// </summary>
        D6 = 54,

        /// <summary>
        /// 7 キー。
        /// </summary>
        D7 = 55,

        /// <summary>
        /// 8 キー。
        /// </summary>
        D8 = 56,

        /// <summary>
        /// 9 キー。
        /// </summary>
        D9 = 57,

        /// <summary>
        /// 小数点キー
        /// </summary>
        Decimal = 110,

        /// <summary>
        /// DEL キー
        /// </summary>
        Delete = 46,

        /// <summary>
        /// 除算記号 (/) キー
        /// </summary>
        Divide = 111,

        /// <summary>
        /// ↓キー。
        /// </summary>
        Down = 40,

        /// <summary>
        /// E キー。
        /// </summary>
        E = 69,

        /// <summary>
        /// End キー。
        /// </summary>
        End = 35,

        /// <summary>
        /// Enter キー。
        /// </summary>
        Enter = 13,

        /// <summary>
        /// Erase Eof キー。
        /// </summary>
        EraseEof = 249,

        /// <summary>
        /// Esc キー。
        /// </summary>
        Escape = 27,

        /// <summary>
        /// Execute キー。
        /// </summary>
        Execute = 43,

        /// <summary>
        /// Exsel キー。
        /// </summary>
        Exsel = 248,

        /// <summary>
        /// F キー。
        /// </summary>
        F = 70,

        /// <summary>
        /// F1 キー。
        /// </summary>
        F1 = 112,

        /// <summary>
        /// F10 キー。
        /// </summary>
        F10 = 121,

        /// <summary>
        /// F11 キー。
        /// </summary>
        F11 = 122,

        /// <summary>
        /// F12 キー。
        /// </summary>
        F12 = 123,

        /// <summary>
        /// F13 キー。
        /// </summary>
        F13 = 124,

        /// <summary>
        /// F14 キー。
        /// </summary>
        F14 = 125,

        /// <summary>
        /// F15 キー。
        /// </summary>
        F15 = 126,

        /// <summary>
        /// F16 キー。
        /// </summary>
        F16 = 127,

        /// <summary>
        /// F17 キー。
        /// </summary>
        F17 = 128,

        /// <summary>
        /// F18 キー。
        /// </summary>
        F18 = 129,

        /// <summary>
        /// F19 キー。
        /// </summary>
        F19 = 130,

        /// <summary>
        /// F2 キー。
        /// </summary>
        F2 = 113,

        /// <summary>
        /// F20 キー。
        /// </summary>
        F20 = 131,

        /// <summary>
        /// F21 キー。
        /// </summary>
        F21 = 132,

        /// <summary>
        /// F22 キー。
        /// </summary>
        F22 = 133,

        /// <summary>
        /// F23 キー。
        /// </summary>
        F23 = 134,

        /// <summary>
        /// F24 キー。
        /// </summary>
        F24 = 135,

        /// <summary>
        /// F3 キー。
        /// </summary>
        F3 = 114,

        /// <summary>
        /// F4 キー。
        /// </summary>
        F4 = 115,

        /// <summary>
        /// F5 キー。
        /// </summary>
        F5 = 116,

        /// <summary>
        /// F6 キー。
        /// </summary>
        F6 = 117,

        /// <summary>
        /// F7 キー。
        /// </summary>
        F7 = 118,

        /// <summary>
        /// F8 キー。
        /// </summary>
        F8 = 119,

        /// <summary>
        /// F9 キー。
        /// </summary>
        F9 = 120,

        /// <summary>
        /// IME Final モード キー
        /// </summary>
        FinalMode = 24,

        /// <summary>
        /// G キー。
        /// </summary>
        G = 71,

        /// <summary>
        /// H キー。
        /// </summary>
        H = 72,

        /// <summary>
        /// IME ハングル モード キー (互換性を保つために保持されています。HangulMode を使用します)
        /// </summary>
        HanguelMode = 21,

        /// <summary>
        /// IME ハングル モード キー。
        /// </summary>
        HangulMode = 21,

        /// <summary>
        /// IME Hanja モード キー。
        /// </summary>
        HanjaMode = 25,

        /// <summary>
        /// Help キー。
        /// </summary>
        Help = 47,

        /// <summary>
        /// Home キー。
        /// </summary>
        Home = 36,

        /// <summary>
        /// I キー。
        /// </summary>
        I = 73,

        /// <summary>
        /// IME Accept キー (IMEAceept の代わりに使用します)
        /// </summary>
        IMEAccept = 30,

        /// <summary>
        /// IME Accept キー 互換性を維持するために残されています。代わりに IMEAccept を使用してください。
        /// </summary>
        IMEAceept = 30,

        /// <summary>
        /// IME 変換キー
        /// </summary>
        IMEConvert = 28,

        /// <summary>
        /// IME モード変更キー
        /// </summary>
        IMEModeChange = 31,

        /// <summary>
        /// IME 無変換キー
        /// </summary>
        IMENonconvert = 29,

        /// <summary>
        /// INS キー
        /// </summary>
        Insert = 45,

        /// <summary>
        /// J キー。
        /// </summary>
        J = 74,

        /// <summary>
        /// IME Junja モード キー。
        /// </summary>
        JunjaMode = 23,

        /// <summary>
        /// K キー。
        /// </summary>
        K = 75,

        /// <summary>
        /// IME かなモード キー。
        /// </summary>
        KanaMode = 21,

        /// <summary>
        /// IME 漢字モード キー。
        /// </summary>
        KanjiMode = 25,

        /// <summary>
        /// キー値からキー コードを抽出するビット マスク。
        /// </summary>
        KeyCode = 65535,

        /// <summary>
        /// L キー。
        /// </summary>
        L = 76,

        /// <summary>
        /// カスタム ホット キー 1。
        /// </summary>
        LaunchApplication1 = 182,

        /// <summary>
        /// カスタム ホット キー 2。
        /// </summary>
        LaunchApplication2 = 183,

        /// <summary>
        /// メールの起動キー。
        /// </summary>
        LaunchMail = 180,

        /// <summary>
        /// マウスの左ボタン
        /// </summary>
        LButton = 1,

        /// <summary>
        /// 左 Ctrl キー。
        /// </summary>
        LControlKey = 162,

        /// <summary>
        /// ←キー。
        /// </summary>
        Left = 37,

        /// <summary>
        /// ライン フィード キー
        /// </summary>
        LineFeed = 10,

        /// <summary>
        /// 左 Alt キー。
        /// </summary>
        LMenu = 164,

        /// <summary>
        /// 左の Shift キー
        /// </summary>
        LShiftKey = 160,

        /// <summary>
        /// 左 Windows ロゴ キー (Microsoft Natural Keyboard)。
        /// </summary>
        LWin = 91,

        /// <summary>
        /// M キー。
        /// </summary>
        M = 77,

        /// <summary>
        /// マウスの中央ボタン (3 ボタン マウスの場合)
        /// </summary>
        MButton = 4,

        /// <summary>
        /// メディアの次のトラック キー。
        /// </summary>
        MediaNextTrack = 176,

        /// <summary>
        /// メディアの再生/一時停止キー。
        /// </summary>
        MediaPlayPause = 179,

        /// <summary>
        /// メディアの前のトラック キー。
        /// </summary>
        MediaPreviousTrack = 177,

        /// <summary>
        /// メディアの停止キー。
        /// </summary>
        MediaStop = 178,

        /// <summary>
        /// Alt キー。
        /// </summary>
        Menu = 18,

        /// <summary>
        /// キー値から修飾子を抽出するビット マスク。
        /// </summary>
        Modifiers = -65536,

        /// <summary>
        /// 乗算記号 (*) キー
        /// </summary>
        Multiply = 106,

        /// <summary>
        /// N キー。
        /// </summary>
        N = 78,

        /// <summary>
        /// Page Down キー。
        /// </summary>
        Next = 34,

        /// <summary>
        /// 将来使用するために予約されている定数。
        /// </summary>
        NoName = 252,

        /// <summary>
        /// 押されたキーがありません。
        /// </summary>
        None = 0,

        /// <summary>
        /// NUM LOCK キー
        /// </summary>
        NumLock = 144,

        /// <summary>
        /// 0 キー (テンキー)。
        /// </summary>
        NumPad0 = 96,

        /// <summary>
        /// 1 キー (テンキー)。
        /// </summary>
        NumPad1 = 97,

        /// <summary>
        /// 2 キー (テンキー)。
        /// </summary>
        NumPad2 = 98,

        /// <summary>
        /// 3 キー (テンキー)。
        /// </summary>
        NumPad3 = 99,

        /// <summary>
        /// 4 キー (テンキー)。
        /// </summary>
        NumPad4 = 100,

        /// <summary>
        /// 5 キー (テンキー)。
        /// </summary>
        NumPad5 = 101,

        /// <summary>
        /// 6 キー (テンキー)。
        /// </summary>
        NumPad6 = 102,

        /// <summary>
        /// 7 キー (テンキー)。
        /// </summary>
        NumPad7 = 103,

        /// <summary>
        /// 8 キー (テンキー)。
        /// </summary>
        NumPad8 = 104,

        /// <summary>
        /// 9 キー (テンキー)
        /// </summary>
        NumPad9 = 105,

        /// <summary>
        /// O キー。
        /// </summary>
        O = 79,

        /// <summary>
        /// OEM 1 キー。
        /// </summary>
        Oem1 = 186,

        /// <summary>
        /// OEM 102 キー。
        /// </summary>
        Oem102 = 226,

        /// <summary>
        /// OEM 2 キー。
        /// </summary>
        Oem2 = 191,

        /// <summary>
        /// OEM 3 キー。
        /// </summary>
        Oem3 = 192,

        /// <summary>
        /// OEM 4 キー。
        /// </summary>
        Oem4 = 219,

        /// <summary>
        /// OEM 5 キー。
        /// </summary>
        Oem5 = 220,

        /// <summary>
        /// OEM 6 キー。
        /// </summary>
        Oem6 = 221,

        /// <summary>
        /// OEM 7 キー。
        /// </summary>
        Oem7 = 222,

        /// <summary>
        /// OEM 8 キー。
        /// </summary>
        Oem8 = 223,

        /// <summary>
        /// RT 102 キーのキーボード上の OEM 山かっこキーまたは円記号キー。
        /// </summary>
        OemBackslash = 226,

        /// <summary>
        /// Clear キー。
        /// </summary>
        OemClear = 254,

        /// <summary>
        /// 米国標準キーボード上の OEM 右角かっこキー。
        /// </summary>
        OemCloseBrackets = 221,

        /// <summary>
        /// 国または地域別キーボード上の OEM コンマ キー。
        /// </summary>
        Oemcomma = 188,

        /// <summary>
        /// 国または地域別キーボード上の OEM マイナス キー。
        /// </summary>
        OemMinus = 189,

        /// <summary>
        /// 米国標準キーボード上の OEM 左角かっこキー。
        /// </summary>
        OemOpenBrackets = 219,

        /// <summary>
        /// 国または地域別キーボード上の OEM ピリオド キー。
        /// </summary>
        OemPeriod = 190,

        /// <summary>
        /// 米国標準キーボード上の OEM パイプ キー。
        /// </summary>
        OemPipe = 220,

        /// <summary>
        /// 国または地域別キーボード上の OEM プラス キー。
        /// </summary>
        Oemplus = 187,

        /// <summary>
        /// 米国標準キーボード上の OEM 疑問符キー。
        /// </summary>
        OemQuestion = 191,

        /// <summary>
        /// 米国標準キーボード上の OEM 一重/二重引用符キー。
        /// </summary>
        OemQuotes = 222,

        /// <summary>
        /// 米国標準キーボード上の OEM セミコロン キー。
        /// </summary>
        OemSemicolon = 186,

        /// <summary>
        /// 米国標準キーボード上の OEM チルダ キー。
        /// </summary>
        Oemtilde = 192,

        /// <summary>
        /// P キー。
        /// </summary>
        P = 80,

        /// <summary>
        /// PA1 キー。
        /// </summary>
        Pa1 = 253,

        /// <summary>
        /// Unicode 文字がキーストロークであるかのように渡されます。 Packet のキー値は、キーボード以外の入力手段に使用される 32 ビット仮想キー値の下位ワードです。
        /// </summary>
        Packet = 231,

        /// <summary>
        /// Page Down キー。
        /// </summary>
        PageDown = 34,

        /// <summary>
        /// Page Up キー。
        /// </summary>
        PageUp = 33,

        /// <summary>
        /// Pause キー。
        /// </summary>
        Pause = 19,

        /// <summary>
        /// Play キー。
        /// </summary>
        Play = 250,

        /// <summary>
        /// Print キー。
        /// </summary>
        Print = 42,

        /// <summary>
        /// Print Screen キー。
        /// </summary>
        PrintScreen = 44,

        /// <summary>
        /// Page Up キー。
        /// </summary>
        Prior = 33,

        /// <summary>
        /// ProcessKey キー
        /// </summary>
        ProcessKey = 229,

        /// <summary>
        /// Q キー。
        /// </summary>
        Q = 81,

        /// <summary>
        /// R キー。
        /// </summary>
        R = 82,

        /// <summary>
        /// マウスの右ボタン
        /// </summary>
        RButton = 2,

        /// <summary>
        /// 右 Ctrl キー。
        /// </summary>
        RControlKey = 163,

        /// <summary>
        /// Return キー
        /// </summary>
        Return = 13,

        /// <summary>
        /// →キー。
        /// </summary>
        Right = 39,

        /// <summary>
        /// 右 Alt キー。
        /// </summary>
        RMenu = 165,

        /// <summary>
        /// 右の Shift キー
        /// </summary>
        RShiftKey = 161,

        /// <summary>
        /// 右 Windows ロゴ キー (Microsoft Natural Keyboard)。
        /// </summary>
        RWin = 92,

        /// <summary>
        /// S キー。
        /// </summary>
        S = 83,

        /// <summary>
        /// ScrollLock キー
        /// </summary>
        Scroll = 145,

        /// <summary>
        /// Select キー。
        /// </summary>
        Select = 41,

        /// <summary>
        /// メディアの選択キー。
        /// </summary>
        SelectMedia = 181,

        /// <summary>
        /// 区切り記号キー
        /// </summary>
        Separator = 108,

        /// <summary>
        /// Shift 修飾子キー
        /// </summary>
        Shift = 65536,

        /// <summary>
        /// Shift キー
        /// </summary>
        ShiftKey = 16,

        /// <summary>
        /// コンピューターのスリープ キー
        /// </summary>
        Sleep = 95,

        /// <summary>
        /// Print Screen キー。
        /// </summary>
        Snapshot = 44,

        /// <summary>
        /// Space キー。
        /// </summary>
        Space = 32,

        /// <summary>
        /// 減算記号 (-) キー
        /// </summary>
        Subtract = 109,

        /// <summary>
        /// T キー。
        /// </summary>
        T = 84,

        /// <summary>
        /// Tab キー。
        /// </summary>
        Tab = 9,

        /// <summary>
        /// U キー。
        /// </summary>
        U = 85,

        /// <summary>
        /// ↑キー。
        /// </summary>
        Up = 38,

        /// <summary>
        /// V キー。
        /// </summary>
        V = 86,

        /// <summary>
        /// 音量下げるキー。
        /// </summary>
        VolumeDown = 174,

        /// <summary>
        /// 音量ミュート キー。
        /// </summary>
        VolumeMute = 173,

        /// <summary>
        /// 音量上げるキー。
        /// </summary>
        VolumeUp = 175,

        /// <summary>
        /// W キー。
        /// </summary>
        W = 87,

        /// <summary>
        /// X キー。
        /// </summary>
        X = 88,

        /// <summary>
        /// x マウスの 1 番目のボタン (5 ボタン マウスの場合)
        /// </summary>
        XButton1 = 5,

        /// <summary>
        /// x マウスの 2 番目のボタン (5 ボタン マウスの場合)
        /// </summary>
        XButton2 = 6,

        /// <summary>
        /// Y キー。
        /// </summary>
        Y = 89,

        /// <summary>
        /// Z キー。
        /// </summary>
        Z = 90,

        /// <summary>
        /// Zoom キー。
        /// </summary>
        Zoom = 251
    }
}
