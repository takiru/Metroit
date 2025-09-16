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
        public DateTime? DateTime { get; } = null;

        /// <summary>
        /// 通知のテキストを取得または設定します。
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// 通知に関するパラメーターを取得または設定します。
        /// </summary>
        public object Args { get; set; }

        /// <summary>
        /// 通知が登録されたときの制御を取得または設定します。
        /// </summary>
        public Action<NotificationInfoParameter> Registered { get; set; }

        /// <summary>
        /// 通知が選択されたときの制御を取得または設定します。
        /// </summary>
        public Action<NotificationInfoParameter> Selected { get; set; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="dateTime">通知日時。</param>
        /// <param name="text">通知テキスト。</param>
        /// <exception cref="ArgumentNullException">通知テキストが <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException">通知テキストが空です。</exception>
        public NotificationInfo(DateTime? dateTime, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException(string.Format(ExceptionResources.GetString("Empty"), nameof(text)));
            }

            Text = text;
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="text">通知テキスト。</param>
        /// <exception cref="ArgumentNullException">通知テキストが <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException">通知テキストが空です。</exception>
        public NotificationInfo(string text) : this(null, text) { }
    }
}
