using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    class TextBoxEx : TextBox
    {
        private const int WM_PAINT = 0x000F;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
            {
                
                using (var g = this.CreateGraphics())
                using (var pen = new Pen(Color.Red))
                {
                    var rect = this.ClientRectangle;

                    // 枠線描画
                    g.DrawRectangle(pen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                    pen.Dispose();

                    // ウォーターマーク描画
                    g.DrawString("watermark", this.Font, new SolidBrush(Color.LightGray), rect.X, rect.Y, StringFormat.GenericTypographic);
                }
                return;
            }
        }
    }
}
