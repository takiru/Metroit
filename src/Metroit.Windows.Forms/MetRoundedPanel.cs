using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 角丸のパネルコントロールを提供します。
    /// </summary>
    public class MetRoundedPanel : MetPanel
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MetRoundedPanel() : base()
        {
            // 最高品質の描画設定
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            // 透明背景を有効にする
            base.BackColor = Color.Transparent;

            // 基底クラスのBorderStyleを強制的にNoneに設定
            base.BorderStyle = BorderStyle.None;
        }

        private int _radius = 10;

        /// <summary>
        /// 角丸の半径を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(10)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetRoundedPanelRadius")]
        public int Radius
        {
            get => _radius;
            set
            {
                if (_radius != value && value >= 0)
                {
                    _radius = value;
                    Invalidate();
                }
            }
        }

        private Color _borderColor = Color.Black;

        /// <summary>
        /// 枠線の色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Black")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetRoundedPanelBorderColor")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    Invalidate();
                }
            }
        }

        private int _borderWidth = 1;

        /// <summary>
        /// 枠線の幅を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(1)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetRoundedPanelBorderWidth")]
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                if (_borderWidth != value && value >= 0)
                {
                    _borderWidth = value;
                    Invalidate();
                }
            }
        }

        private Color _fillColor = Color.White;

        /// <summary>
        /// 塗りつぶし色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "White")]
        [MetCategory("MetAppearance")]
        [MetDescription("MetRoundedPanelFillColor")]
        public Color FillColor
        {
            get => _fillColor;
            set
            {
                if (_fillColor != value)
                {
                    _fillColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// BackColorプロパティを隠します（このコントロールでは使用しません）
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("BackColorは使用できません。FillColorプロパティを使用してください。", true)]
#pragma warning disable CS0809
        public override Color BackColor
#pragma warning restore CS0809
        {
            get => base.BackColor;
            set { }
        }

        /// <summary>
        /// BorderStyleプロパティを隠します（このコントロールでは使用しません）
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("BorderStyleは使用できません。BorderColorとBorderWidthプロパティを使用してください。", true)]
        public new BorderStyle BorderStyle
        {
            get => BorderStyle.None;
            set { }
        }

        /// <summary>
        /// 透明な背景を正確に描画します
        /// </summary>
        private void DrawTransparentBackground(Graphics g, Region clipRegion)
        {
            if (Parent == null) return;

            // クリッピング領域を設定
            Region oldClip = g.Clip;
            try
            {
                g.Clip = clipRegion;

                // 親の背景を描画するために、座標を親の座標系に変換
                g.TranslateTransform(-Location.X, -Location.Y);

                // 親コントロールの背景描画メソッドを呼び出し
                InvokePaintBackground(Parent, new PaintEventArgs(g, Bounds));

                // 親コントロール上の他の子コントロールも描画（この Panel より前面にあるもの以外）
                InvokeParentPaint(Parent, g);
            }
            finally
            {
                g.TranslateTransform(Location.X, Location.Y);
                g.Clip = oldClip;
            }
        }

        /// <summary>
        /// 親コントロールの描画を呼び出します
        /// </summary>
        private void InvokeParentPaint(Control parent, Graphics g)
        {
            try
            {
                // 親の Paint イベントを発生させる
                var paintEventArgs = new PaintEventArgs(g, parent.ClientRectangle);
                typeof(Control).GetMethod("OnPaint", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.Invoke(parent, new object[] { paintEventArgs });
            }
            catch
            {
                // エラーが発生した場合は親の背景色で塗りつぶし
                using (var brush = new SolidBrush(parent.BackColor))
                {
                    g.FillRectangle(brush, parent.ClientRectangle);
                }
            }
        }

        /// <summary>
        /// 完璧な角丸パスを作成します
        /// </summary>
        private GraphicsPath CreateRoundedPath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            // 半径の境界チェック
            float maxRadius = Math.Min(rect.Width, rect.Height) / 2f;
            radius = Math.Min(radius, maxRadius);

            float diameter = radius * 2f;

            // 各角の円弧を高精度で配置
            // 左上角
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180f, 90f);

            // 上辺
            if (rect.Width > diameter)
            {
                path.AddLine(rect.X + radius, rect.Y, rect.X + rect.Width - radius, rect.Y);
            }

            // 右上角
            path.AddArc(rect.X + rect.Width - diameter, rect.Y, diameter, diameter, 270f, 90f);

            // 右辺
            if (rect.Height > diameter)
            {
                path.AddLine(rect.X + rect.Width, rect.Y + radius, rect.X + rect.Width, rect.Y + rect.Height - radius);
            }

            // 右下角
            path.AddArc(rect.X + rect.Width - diameter, rect.Y + rect.Height - diameter, diameter, diameter, 0f, 90f);

            // 下辺
            if (rect.Width > diameter)
            {
                path.AddLine(rect.X + rect.Width - radius, rect.Y + rect.Height, rect.X + radius, rect.Y + rect.Height);
            }

            // 左下角
            path.AddArc(rect.X, rect.Y + rect.Height - diameter, diameter, diameter, 90f, 90f);

            // 左辺
            if (rect.Height > diameter)
            {
                path.AddLine(rect.X, rect.Y + rect.Height - radius, rect.X, rect.Y + radius);
            }

            path.CloseFigure();

            // パスを最適化して滑らかさを向上
            path.Flatten(new Matrix(), 0.1f);

            return path;
        }

        /// <summary>
        /// 高品質な角丸図形を描画します
        /// </summary>
        private void DrawRoundedShape(Graphics g)
        {
            if (Width <= 0 || Height <= 0) return;

            // 描画品質を最高に設定
            var oldSmoothing = g.SmoothingMode;
            var oldInterpolation = g.InterpolationMode;
            var oldCompositing = g.CompositingQuality;
            var oldPixelOffset = g.PixelOffsetMode;

            try
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // 外側の境界（Width-1, Height-1まで）
                RectangleF outerRect = new RectangleF(0, 0, Width - 1, Height - 1);

                using (GraphicsPath outerPath = CreateRoundedPath(outerRect, _radius))
                {
                    // 全体の領域から外側パスを除いた部分を透明にするためのクリッピング領域を作成
                    using (Region transparentRegion = new Region(new Rectangle(0, 0, Width, Height)))
                    {
                        transparentRegion.Exclude(outerPath);

                        // 透明部分の背景を描画
                        if (transparentRegion != null && !transparentRegion.IsEmpty(g))
                        {
                            DrawTransparentBackground(g, transparentRegion);
                        }
                    }

                    // 枠線がある場合
                    if (_borderWidth > 0)
                    {
                        // まず全体を枠線色で塗りつぶし
                        using (SolidBrush borderBrush = new SolidBrush(_borderColor))
                        {
                            g.FillPath(borderBrush, outerPath);
                        }

                        // 内側の領域を計算
                        float innerOffset = _borderWidth;
                        RectangleF innerRect = new RectangleF(
                            innerOffset,
                            innerOffset,
                            Math.Max(0, Width - 1 - innerOffset * 2),
                            Math.Max(0, Height - 1 - innerOffset * 2)
                        );

                        if (innerRect.Width > 0 && innerRect.Height > 0)
                        {
                            // 内側の角丸半径を計算（外側半径から枠線分を引く）
                            float innerRadius = Math.Max(0, _radius - innerOffset);

                            using (GraphicsPath innerPath = CreateRoundedPath(innerRect, innerRadius))
                            {
                                // 塗りつぶし（内側）
                                using (SolidBrush fillBrush = new SolidBrush(_fillColor))
                                {
                                    g.FillPath(fillBrush, innerPath);
                                }

                                // 内側パスのエッジをさらに滑らかにするための補正描画
                                using (Pen innerSmoothPen = new Pen(Color.FromArgb(96, _fillColor), 0.8f))
                                {
                                    innerSmoothPen.Alignment = PenAlignment.Inset;
                                    g.DrawPath(innerSmoothPen, innerPath);
                                }
                            }
                        }

                        // 外側の境界線を滑らかにするための補正描画
                        using (Pen outerSmoothPen = new Pen(Color.FromArgb(96, _borderColor), 0.8f))
                        {
                            outerSmoothPen.Alignment = PenAlignment.Inset;
                            g.DrawPath(outerSmoothPen, outerPath);
                        }
                    }
                    else
                    {
                        // 枠線なし、塗りつぶしのみ
                        using (SolidBrush fillBrush = new SolidBrush(_fillColor))
                        {
                            g.FillPath(fillBrush, outerPath);
                        }

                        // アンチエイリアシング補正のための境界線
                        using (Pen smoothPen = new Pen(Color.FromArgb(64, _fillColor), 0.5f))
                        {
                            smoothPen.Alignment = PenAlignment.Inset;
                            g.DrawPath(smoothPen, outerPath);
                        }
                    }
                }
            }
            finally
            {
                g.SmoothingMode = oldSmoothing;
                g.InterpolationMode = oldInterpolation;
                g.CompositingQuality = oldCompositing;
                g.PixelOffsetMode = oldPixelOffset;
            }
        }

        /// <summary>
        /// BorderStyleの変更を監視して即座にNoneに戻します
        /// </summary>
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            // BorderStyleが変更されていたら強制的にNoneに戻す
            if (base.BorderStyle != BorderStyle.None)
            {
                base.BorderStyle = BorderStyle.None;
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }

        /// <summary>
        /// BorderStyleの変更を監視します
        /// </summary>
        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            // BorderStyleが変更されていたら強制的にNoneに戻す
            if (base.BorderStyle != BorderStyle.None)
            {
                base.BorderStyle = BorderStyle.None;
            }

            base.OnInvalidated(e);
        }

        /// <summary>
        /// 描画前にBorderStyleをNoneに強制設定
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // BorderStyleが変更されていたら強制的にNoneに戻す
            if (base.BorderStyle != BorderStyle.None)
            {
                base.BorderStyle = BorderStyle.None;
            }

            // 背景描画はカスタム描画で行うため、基底クラスの背景描画はスキップ
            // base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// コントロールの描画を行います。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // BorderStyleが変更されていたら強制的にNoneに戻す（最終防御線）
            if (base.BorderStyle != BorderStyle.None)
            {
                base.BorderStyle = BorderStyle.None;
            }

            // 角丸図形を描画
            DrawRoundedShape(e.Graphics);

            // 子コントロールの描画
            base.OnPaint(e);
        }

        /// <summary>
        /// 親コントロールが変更されたときに再描画します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            Invalidate();
        }

        /// <summary>
        /// 親コントロールの背景色が変更されたときに再描画します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            Invalidate();
        }
    }
}