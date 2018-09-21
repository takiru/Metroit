using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Metroit.Api.Win32;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// MetTextBox 用の表示用ラベルを提供します。
    /// </summary>
    internal class TextBoxLabel : Label
    {
        private MetTextBox target = null;

        /// <summary>
        /// TextBoxLabel の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="control"></param>
        public TextBoxLabel(MetTextBox control)
        {
            this.target = control;
        }

        /// <summary>
        /// ウィンドウメッセージを処理する。
        /// </summary>
        /// <param name = "m" > ウィンドウメッセージ。</param>
        protected override void WndProc(ref Message m)
        {
            // コントロール枠を描画する
            if (m.Msg == WindowMessage.WM_NCPAINT)
            {
                this.target.drawBorder();

                //using (Graphics g = this.Parent.CreateGraphics())
                //{
                //    Rectangle rct = new Rectangle(this.Location, this.Size);
                //    rct.Inflate(1, 1);
                //    ControlPaint.DrawBorder(g, rct, this.target.FocusForeColor, ButtonBorderStyle.Solid);
                //}
            }

            base.WndProc(ref m);
        }
    }
}
