using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    internal class ReadOnlyLabel : Label
    {
        public event EventHandler RedrawLabelOuterFrame;

        private MetTextBox target = null;

        public ReadOnlyLabel() { }

        /// <summary>
        /// TextBoxLabel の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="control"></param>
        public ReadOnlyLabel(MetTextBox control)
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
                //this.RedrawLabelOuterFrame(this, EventArgs.Empty);
                this.redrawColor();

                //this.target.drawBorder();

                //using (Graphics g = this.Parent.CreateGraphics())
                //{
                //    Rectangle rct = new Rectangle(this.Location, this.Size);
                //    rct.Inflate(1, 1);
                //    //ControlPaint.DrawBorder(g, rct, this.target.FocusForeColor, ButtonBorderStyle.Solid);
                //    ControlPaint.DrawBorder(g, rct, Color.Red, ButtonBorderStyle.Solid);
                //}
            }

            base.WndProc(ref m);
        }


        /// <summary>
        /// 背景色・文字色の描画し直します。
        /// </summary>
        private void redrawColor()
        {
            // Bitmapを自分の上に描画して背景色を設定する
            var bsz = SystemInformation.Border3DSize;
            using (var g = this.Parent.CreateGraphics())
            using (var bmp = this.getControlImage())
            {
                //g.DrawImage(bmp, -bsz.Width + 2, -bsz.Height + 2);
                g.DrawImage(bmp, this.Location.X, this.Location.Y);
            }
        }

        /// <summary>
        /// 背景色・文字色描画用のBitmapオブジェクトを取得する。
        /// </summary>
        /// <returns></returns>
        private Bitmap getControlImage()
        {
            // 自分自身の画像をBitmapにコピー
            var bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));

            // Bitmapの背景色をMe.BackColorに変更する
            using (var g = Graphics.FromImage(bmp))
            {
                System.Drawing.Imaging.ColorMap[] cm = { new System.Drawing.Imaging.ColorMap(), new System.Drawing.Imaging.ColorMap() };

                // 背景色のマッピング
                cm[0].OldColor = SystemColors.Window;
                cm[0].NewColor = this.BackColor;

                // 文字色のマッピング
                cm[1].OldColor = SystemColors.WindowText;
                cm[1].NewColor = this.ForeColor;

                // 背景色・文字色の変更
                var ia = new System.Drawing.Imaging.ImageAttributes();
                ia.SetRemapTable(cm);
                var r = new Rectangle(0, 0, bmp.Width, bmp.Height);
                g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height),
                            0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, ia);

                // 外枠の変更
                //var frameColor = this.BaseOuterFrameColor;
                //var form = this.FindForm();
                //if (form != null && form.ActiveControl == this)
                //{
                //    frameColor = this.FocusOuterFrameColor;
                //}
                //frameColor = this.Error ? this.ErrorOuterFrameColor : frameColor;

                var frameColor = Color.Red;
                g.DrawRectangle(new Pen(frameColor), new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            return bmp;
        }




        ///// <summary>
        ///// コントロールの枠色を取得または設定します。
        ///// </summary>
        //[Browsable(true)]
        //[DefaultValue(typeof(Color), "Transparent")]
        //[MetCategory("MetAppearance")]
        //[MetDescription("ControlBaseOuterFrameColor")]
        //public Color BaseOuterFrameColor { get; set; } = Color.Transparent;

        ///// <summary>
        ///// フォーカス時のコントロールの枠色を取得または設定します。
        ///// </summary>
        //[Browsable(true)]
        //[DefaultValue(typeof(Color), "Transparent")]
        //[MetCategory("MetAppearance")]
        //[MetDescription("ControlFocusOuterFrameColor")]
        //public Color FocusOuterFrameColor { get; set; } = Color.Transparent;

        ///// <summary>
        ///// エラー時のコントロールの枠色を取得または設定します。
        ///// </summary>
        //[Browsable(true)]
        //[DefaultValue(typeof(Color), "Red")]
        //[MetCategory("MetAppearance")]
        //[MetDescription("ControlErrorOuterFrameColor")]
        //public Color ErrorOuterFrameColor { get; set; } = Color.Red;

        //private bool error = false;

        ///// <summary>
        ///// コントロールがエラーかどうかを取得または設定します。
        ///// </summary>
        //[Browsable(true)]
        //[DefaultValue(false)]
        //[MetCategory("MetAppearance")]
        //[MetDescription("ControlError")]
        //public bool Error
        //{
        //    get => this.error;
        //    set
        //    {
        //        this.error = value;
        //        this.RedrawLabelOuterFrame(this, EventArgs.Empty);
        //    }
        //}
    }
}
