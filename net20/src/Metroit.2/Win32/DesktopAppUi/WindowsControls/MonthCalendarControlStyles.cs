using System;
using System.Collections.Generic;
using System.Text;

namespace Metroit.Win32.DesktopAppUi.WindowsControls
{
    /// <summary>
    /// MonthCalendarのスタイル種類を提供します。
    /// </summary>
    public static class MonthCalendarControlStyles
    {
        /// <summary>
        /// 月間カレンダーはMCN_GETDAYSTATE通知を送信して、太字で表示する日に関する情報を要求します。
        /// </summary>
        public const int MCS_DAYSTATE = 0x0001;

        /// <summary>
        /// 月間カレンダーを使用すると、ユーザーはコントロール内の日付の範囲を選択できます。デフォルトでは、最大範囲は1週間です。MCM_SETMAXSELCOUNTメッセージを使用して、選択できる最大範囲を変更できます。
        /// </summary>
        public const int MCS_MULTISELECT = 0x0002;

        /// <summary>
        /// 月暦コントロールは、日の各行の左側に週番号（1〜52）を表示します。第1週は、少なくとも4日を含む最初の週として定義されます。
        /// </summary>
        public const int MCS_WEEKNUMBERS = 0x0004;

        /// <summary>
        /// 月のカレンダーコントロールは、「今日」の日付を丸で囲みません。
        /// </summary>
        public const int MCS_NOTODAYCIRCLE = 0x0008;

        /// <summary>
        /// 月暦コントロールは、コントロールの下部に「今日」の日付を表示しません。
        /// </summary>
        public const int MCS_NOTODAY = 0x0010;

        /// <summary>
        /// 前月と翌月の日付は、当月のカレンダーには表示されません。
        /// </summary>
        public const int MCS_NOTRAILINGDATES = 0x0040;

        /// <summary>
        /// 短い日の名前がヘッダーに表示されます。
        /// </summary>
        public const int MCS_SHORTDAYSOFWEEK = 0x0080;

        /// <summary>
        /// ユーザーがカレンダーの次または前に移動しても、選択は変更されません。これにより、ユーザーは表示されているよりも広い範囲を選択できます。
        /// </summary>
        public const int MCS_NOSELCHANGEONNAV = 0x0100;
    }
}
