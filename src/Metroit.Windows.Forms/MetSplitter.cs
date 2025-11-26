using Metroit.Win32.Api;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 拡張したSplitterコントロールを提供します。
    /// </summary>
    public class MetSplitter : Splitter
    {
        private static readonly int WM_NCPAINT = 0x85;

        private Color _borderColor = Color.Empty;

        /// <summary>
        /// ボーダー色を取得または設定します。<see cref="BorderStyle"/>が<see cref="BorderStyle.FixedSingle"/>の場合に使用されます。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Empty")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetSplitterBorderColor")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        private Color _knobColor = Color.Gray;

        /// <summary>
        /// ノブの色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Gray")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetSplitterKnobColor")]
        public Color KnobColor
        {
            get => _knobColor;
            set
            {
                _knobColor = value;
                Invalidate();
            }
        }

        private int _knobWidth = 20;

        /// <summary>
        /// ノブの幅を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(20)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetSplitterKnobWidth")]
        public int KnobWidth
        {
            get => _knobWidth;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "KnobWidthは0以上の値を指定してください。");
                }
                _knobWidth = value;
                Invalidate();
            }
        }

        private int _knobThickness = 1;

        /// <summary>
        /// ノブの線の太さを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(1)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetSplitterKnobThickness")]
        public int KnobThickness
        {
            get => _knobThickness;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "KnobThicknessは1以上の値を指定してください。");
                }
                _knobThickness = value;
                Invalidate();
            }
        }

        private HorizontalAlignment _knobAlign = HorizontalAlignment.Center;

        /// <summary>
        /// ノブの配置位置を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(HorizontalAlignment.Center)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetSplitterKnobAlign")]
        public HorizontalAlignment KnobAlign
        {
            get => _knobAlign;
            set
            {
                _knobAlign = value;
                Invalidate();
            }
        }

        /// <summary>
        /// パディングが変更になったら再描画を指示する。
        /// </summary>
        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            Invalidate();
        }

        /// <summary>
        /// BorderStyle = FixedSingle のときにボーダー色が指定されていたら、既定のボーダー描画を無効とする。
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WindowMessage.WM_NCPAINT)
            {
                if (IsCustomBorderDraw())
                {
                    return;
                }
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// ボーダーとノブをカスタム描画する。
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsCustomBorderDraw())
            {
                DrawCustomBorder();
            }

            DrawKnob();

            base.OnPaint(e);
        }

        /// <summary>
        /// ウィンドウサイズが狭くなったときに OnPaint() が走行しないため、明示的に再描画を指示する。
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        /// <summary>
        /// カスタムボーダーを描画するかどうかを判定する。
        /// </summary>
        /// <returns>カスタムボーダーを描画するときは true, それ以外は false を返却する。</returns>
        private bool IsCustomBorderDraw()
        {
            if (BorderStyle != BorderStyle.FixedSingle)
            {
                return false;
            }

            if (BorderColor == Color.Empty)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// BorderStyle = FixedSingle のときにボーダー色が指定されていたら、カスタムボーダーを描画する。
        /// </summary>
        private void DrawCustomBorder()
        {
            IntPtr hdc = User32.GetWindowDC(Handle);
            if (hdc == IntPtr.Zero)
            {
                return;
            }

            using (Graphics g = Graphics.FromHdc(hdc))
            using (SolidBrush brush = new SolidBrush(BackColor))
            using (Pen borderPen = new Pen(BorderColor, 1))
            {
                Rectangle rect = new Rectangle(0, 0, Width, Height);

                // 背景をすべて塗りつぶし
                g.FillRectangle(brush, rect);

                // ボーダーを描画
                g.DrawRectangle(borderPen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
            }
            User32.ReleaseDC(Handle, hdc);
        }

        /// <summary>
        /// ノブを描画する。
        /// </summary>
        private void DrawKnob()
        {
            IntPtr hdc = User32.GetWindowDC(Handle);
            if (hdc == IntPtr.Zero)
            {
                return;
            }

            // 線の設定
            int lineSpacing = 2;

            using (Graphics g = Graphics.FromHdc(hdc))
            using (Pen pen = new Pen(KnobColor, KnobThickness))
            {
                if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                {
                    // 垂直ノブ（左右Dock）
                    int centerX = Width / 2;
                    int centerY = CalculateVerticalKnobPosition();
                    DrawVerticalKnob(g, pen, centerX, centerY, KnobWidth, lineSpacing);
                }
                else
                {
                    // 水平ノブ（上下Dockまたはその他）
                    int centerX = CalculateHorizontalKnobPosition();
                    int centerY = Height / 2;
                    DrawHorizontalKnob(g, pen, centerX, centerY, KnobWidth, lineSpacing);
                }
            }
            User32.ReleaseDC(Handle, hdc);
        }

        /// <summary>
        /// 垂直ノブ（左右Dock）のY座標を計算する。
        /// </summary>
        /// <returns>Y座標</returns>
        private int CalculateVerticalKnobPosition()
        {
            switch (KnobAlign)
            {
                case HorizontalAlignment.Left:
                    // 上端から描画（Padding.Top + ノブ半分）
                    return Padding.Top + (KnobWidth / 2);
                case HorizontalAlignment.Right:
                    // 下端から描画（Height - Padding.Bottom - ノブ半分）
                    return Height - Padding.Bottom - (KnobWidth / 2);
                default:
                    // 中央
                    return Height / 2;
            }
        }

        /// <summary>
        /// 水平ノブ（上下Dockまたはその他）のX座標を計算する。
        /// </summary>
        /// <returns>X座標</returns>
        private int CalculateHorizontalKnobPosition()
        {
            switch (KnobAlign)
            {
                case HorizontalAlignment.Left:
                    // 左端から描画（Padding.Left + ノブ半分）
                    return Padding.Left + (KnobWidth / 2);
                case HorizontalAlignment.Right:
                    // 右端から描画（Width - Padding.Right - ノブ半分）
                    return Width - Padding.Right - (KnobWidth / 2);
                default:
                    // 中央
                    return Width / 2;
            }
        }

        /// <summary>
        /// 水平ノブを描画する。
        /// </summary>
        /// <param name="g">グラフィックオブジェクト。</param>
        /// <param name="pen">ペン。</param>
        /// <param name="centerX">描画水平位置。</param>
        /// <param name="centerY">描画垂直位置。</param>
        /// <param name="lineWidth">描画幅。</param>
        /// <param name="lineSpacing">描画間隔。</param>
        private void DrawHorizontalKnob(Graphics g, Pen pen, int centerX, int centerY,
                                        int lineWidth, int lineSpacing)
        {
            // 線を描画する水平座標
            int startX = centerX - (lineWidth / 2);
            int endX = centerX + (lineWidth / 2);

            // 3本の線の開始Y座標を、中央の線を基準に上下に算出
            int line1Y = centerY - lineSpacing - 1;
            int line2Y = centerY;
            int line3Y = centerY + lineSpacing + 1;

            // 3本の横線を描画
            g.DrawLine(pen, startX, line1Y, endX, line1Y);
            g.DrawLine(pen, startX, line2Y, endX, line2Y);
            g.DrawLine(pen, startX, line3Y, endX, line3Y);
        }

        /// <summary>
        /// 垂直ノブを描画する。
        /// </summary>
        /// <param name="g">グラフィックオブジェクト。</param>
        /// <param name="pen">ペン。</param>
        /// <param name="centerX">描画水平位置。</param>
        /// <param name="centerY">描画垂直位置。</param>
        /// <param name="lineHeight">描画高さ。</param>
        /// <param name="lineSpacing">描画間隔。</param>
        private void DrawVerticalKnob(Graphics g, Pen pen, int centerX, int centerY,
                                      int lineHeight, int lineSpacing)
        {
            // 線を描画する垂直座標
            int startY = centerY - (lineHeight / 2);
            int endY = centerY + (lineHeight / 2);

            // 3本の線の開始X座標を、中央の線を基準に左右に算出
            int line1X = centerX - lineSpacing - 1;
            int line2X = centerX;
            int line3X = centerX + lineSpacing + 1;

            // 3本の縦線を描画
            g.DrawLine(pen, line1X, startY, line1X, endY);
            g.DrawLine(pen, line2X, startY, line2X, endY);
            g.DrawLine(pen, line3X, startY, line3X, endY);
        }
    }
}