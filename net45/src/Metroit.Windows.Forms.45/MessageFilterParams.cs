using System.Collections.Generic;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// ウィンドウメッセージフィルタを実施するパラメーターを提供します。
    /// </summary>
    public class MessageFilterParams
    {
        /// <summary>
        /// 新しい MessageFilterParams インスタンスを生成します。
        /// </summary>
        public MessageFilterParams() { }

        /// <summary>
        /// 新しい MessageFilterParams インスタンスを生成します。
        /// </summary>
        /// <param name="messages">ウィンドウメッセージ。</param>
        public MessageFilterParams(params int[] messages)
        {
            var messagesList = new List<int>();
            messagesList.AddRange(messages);
            this.Messages = messagesList;
        }

        /// <summary>
        /// ウィンドウメッセージフィルタを動作させるコントロールを取得または設定します。
        /// </summary>
        public List<Control> Controls { get; set; } = new List<Control>();

        /// <summary>
        /// ウィンドウメッセージフィルタを動作させるウィンドウメッセージを取得または設定します。
        /// </summary>
        public List<int> Messages { get; set; } = new List<int>();

        /// <summary>
        /// ウィンドウメッセージフィルタを動作させるWParamを取得または設定します。
        /// 設定しなかった場合、すべてのWParamが動作対象となります。
        /// </summary>
        public List<int> WParam { get; set; } = new List<int>();

        /// <summary>
        /// ウィンドウメッセージフィルタを動作させるLParamを取得または設定します。
        /// 設定しなかった場合、すべてのLParamが動作対象となります。
        /// </summary>
        public List<int> LParam { get; set; } = new List<int>();

        // 実行する命令
        /// <summary>
        /// ウィンドウメッセージフィルタで制御したい制御を取得または設定します。
        /// </summary>
        public MessageFilterAction Action { get; set; } = null;
    }
}
