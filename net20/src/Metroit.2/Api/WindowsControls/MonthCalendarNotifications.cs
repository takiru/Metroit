using System;
using System.Collections.Generic;
using System.Text;

namespace Metroit.Api.WindowsControls
{
    /// <summary>
    /// MonthCalenderの受けた通知の種類を提供します。
    /// </summary>
    public static class MonthCalendarNotifications
    {
        /// <summary>
        /// MonthCalendarNotificationsの基準になる値。
        /// </summary>
        private const int MCN_FIRST = -745;

        /// <summary>
        /// 個々の日の表示方法に関する情報を要求するために、月のカレンダーコントロールによって送信されます。この通知コードは、MCS_DAYSTATEスタイルを使用する月暦コントロールによってのみ送信され、WM_NOTIFYメッセージの形式で送信されます。
        /// </summary>
        public const int MCN_GETDAYSTATE = MCN_FIRST - 2;

        /// <summary>
        /// 現在選択されている日付または日付の範囲が変更されたときに、月のカレンダーコントロールによって送信されます。この通知コードは、WM_NOTIFYメッセージの形式で送信されます。
        /// </summary>
        public const int MCN_SELCHANGE = MCN_FIRST - 4;

        /// <summary>
        /// ユーザーが月のカレンダーコントロール内で明示的な日付を選択すると、月のカレンダーコントロールによって送信されます。この通知コードは、WM_NOTIFYメッセージの形式で送信されます。
        /// </summary>
        public const int MCN_SELECT = MCN_FIRST - 1;

        /// <summary>
        /// 現在のビューが変更されたときに、月のカレンダーコントロールによって送信されます。この通知コードは、WM_NOTIFYメッセージの形式で送信されます。
        /// </summary>
        public const int MCN_VIEWCHANGE = MCN_FIRST - 5;

        /// <summary>
        /// コントロールがマウスキャプチャを解放していることを月次コントロールの親ウィンドウに通知します。この通知コードは、WM_NOTIFYメッセージの形式で送信されます。
        /// </summary>
        public const int NM_RELEASEDCAPTURE = -16;


    }
}
