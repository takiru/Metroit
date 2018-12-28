using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// ウィンドウメッセージのフィルタリングを提供します。
    /// </summary>
    public class MessageFilter : IMessageFilter
    {
        private MessageFilterParams parameter = null;
        private List<IntPtr> controlHandles = new List<IntPtr>();

        /// <summary>
        /// 新しい MessageFilter インスタンスを生成します。
        /// </summary>
        /// <param name="parameter"></param>
        public MessageFilter(MessageFilterParams parameter)
        {
            this.parameter = parameter;

            // コントロールのハンドルを取得
            foreach (var control in this.parameter.Controls)
            {
                this.controlHandles.Add(control.Handle);
            }
        }

        /// <summary>
        /// 指定した条件でフィルタリングを実施します。
        /// </summary>
        /// <param name="m">Message オブジェクト。</param>
        /// <returns></returns>
        public bool PreFilterMessage(ref Message m)
        {
            // 捕捉したハンドルが制御対象じゃない時は何もしない
            if (!this.controlHandles.Contains(m.HWnd))
            {
                return false;
            }

            // 捕捉したメッセージが制御対象じゃない時は何もしない
            if (!this.parameter.Messages.Contains(m.Msg))
            {
                return false;
            }

            // 捕捉したWParamが制御対象じゃない時は何もしない
            if (this.parameter.WParam.Count > 0 && !this.parameter.WParam.Contains(m.WParam.ToInt32()))
            {
                return false;
            }

            // 捕捉したLParamが制御対象じゃない時は何もしない
            if (this.parameter.LParam.Count > 0 && !this.parameter.LParam.Contains(m.LParam.ToInt32()))
            {
                return false;
            }

            var e = new MessageFilterActionParams(m);
            this.parameter.Action(e);
            return e.StopMessage;
        }
    }
}
