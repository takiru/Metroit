using System;
using System.Collections.Generic;
using System.Text;

namespace Metroit.Api.WindowsControls
{
    /// <summary>
    /// DateTimePickerを操作するメッセージを提供します。
    /// </summary>
    public static class DateTimePickerMessage
    {
        /// <summary>
        /// DateTimePickerMessageの基準になる値。
        /// </summary>
        private const int DTM_FIRST = 0x1000;

        /// <summary>
        /// 日付と時刻のピッカー（DTP）コントロールを閉じます。このメッセージを明示的に送信するか、DateTime_CloseMonthCalマクロを使用して送信します。
        /// </summary>
        public const int DTM_CLOSEMONTHCAL = DTM_FIRST + 13;

        /// <summary>
        /// 日付と時刻のピッカー（DTP）コントロールに関する情報を取得します。
        /// </summary>
        public const int DTM_GETDATETIMEPICKERINFO = DTM_FIRST + 14;

        /// <summary>
        /// クリッピングなしでコントロールを表示するために必要なサイズを取得します。このメッセージを明示的に送信するか、DateTime_GetIdealSizeマクロを使用して送信します。
        /// </summary>
        public const int DTM_GETIDEALSIZE = DTM_FIRST + 15;

        /// <summary>
        /// 日付と時刻のピッカー（DTP）コントロール内の月暦の特定の部分の色を取得します。このメッセージを明示的に送信するか、DateTime_GetMonthCalColorマクロを使用できます。
        /// </summary>
        public const int DTM_GETMCCOLOR = DTM_FIRST + 7;

        /// <summary>
        /// 日付と時刻のピッカー（DTP）コントロールの子月暦コントロールが現在使用しているフォントを取得します。このメッセージを明示的に送信するか、DateTime_GetMonthCalFontマクロを使用できます。
        /// </summary>
        public const int DTM_GETMCFONT = DTM_FIRST + 10;

        /// <summary>
        /// 日付と時刻のピッカー（DTP）コントロールのスタイルを取得します。このメッセージを明示的に送信するか、DateTime_GetMonthCalStyleマクロを使用して送信します。
        /// </summary>
        public const int DTM_GETMCSTYLE = DTM_FIRST + 12;

        /// <summary>
        /// 日付と時刻のピッカー（DTP）の子月暦コントロールへのハンドルを取得します。このメッセージを明示的に送信するか、DateTime_GetMonthCalマクロを使用できます。
        /// </summary>
        public const int DTM_GETMONTHCAL = DTM_FIRST + 8;

        /// <summary>
        /// 日付と時刻のピッカー（DTP）コントロールの現在の最小および最大許容システム時間を取得します。このメッセージを明示的に送信するか、DateTime_GetRangeマクロを使用できます。
        /// </summary>
        public const int DTM_GETRANGE = DTM_FIRST + 3;

        /// <summary>
        /// 日付と時刻のピッカー（DTP）コントロールから現在選択されている時刻を取得し、指定されたSYSTEMTIME構造体に配置します。このメッセージを明示的に送信するか、DateTime_GetSystemtimeマクロを使用できます。
        /// </summary>
        public const int DTM_GETSYSTEMTIME = DTM_FIRST + 1;

        /// <summary>
        /// 指定されたフォーマット文字列に基づいて、日付と時刻のピッカー（DTP）コントロールの表示を設定します。このメッセージを明示的に送信するか、DateTime_SetFormatマクロを使用できます。
        /// </summary>
        public const int DTM_SETFORMAT = DTM_FIRST + 5;

        /// <summary>
        /// 日付と時刻のピッカー（DTP）コントロール内で、月のカレンダーの特定の部分の色を設定します。このメッセージを明示的に送信するか、DateTime_SetMonthCalColorマクロを使用できます。
        /// </summary>
        public const int DTM_SETMCCOLOR = DTM_FIRST + 6;

        /// <summary>
        /// 日付と時刻のピッカー（DTP）コントロールの子月暦コントロールで使用されるフォントを設定します。このメッセージを明示的に送信するか、DateTime_SetMonthCalFontマクロを使用できます。
        /// </summary>
        public const int DTM_SETMCFONT = DTM_FIRST + 9;

        /// <summary>
        /// 日付と時刻のピッカー（DTP）コントロールのスタイルを設定します。このメッセージを明示的に送信するか、DateTime_SetMonthCalStyleマクロを使用して送信します。
        /// </summary>
        public const int DTM_SETMCSTYLE = DTM_FIRST + 11;

        /// <summary>
        /// 日付と時刻のピッカー（DTP）コントロールの最小および最大許容システム時間を設定します。このメッセージを明示的に送信するか、DateTime_SetRangeマクロを使用できます。
        /// </summary>
        public const int DTM_SETRANGE = DTM_FIRST + 4;

        /// <summary>
        /// 日付と時刻のピッカー（DTP）コントロールで時刻を設定します。このメッセージを明示的に送信するか、DateTime_SetSystemtimeマクロを使用できます。
        /// </summary>
        public const int DTM_SETSYSTEMTIME = DTM_FIRST + 2;
    }
}
