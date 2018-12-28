using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// MessageFilterAction を実行するパラメーターを提供します。
    /// </summary>
    public class MessageFilterActionParams
    {
        /// <summary>
        /// 新しい MessageFilterActionParams インスタンスを生成します。
        /// </summary>
        /// <param name="m">Message オブジェクト。</param>
        public MessageFilterActionParams(Message m)
        {
            this.Message = m;
        }

        /// <summary>
        /// ウィンドウメッセージの通知を停止するかどうかを指定します。
        /// 指定しない場合、ウィンドウメッセージの通知を停止しません。
        /// </summary>
        public bool StopMessage { get; set; } = false;

        /// <summary>
        /// 通知されたウィンドウメッセージを取得します。
        /// </summary>
        public Message Message { get; }
    }
}
