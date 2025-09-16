using System;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// <see cref="MetNotificationPanel"/> から <see cref="NotificationInfo"/> を操作するときのパラメーターを提供します。
    /// </summary>
    public class NotificationInfoParameter
    {
        /// <summary>
        /// 通知の日時を取得または設定します。
        /// </summary>
        public DateTime? DateTime { get; }

        /// <summary>
        /// 通知のテキストを取得または設定します。
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// 通知に関するパラメーターを取得または設定します。
        /// </summary>
        public object Args { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="dateTime">通知日時。</param>
        /// <param name="text">通知テキスト。</param>
        /// <param name="args">通知に関するパラメーター。</param>
        internal NotificationInfoParameter(DateTime? dateTime, string text, object args)
        {
            DateTime = dateTime;
            Text = text;
            Args = args;
        }
    }
}
