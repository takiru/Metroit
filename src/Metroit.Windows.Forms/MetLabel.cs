using Metroit.Win32.Api;
using Metroit.Win32.Api.WindowsGdi.WinUser;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 標準Labelを拡張し、新たにいくつかの機能を設けたテキストエリアを提供します。
    /// </summary>
    /// <remarks>
    /// [拡張機能]<br />
    /// 　・線の描画。
    /// </remarks>
    [ToolboxItem(true)]
    public class MetLabel : Label
    {
        /// <summary>
        /// MetLabel の新しいインスタンスを初期化します。
        /// </summary>
        public MetLabel() : base()
        {
            if (Environment.StackTrace.Contains("System.Windows.Forms.Design.ControlDesigner"))
            {
                this.AutoSize = true;
            }
        }

        private LinePosition linePosition = LinePosition.None;

        /// <summary>
        /// 描画する線の位置を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [DefaultValue(typeof(LinePosition), "None")]
        [MetDescription("ControlLinePosition")]
        public LinePosition LinePosition
        {
            get => linePosition;
            set
            {
                linePosition = value;
                Invalidate();
            }
        }

        private DashStyle lineDashStyle = DashStyle.Solid;

        /// <summary>
        /// 描画する線の種類を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [DefaultValue(typeof(DashStyle), "Solid")]
        [MetDescription("ControlLineDashStyle")]
        public DashStyle LineDashStyle
        {
            get => lineDashStyle;
            set
            {
                // Custom は指定不可とする
                lineDashStyle = value == DashStyle.Custom ? DashStyle.Solid : value;
                Invalidate();
            }
        }

        private Color lineColor = Color.FromArgb(100, 100, 100);

        /// <summary>
        /// 描画する線の色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [DefaultValue(typeof(Color), "100, 100, 100")]
        [MetDescription("ControlLineColor")]
        public Color LineColor
        {
            get => lineColor;
            set
            {
                lineColor = value;
                Invalidate();
            }
        }

        private float lineWidth = 1.0f;

        /// <summary>
        /// 描画する線の幅を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [DefaultValue(1.0f)]
        [MetDescription("ControlLineWidth")]
        public float LineWidth
        {
            get => lineWidth;
            set
            {
                lineWidth = value;
                Invalidate();
            }
        }

        /// <summary>
        /// ウィンドウメッセージを捕捉し、線の描画を行います。
        /// </summary>
        /// <param name="m">ウィンドウメッセージ。</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WindowMessage.WM_PAINT)
            {
                this.drawPaint(ref m);
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// プロパティ設定された情報を描画する。
        /// </summary>
        private void drawPaint(ref Message m)
        {
            using (var bmp = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height))
            using (var bmpGraphics = Graphics.FromImage(bmp))
            {
                // bitmap に描画してもらう
                var bmphdc = bmpGraphics.GetHdc();
                var msg = Message.Create(m.HWnd, WindowMessage.WM_PAINT, bmphdc, IntPtr.Zero);
                base.WndProc(ref msg);
                bmpGraphics.ReleaseHdc();

                this.drawBitmap(bmp, bmpGraphics);

                // コントロールへ描画
                var ps = new PaintStruct();
                var controlHdc = User32.BeginPaint(m.HWnd, ref ps);
                using (var controlGraphics = Graphics.FromHdc(controlHdc))
                {
                    controlGraphics.DrawImage(bmp, 0, 0);
                }
                User32.EndPaint(m.HWnd, ref ps);
            }
        }

        /// <summary>
        /// Bitmapオブジェクトにコントロール描画を行う。
        /// </summary>
        private void drawBitmap(Bitmap bmp, Graphics bmpGraphics)
        {
            this.drawLine(bmpGraphics);
        }

        /// <summary>
        /// 線を描画する。
        /// </summary>
        private void drawLine(Graphics g)
        {
            if (BorderStyle != BorderStyle.None)
            {
                return;
            }

            if (linePosition == LinePosition.None)
            {
                return;
            }

            var pen = new Pen(LineColor, LineWidth);
            pen.DashStyle = LineDashStyle;

            if (LinePosition == LinePosition.Overline)
            {
                g.DrawLine(pen, new Point(0, 0), new Point(Width, 0));
            }
            if (LinePosition == LinePosition.Underline)
            {
                g.DrawLine(pen, new Point(0, Height - 1), new Point(Width, Height - 1));
            }
        }
    }
}
