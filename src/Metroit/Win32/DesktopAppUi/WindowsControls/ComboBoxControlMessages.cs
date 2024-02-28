namespace Metroit.Win32.DesktopAppUi.WindowsControls
{
    /// <summary>
    /// ComboBoxを操作するメッセージを提供します。
    /// </summary>
    public static class ComboBoxControlMessages
    {
        /// <summary>
        /// コンボボックスのリストボックスに文字列を追加します。コンボボックスにCBS_SORTスタイルがない場合、文字列はリストの最後に追加されます。それ以外の場合は、文字列がリストに挿入され、リストが並べ替えられます。
        /// </summary>
        public const int CB_ADDSTRING = 0x0143;

        /// <summary>
        /// コンボボックスのリストボックス内の文字列を削除します。
        /// </summary>
        public const int CB_DELETESTRING = 0x0144;

        /// <summary>
        /// コンボボックスに表示されるリストに名前を追加します。このメッセージは、指定された文字列とファイル属性のセットに一致するディレクトリとファイルの名前を追加します。CB_DIRは、マップされたドライブ文字をリストに追加することもできます。
        /// </summary>
        public const int CB_DIR = 0x0145;

        /// <summary>
        /// コンボボックスのリストボックスで、指定した文字列の文字で始まる項目を検索します。
        /// </summary>
        public const int CB_FINDSTRING = 0x014c;

        /// <summary>
        /// lParamパラメーターで指定された文字列と一致するコンボボックス内の最初のリストボックス文字列を検索します。
        /// </summary>
        public const int CB_FINDSTRINGEXACT = 0x0158;

        /// <summary>
        /// 指定されたコンボボックスに関する情報を取得します。
        /// </summary>
        public const int CB_GETCOMBOBOXINFO = 0x0164;

        /// <summary>
        /// コンボボックスのリストボックス内のアイテムの数を取得します。
        /// </summary>
        public const int CB_GETCOUNT = 0x0146;

        /// <summary>
        /// コンボボックスのエディットコントロールに表示されるキューバナーテキストを取得します。このメッセージを明示的に送信するか、ComboBox_GetCueBannerTextマクロを使用して送信します。
        /// </summary>
        public const int CB_GETCUEBANNER = 0x1704;

        /// <summary>
        /// アプリケーションはCB_GETCURSELメッセージを送信して、コンボボックスのリストボックスで現在選択されているアイテムのインデックスを取得します（存在する場合）。
        /// </summary>
        public const int CB_GETCURSEL = 0x0147;

        /// <summary>
        /// アプリケーションは、CB_GETDROPPEDCONTROLRECTメッセージを送信して、ドロップダウン状態のコンボボックスの画面座標を取得します。
        /// </summary>
        public const int CB_GETDROPPEDCONTROLRECT = 0x0152;

        /// <summary>
        /// コンボボックスのリストボックスをドロップダウンするかどうかを決定します。
        /// </summary>
        public const int CB_GETDROPPEDSTATE = 0x0157;

        /// <summary>
        /// CBS_DROPDOWNまたはCBS_DROPDOWNLISTスタイルのコンボボックスのリストボックスの最小許容幅をピクセル単位で取得します。
        /// </summary>
        public const int CB_GETDROPPEDWIDTH = 0x015f;

        /// <summary>
        /// コンボボックスの編集コントロールで、現在の選択範囲の開始文字と終了文字の位置を取得します。
        /// </summary>
        public const int CB_GETEDITSEL = 0x0140;

        /// <summary>
        /// コンボボックスにデフォルトのユーザーインターフェイスがあるか、拡張ユーザーインターフェイスがあるかを決定します。
        /// </summary>
        public const int CB_GETEXTENDEDUI = 0x0156;

        /// <summary>
        /// リストボックスを水平方向にスクロールできる幅（スクロール可能な幅）をピクセル単位で取得します。これは、リストボックスに水平スクロールバーがある場合にのみ適用されます。
        /// </summary>
        public const int CB_GETHORIZONTALEXTENT = 0x015d;

        /// <summary>
        /// アプリケーションは、CB_GETITEMDATAメッセージをコンボボックスに送信して、コンボボックス内の指定された項目に関連付けられたアプリケーション提供の値を取得します。
        /// </summary>
        public const int CB_GETITEMDATA = 0x0150;

        /// <summary>
        /// リストアイテムまたはコンボボックスの選択フィールドの高さを決定します。
        /// </summary>
        public const int CB_GETITEMHEIGHT = 0x0154;

        /// <summary>
        /// コンボボックスのリストから文字列を取得します。
        /// </summary>
        public const int CB_GETLBTEXT = 0x0148;

        /// <summary>
        /// コンボボックスのリスト内の文字列の長さを文字数で取得します。
        /// </summary>
        public const int CB_GETLBTEXTLEN = 0x0149;

        /// <summary>
        /// コンボボックスの現在のロケールを取得します。ロケールは、CBS_SORTスタイルのコンボボックスに表示されるテキストと、CB_ADDSTRINGメッセージを使用して追加されたテキストの正しいソート順を決定するために使用されます。
        /// </summary>
        public const int CB_GETLOCALE = 0x015a;

        /// <summary>
        /// コンボボックスのドロップダウンリストに表示されるアイテムの最小数を取得します。
        /// </summary>
        public const int CB_GETMINVISIBLE = 0x1702;

        /// <summary>
        /// アプリケーションはCB_GETTOPINDEXメッセージを送信して、コンボボックスのリストボックス部分にある最初の表示項目のゼロベースのインデックスを取得します。最初は、インデックス0のアイテムがリストボックスの一番上にありますが、リストボックスの内容がスクロールされている場合は、別のアイテムが一番上にある可能性があります。
        /// </summary>
        public const int CB_GETTOPINDEX = 0x015b;

        /// <summary>
        /// アプリケーションは、コンボボックスのリストボックス部分に多数の項目を追加する前に、CB_INITSTORAGEメッセージを送信します。このメッセージは、リストボックスアイテムを格納するためのメモリを割り当てます。
        /// </summary>
        public const int CB_INITSTORAGE = 0x0161;

        /// <summary>
        /// 文字列またはアイテムデータをコンボボックスのリストに挿入します。異なりCB_ADDSTRINGのメッセージ、CB_INSERTSTRINGのメッセージを持つリスト起こさないCBS_SORTのスタイルをソートします。
        /// </summary>
        public const int CB_INSERTSTRING = 0x014a;

        /// <summary>
        /// ユーザーがコンボボックスの編集コントロールに入力できるテキストの長さを制限します。
        /// </summary>
        public const int CB_LIMITTEXT = 0x0141;

        /// <summary>
        /// リストボックスからすべての項目を削除し、コンボボックスのコントロールを編集します。
        /// </summary>
        public const int CB_RESETCONTENT = 0x014b;

        /// <summary>
        /// コンボボックスのリストで、指定された文字列の文字で始まる項目を検索します。一致するアイテムが見つかった場合、それが選択され、編集コントロールにコピーされます。
        /// </summary>
        public const int CB_SELECTSTRING = 0x014d;

        /// <summary>
        /// コンボボックスの編集コントロールに表示されるキューバナーテキストを設定します。
        /// </summary>
        public const int CB_SETCUEBANNER = 0x1703;

        /// <summary>
        /// アプリケーションはCB_SETCURSELメッセージを送信して、コンボボックスのリストから文字列を選択します。必要に応じて、リストは文字列をスクロールして表示します。コンボボックスの編集コントロールのテキストは、新しい選択を反映するように変更され、リスト内の以前の選択はすべて削除されます。
        /// </summary>
        public const int CB_SETCURSEL = 0x014e;

        /// <summary>
        /// アプリケーションは、CB_SETDROPPEDWIDTHメッセージを送信して、CBS_DROPDOWNまたはCBS_DROPDOWNLISTスタイルのコンボボックスのリストボックスの最小許容幅をピクセル単位で設定します。
        /// </summary>
        public const int CB_SETDROPPEDWIDTH = 0x0160;

        /// <summary>
        /// アプリケーションはCB_SETEDITSELメッセージを送信して、コンボボックスの編集コントロール内の文字を選択します。
        /// </summary>
        public const int CB_SETEDITSEL = 0x0142;

        /// <summary>
        /// アプリケーションはCB_SETEXTENDEDUIメッセージを送信して、CBS_DROPDOWNまたはCBS_DROPDOWNLISTスタイルのコンボボックスのデフォルトUIまたは拡張UIのいずれかを選択します。
        /// </summary>
        public const int CB_SETEXTENDEDUI = 0x0155;

        /// <summary>
        /// アプリケーションは、CB_SETHORIZONTALEXTENTメッセージを送信して、リストボックスを水平方向にスクロールできる幅（スクロール可能な幅）をピクセル単位で設定します。リストボックスの幅がこの値よりも小さい場合、水平スクロールバーはリストボックス内のアイテムを水平方向にスクロールします。リストボックスの幅がこの値以上の場合、水平スクロールバーは非表示になり、コンボボックスのスタイルがCBS_DISABLENOSCROLLスタイルの場合は無効になります。
        /// </summary>
        public const int CB_SETHORIZONTALEXTENT = 0x015e;

        /// <summary>
        /// アプリケーションはCB_SETITEMDATAメッセージを送信して、コンボボックス内の指定されたアイテムに関連付けられた値を設定します。
        /// </summary>
        public const int CB_SETITEMDATA = 0x0151;

        /// <summary>
        /// アプリケーションは、CB_SETITEMHEIGHTメッセージを送信して、リスト項目の高さまたはコンボボックスの選択フィールドを設定します。
        /// </summary>
        public const int CB_SETITEMHEIGHT = 0x0153;

        /// <summary>
        /// アプリケーションはCB_SETLOCALEメッセージを送信して、コンボボックスの現在のロケールを設定します。コンボボックスがCBS_SORTスタイルであり、文字列がCB_ADDSTRINGを使用して追加される場合、コンボボックスのロケールは、リスト項目のソート方法に影響します。
        /// </summary>
        public const int CB_SETLOCALE = 0x0159;

        /// <summary>
        /// アプリケーションはCB_SETMINVISIBLEメッセージを送信して、コンボボックスのドロップダウンリストに表示されるアイテムの最小数を設定します。
        /// </summary>
        public const int CB_SETMINVISIBLE = 0x1701;

        /// <summary>
        /// アプリケーションはCB_SETTOPINDEXメッセージを送信して、特定の項目がコンボボックスのリストボックスに表示されるようにします。指定された項目がリストボックスの上部に表示されるか、最大スクロール範囲に達するように、リストボックスの内容がスクロールされます。
        /// </summary>
        public const int CB_SETTOPINDEX = 0x015c;

        /// <summary>
        /// アプリケーションは、CB_SHOWDROPDOWNメッセージを送信して、CBS_DROPDOWNまたはCBS_DROPDOWNLISTスタイルのコンボボックスのリストボックスを表示または非表示にします。
        /// </summary>
        public const int CB_SHOWDROPDOWN = 0x014f;
    }
}
