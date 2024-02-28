namespace Metroit.Win32.DesktopAppUi.WindowsControls
{
    /// <summary>
    /// MonthCalendarを操作するメッセージを提供します。
    /// </summary>
    public static class MonthCalendarMessages
    {
        /// <summary>
        /// MonthCalenderMessageの基準になる値。
        /// </summary>
        private const int MCM_FIRST = 0x1000;

        /// <summary>
        /// 境界線のサイズをピクセル単位で取得します。このメッセージは、明示的に送信することも、MonthCal_GetCurrentViewマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETCALENDARBORDER = MCM_FIRST + 31;

        /// <summary>
        /// カレンダーコントロールに現在表示されているカレンダーの数を取得します。このメッセージは、明示的に送信することも、MonthCal_GetCalendarCountマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETCALENDARCOUNT = MCM_FIRST + 23;

        /// <summary>
        /// カレンダーグリッドに関する情報を取得します。
        /// </summary>
        public const int MCM_GETCALENDARGRIDINFO = MCM_FIRST + 24;

        /// <summary>
        /// 指定されたカレンダーコントロールのカレンダーIDを取得します。このメッセージは、明示的に送信することも、MonthCal_GetCALIDマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETCALID = MCM_FIRST + 27;

        /// <summary>
        /// 月のカレンダーコントロールの特定の部分の色を取得します。このメッセージは、明示的に送信することも、MonthCal_GetColorマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETCOLOR = MCM_FIRST + 11;

        /// <summary>
        /// カレンダーの現在のビューを取得します。このメッセージは、明示的に送信することも、MonthCal_GetCurrentViewマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETCURRENTVIEW = MCM_FIRST + 22;

        /// <summary>
        /// 現在選択されている日付を取得します。このメッセージは、明示的に送信することも、MonthCal_GetCurSelマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETCURSEL = MCM_FIRST + 1;

        /// <summary>
        /// 月のカレンダーコントロールの最初の曜日を取得します。このメッセージは、明示的に送信することも、MonthCal_GetFirstDayOfWeekマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETFIRSTDAYOFWEEK = MCM_FIRST + 16;

        /// <summary>
        /// 月のカレンダーコントロールで選択できる最大の日付範囲を取得します。このメッセージは、明示的に送信することも、MonthCal_GetMaxSelCountマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETMAXSELCOUNT = MCM_FIRST + 3;

        /// <summary>
        /// 月暦コントロールの「今日」の文字列の最大幅を取得します。これには、ラベルテキストと日付テキストが含まれます。このメッセージは、明示的に送信することも、MonthCal_GetMaxTodayWidthマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETMAXTODAYWIDTH = MCM_FIRST + 21;

        /// <summary>
        /// 月のカレンダーコントロールで1か月全体を表示するために必要な最小サイズを取得します。このメッセージは、明示的に送信することも、MonthCal_GetMinReqRectマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETMINREQRECT = MCM_FIRST + 9;

        /// <summary>
        /// 月のカレンダーコントロールのスクロール速度を取得します。スクロールレートは、ユーザーがスクロールボタンをクリックしたときにコントロールがディスプレイを移動する月数です。このメッセージは、明示的に送信することも、MonthCal_GetMonthDeltaマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETMONTHDELTA = MCM_FIRST + 19;

        /// <summary>
        /// 月のカレンダーコントロールの表示の上限と下限を表す日付情報を（SYSTEMTIME構造を使用して）取得します。このメッセージは、明示的に送信することも、MonthCal_GetMonthRangeマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETMONTHRANGE = MCM_FIRST + 7;

        /// <summary>
        /// 月のカレンダーコントロールに設定されている最小および最大の許容日を取得します。このメッセージは、明示的に送信することも、MonthCal_GetRangeマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETRANGE = MCM_FIRST + 17;

        /// <summary>
        /// ユーザーが現在選択している日付範囲の上限と下限を表す日付情報を取得します。このメッセージは、明示的に送信することも、MonthCal_GetSelRangeマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETSELRANGE = MCM_FIRST + 5;

        /// <summary>
        /// 月のカレンダーコントロールの「今日」として指定された日付の日付情報を取得します。このメッセージは、明示的に送信することも、MonthCal_GetTodayマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_GETTODAY = MCM_FIRST + 13;

        /// <summary>
        /// コントロールのUnicode文字フォーマットフラグを取得します。このメッセージを明示的に送信するか、MonthCal_GetUnicodeFormatマクロを使用できます。
        /// </summary>
        public const int MCM_GETUNICODEFORMAT = 0x2000 + 6;

        /// <summary>
        /// 月のカレンダーコントロールのどの部分が画面の特定のポイントにあるかを決定します。このメッセージは、明示的に送信することも、MonthCal_HitTestマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_HITTEST = MCM_FIRST + 14;

        /// <summary>
        /// 境界線のサイズをピクセル単位で設定します。このメッセージは、明示的に送信することも、MonthCal_SetCalendarBorderマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_SETCALENDARBORDER = MCM_FIRST + 30;

        /// <summary>
        /// 指定されたカレンダーコントロールのカレンダーIDを設定します。このメッセージは、明示的に送信することも、MonthCal_SetCALIDマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_SETCALID = MCM_FIRST + 28;

        /// <summary>
        /// 月のカレンダーコントロールの特定の部分の色を設定します。このメッセージは、明示的に送信することも、MonthCal_SetColorマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_SETCOLOR = MCM_FIRST + 10;

        /// <summary>
        /// カレンダーの現在のビューを設定します。このメッセージは、明示的に送信することも、MonthCal_SetCurrentViewマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_SETCURRENTVIEW = MCM_FIRST + 32;

        /// <summary>
        /// 月のカレンダーコントロールに現在選択されている日付を設定します。指定した日付が表示されていない場合、コントロールは表示を更新して表示します。このメッセージは、明示的に送信することも、MonthCal_SetCurSelマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_SETCURSEL = MCM_FIRST + 2;

        /// <summary>
        /// 月のカレンダーコントロール内に現在表示されているすべての月の日の状態を設定します。このメッセージは、明示的に送信することも、MonthCal_SetDayStateマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_SETDAYSTATE = MCM_FIRST + 8;

        /// <summary>
        /// 月のカレンダーコントロールの最初の曜日を設定します。このメッセージは、明示的に送信することも、MonthCal_SetFirstDayOfWeekマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_SETFIRSTDAYOFWEEK = MCM_FIRST + 15;

        /// <summary>
        /// 月間カレンダーコントロールで選択できる最大日数を設定します。このメッセージは、明示的に送信することも、MonthCal_SetMaxSelCountマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_SETMAXSELCOUNT = MCM_FIRST + 4;

        /// <summary>
        /// 月間カレンダーコントロールのスクロール速度を設定します。スクロールレートは、ユーザーがスクロールボタンをクリックしたときにコントロールがディスプレイを移動する月数です。このメッセージは、明示的に送信することも、MonthCal_SetMonthDeltaマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_SETMONTHDELTA = MCM_FIRST + 20;

        /// <summary>
        /// 月のカレンダーコントロールの最小許容日と最大許容日を設定します。このメッセージは、明示的に送信することも、MonthCal_SetRangeマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_SETRANGE = MCM_FIRST + 18;

        /// <summary>
        /// 月のカレンダーコントロールの選択を特定の日付範囲に設定します。このメッセージは、明示的に送信することも、MonthCal_SetSelRangeマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_SETSELRANGE = MCM_FIRST + 6;

        /// <summary>
        /// 月のカレンダーコントロールの「今日」の選択を設定します。このメッセージは、明示的に送信することも、MonthCal_SetTodayマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_SETTODAY = MCM_FIRST + 12;

        /// <summary>
        /// コントロールのUnicode文字フォーマットフラグを設定します。このメッセージを使用すると、コントロールを再作成しなくても、実行時にコントロールが使用する文字セットを変更できます。このメッセージを明示的に送信するか、MonthCal_SetUnicodeFormatマクロを使用できます。
        /// </summary>
        public const int MCM_SETUNICODEFORMAT = 0x2000 + 5;

        /// <summary>
        /// 指定された長方形に収まるカレンダーの数を計算し、その数のカレンダーに収まるために長方形が必要とする最小サイズを返します。このメッセージは、明示的に送信することも、MonthCal_SizeRectToMinマクロを使用して送信することもできます。
        /// </summary>
        public const int MCM_SIZERECTTOMIN = MCM_FIRST + 29;
    }
}
