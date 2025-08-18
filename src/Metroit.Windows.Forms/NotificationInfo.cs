using System;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 通知情報を提供します。
    /// </summary>
    public class NotificationInfo
    {
        /// <summary>
        /// 通知の日時を取得または設定します。
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 通知のテキストを取得または設定します。
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 通知が登録されたときの制御を取得または設定します。
        /// </summary>
        public Action Registered { get; set; }

        /// <summary>
        /// 通知が選択されたときの制御を取得または設定します。
        /// </summary>
        public Action Selected { get; set; }
    }
}
