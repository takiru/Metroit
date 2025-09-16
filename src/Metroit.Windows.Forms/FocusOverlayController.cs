using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// フォーカスのオーバーレイ操作を提供します。
    /// </summary>
    internal class FocusOverlayController : IDisposable
    {
        private readonly Control Control;
        private readonly MethodInfo ParentInvokePaintBackground;
        private readonly MethodInfo ParentInvokePaint;
        private readonly Func<Color> GetCurrentFillColorDelegate;

        private FocusOverlay _focusOverlay;
        private ExtendsAppearanceBase _extendsAppearance;
        private RoundedCornerRadius _radius;

        public FocusOverlayController(Control control, Func<Color> getCurrentFillColor = null)
        {
            Control = control;
            Control.MouseEnter += (s, e) => { MouseState = MouseState.Hover; Control.Invalidate(); };
            Control.MouseLeave += (s, e) => { MouseState = MouseState.Normal; Control.Invalidate(); };
            Control.MouseDown += (s, e) => { MouseState = MouseState.Pressed; Control.Invalidate(); };
            Control.MouseUp += (s, e) => { MouseState = MouseState.Hover; Control.Invalidate(); };
            Control.Resize += (s, e) => { Control.Invalidate(); };
            Control.GotFocus += (s, e) => { Control.Invalidate(); };
            Control.LostFocus += (s, e) => { Control.Invalidate(); };


            ParentInvokePaintBackground = control.Parent
                .GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name == "OnPaintBackground" &&
                        x.GetParameters().Count() == 1 &&
                        x.GetParameters()[0].ParameterType == typeof(PaintEventArgs))
                .FirstOrDefault();
            ParentInvokePaint = control.Parent
                .GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name == "OnPaint" &&
                        x.GetParameters().Count() == 1 &&
                        x.GetParameters()[0].ParameterType == typeof(PaintEventArgs))
                .FirstOrDefault();

            GetCurrentFillColorDelegate = getCurrentFillColor;
        }
        /// <summary>
        /// 現在のマウス状態。
        /// </summary>
        public MouseState MouseState { get; private set; } = MouseState.Normal;

        /// <summary>
        /// オーバーレイを破棄します。
        /// </summary>
        public void Dispose()
        {
            _focusOverlay.Dispose();
        }

        /// <summary>
        /// 角丸図形を描画する。
        /// フォーカス時の枠線、境界線、背景色を描画する。
        /// </summary>
        /// <param name="g">グラフィックス。</param>
        /// <param name="extendsAppearance">描画に利用する表示設定。</param>
        /// <param name="radius">角丸の半径。</param>
        public void DrawRoundedShape(Graphics g, ExtendsAppearanceBase extendsAppearance,
            RoundedCornerRadius radius)
        {
            if (Control.Width <= 0 || Control.Height <= 0)
            {
                return;
            }

            _extendsAppearance = extendsAppearance;
            _radius = radius;

            if (_focusOverlay == null)
            {
                _focusOverlay = new FocusOverlay(Control, _extendsAppearance.FocusOverlayWidth,
                    _extendsAppearance.FocusOverlayColor, _radius);
            }

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

                RectangleF contentRect = new RectangleF(0, 0, Control.Width, Control.Height);

                // 描画領域を親の背景で塗りつぶす
                DrawTransparentBackground(g, new Region(contentRect));

                // コンテンツ領域
                using (GraphicsPath contentPath = CreateRoundedPath(contentRect, _radius.TopLeft,
                    _radius.TopRight, _radius.BottomLeft, _radius.BottomRight))
                {
                    // 境界線を描画
                    if (_extendsAppearance.BorderSize > 0)
                    {
                        using (SolidBrush borderBrush = new SolidBrush(GetCurrentBorderColor()))
                        {
                            g.FillPath(borderBrush, contentPath);
                        }
                    }

                    // 背景を描画
                    float backOffset = _extendsAppearance.BorderSize;
                    RectangleF backRect = new RectangleF(
                        backOffset,
                        backOffset,
                        Math.Max(0, Control.Width - backOffset * 2),
                        Math.Max(0, Control.Height - backOffset * 2)
                    );

                    if (backRect.Width > 0 && backRect.Height > 0)
                    {
                        using (GraphicsPath backPath = CreateRoundedPath(backRect,
                            _radius.TopLeft, _radius.TopRight, _radius.BottomLeft, _radius.BottomRight))
                        {
                            Color currentFillColor = GetCurrentFillColorDelegate == null ?
                                GetCurrentFillColor() : GetCurrentFillColorDelegate.Invoke();
                            using (SolidBrush fillBrush = new SolidBrush(currentFillColor))
                            {
                                g.FillPath(fillBrush, backPath);
                            }
                        }
                    }
                }

                // フォーカス枠をオーバーレイウィンドウで表示
                _focusOverlay.Show();
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
        /// 親背景を正確に描画して、透明な背景をクリッピングによって描画する。
        /// </summary>
        /// <param name="g">グラフィックス。</param>
        /// <param name="clipRegion">クリッピング領域。</param>
        private void DrawTransparentBackground(Graphics g, Region clipRegion)
        {
            Region oldClip = g.Clip;
            try
            {
                g.Clip = clipRegion;

                // 親の背景を描画するために、座標を親の座標系に変換
                g.TranslateTransform(-Control.Location.X, -Control.Location.Y);

                // 親コントロールの背景描画を行う
                var e = new PaintEventArgs(g, Control.Bounds);
                ParentInvokePaintBackground?.Invoke(Control.Parent, new[] { e });
                ParentInvokePaint?.Invoke(Control.Parent, new[] { e });
            }
            finally
            {
                g.TranslateTransform(Control.Location.X, Control.Location.Y);
                g.Clip = oldClip;
            }
        }

        /// <summary>
        /// 角丸パスを作成する。
        /// </summary>
        /// <param name="rect">描画領域。</param>
        /// <param name="topLeftRadius">左上角の角丸半径。</param>
        /// <param name="topRightRadius">右上角の角丸半径。</param>
        /// <param name="bottomLeftRadius">左下角の角丸半径。</param>
        /// <param name="bottomRightRadius">右下角の角丸半径。</param>
        /// <returns>角丸パス。</returns>
        private GraphicsPath CreateRoundedPath(RectangleF rect, float topLeftRadius, float topRightRadius,
            float bottomLeftRadius, float bottomRightRadius)
        {
            GraphicsPath path = new GraphicsPath();

            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return path;
            }

            if (topLeftRadius <= 0 && topRightRadius <= 0 && bottomLeftRadius <= 0 && bottomRightRadius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            // 半径の境界チェック
            float maxRadius = Math.Min(rect.Width, rect.Height) / 2f;
            var drawTopLeftRadius = Math.Min(topLeftRadius, maxRadius);
            var drawTopRightRadius = Math.Min(topRightRadius, maxRadius);
            var drawBottomLeftRadius = Math.Min(bottomLeftRadius, maxRadius);
            var drawBottomRightRadius = Math.Min(bottomRightRadius, maxRadius);

            float x = rect.X;
            float y = rect.Y;
            float width = rect.Width;
            float height = rect.Height;

            // 角丸の直径
            float topLeftDiameter = drawTopLeftRadius * 2f;
            float topRightDiameter = drawTopRightRadius * 2f;
            float bottomLeftDiameter = drawBottomLeftRadius * 2f;
            float bottomRightDiameter = drawBottomRightRadius * 2f;

            // 左上角から開始
            if (drawTopLeftRadius > 0)
            {
                // 左上の円弧
                path.AddArc(x, y,
                    topLeftDiameter, topLeftDiameter, 180f, 90f);
                // 上辺へ移動
                path.AddLine(x + drawTopLeftRadius, y,
                    x + width - drawTopRightRadius, y);
            }
            else
            {
                // 角丸なしの場合は左上角から上辺
                path.AddLine(x, y,
                    x + width - drawTopRightRadius, y);
            }

            // 右上角
            if (drawTopRightRadius > 0)
            {
                // 右上の円弧
                path.AddArc(x + width - topRightDiameter, y, topRightDiameter,
                    topRightDiameter, 270f, 90f);
                // 右辺へ移動
                path.AddLine(x + width, y + drawTopRightRadius,
                    x + width, y + height - drawBottomRightRadius);
            }
            else
            {
                // 角丸なしの場合は右上角から右辺
                path.AddLine(x + width, y, x + width,
                    y + height - drawBottomRightRadius);
            }

            // 右下角
            if (drawBottomRightRadius > 0)
            {
                // 右下の円弧
                path.AddArc(x + width - bottomRightDiameter, y + height - bottomRightDiameter,
                    bottomRightDiameter, bottomRightDiameter, 0f, 90f);
                // 下辺へ移動
                path.AddLine(x + width - drawBottomRightRadius, y + height,
                    x + drawBottomLeftRadius, y + height);
            }
            else
            {
                // 角丸なしの場合は右下角から下辺
                path.AddLine(x + width, y + height,
                    x + drawBottomLeftRadius, y + height);
            }

            // 左下角
            if (drawBottomLeftRadius > 0)
            {
                // 左下の円弧
                path.AddArc(x, y + height - bottomLeftDiameter,
                    bottomLeftDiameter, bottomLeftDiameter, 90f, 90f);
                // 左辺へ移動（開始点まで戻る）
                path.AddLine(x, y + height - drawBottomLeftRadius,
                    x, y + drawTopLeftRadius);
            }
            else
            {
                // 角丸なしの場合は左下角から左辺
                path.AddLine(x, y + height,
                    x, y + drawTopLeftRadius);
            }

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// 現在のコントロールとマウス状態に応じた塗りつぶし色を取得する。
        /// </summary>
        /// <returns>現在の塗りつぶし色。</returns>
        private Color GetCurrentBorderColor()
        {
            if (!Control.Enabled)
            {
                return ControlPaint.LightLight(_extendsAppearance.BorderColor);
            }

            return _extendsAppearance.BorderColor;
        }

        /// <summary>
        /// 現在のコントロールとマウス状態に応じた塗りつぶし色を取得する。
        /// </summary>
        /// <returns>現在の塗りつぶし色。</returns>
        private Color GetCurrentFillColor()
        {
            if (!Control.Enabled)
            {
                return ControlPaint.LightLight(Control.BackColor);
            }

            switch (MouseState)
            {
                case MouseState.Hover:
                    return _extendsAppearance.MouseOverBackColor;

                case MouseState.Pressed:
                    return _extendsAppearance.MouseDownBackColor;

                default:
                    return Control.BackColor;
            }
        }
    }
}
