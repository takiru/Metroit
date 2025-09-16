using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// フォーカス用のオーバーレイウィンドウを提供します。
    /// </summary>
    internal class FocusOverlayWindow : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst,
            ref Size psize, IntPtr hdcSrc, ref Point pptSrc, uint crKey, ref BLENDFUNCTION pblend,
            uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int cx, int cy);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hgdiobj);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha,
            uint dwFlags);

        private const int WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int WS_EX_NOACTIVATE = 0x8000000;
        private const int WS_EX_TOOLWINDOW = 0x80;
        private const int WS_EX_TOPMOST = 0x8;
        private const uint ULW_ALPHA = 2;
        private const byte AC_SRC_OVER = 0;
        private const byte AC_SRC_ALPHA = 1;

        private const int WM_ACTIVATE = 0x0006;
        private const int WM_ACTIVATEAPP = 0x001C;
        private const int WM_NCACTIVATE = 0x0086;
        private const int WM_SETFOCUS = 0x0007;
        private const int WM_KILLFOCUS = 0x0008;
        private const int WM_MOUSEACTIVATE = 0x0021;
        private const int MA_NOACTIVATE = 3;

        [StructLayout(LayoutKind.Sequential)]
        private struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        private readonly int OverlayWidth;
        private readonly Color OverlayColor;
        private readonly RoundedCornerRadius CornerRadius;

        private Rectangle _overlayRect;
        private Rectangle _contentRect;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="overlayWidth">オーバーレイの幅。</param>
        /// <param name="overlayColor">オーバーレイの色。</param>
        /// <param name="cornerRadius">角丸設定。</param>
        public FocusOverlayWindow(int overlayWidth, Color overlayColor, RoundedCornerRadius cornerRadius)
        {
            OverlayWidth = overlayWidth;
            OverlayColor = overlayColor;
            CornerRadius = cornerRadius;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            ControlBox = false;
            MinimizeBox = false;
            MaximizeBox = false;
            KeyPreview = false;
            Bounds = _overlayRect;
        }

        /// <summary>
        /// CreateParamsをオーバーライドしてレイヤードウィンドウにする。
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST;
                return cp;
            }
        }

        /// <summary>
        /// 完全に非アクティブにする。
        /// </summary>
        protected override bool ShowWithoutActivation => true;

        /// <summary>
        /// ウィンドウメッセージをオーバーライドしてアクティブ化を完全に防ぐ。
        /// </summary>
        /// <param name="m">ウィンドウメッセージ</param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_ACTIVATE:
                case WM_ACTIVATEAPP:
                case WM_NCACTIVATE:
                case WM_SETFOCUS:
                    // アクティブ化関連のメッセージを無視
                    return;

                case WM_MOUSEACTIVATE:
                    // マウスクリックでもアクティブにしない
                    m.Result = (IntPtr)MA_NOACTIVATE;
                    return;

                case WM_KILLFOCUS:
                    // フォーカス喪失メッセージも無視
                    return;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// キーイベントを無効化する。
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return false;
        }

        /// <summary>
        /// 表示位置とサイズを決定してオーバーレイを表示します。
        /// </summary>
        /// <param name="clippedOverlayRect">オーバーレイの表示領域。</param>
        /// <param name="contentRect">コントロールの表示領域。</param>
        public void Show(Rectangle clippedOverlayRect, Rectangle contentRect)
        {
            Redraw(clippedOverlayRect, contentRect);
            Show();
        }

        /// <summary>
        /// 表示位置とサイズを更新し、内容を再描画します。
        /// </summary>
        /// <param name="clippedOverlayRect">オーバーレイの表示領域。</param>
        /// <param name="contentRect">コントロールの表示領域。</param>
        public void Redraw(Rectangle clippedOverlayRect, Rectangle contentRect)
        {
            _contentRect = contentRect;
            UpdateOverlayRectangle(contentRect);

            // 現在表示中でサイズや位置が変わった場合のみ更新
            if (Bounds != clippedOverlayRect)
            {
                Bounds = clippedOverlayRect;
                RedrawOverlay();
            }
        }

        /// <summary>
        /// コントロールの表示領域からオーバーレイの表示領域を計算します。
        /// </summary>
        /// <param name="contentRect">コントロールの表示領域。</param>
        private void UpdateOverlayRectangle(Rectangle contentRect)
        {
            _overlayRect = new Rectangle(
                contentRect.X - OverlayWidth,
                contentRect.Y - OverlayWidth,
                contentRect.Width + (OverlayWidth * 2),
                contentRect.Height + (OverlayWidth * 2)
            );
        }

        /// <summary>
        /// レイヤードウィンドウの内容を更新
        /// </summary>
        private void RedrawOverlay()
        {
            // 高解像度ビットマップを作成（アンチエイリアスのため2倍解像度）
            int scaleFactor = 2;
            int scaledWidth = Width * scaleFactor;
            int scaledHeight = Height * scaleFactor;

            using (Bitmap bitmap = new Bitmap(scaledWidth, scaledHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    // 最高品質の描画設定
                    g.Clear(Color.Transparent);
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                    // スケール変換を適用
                    g.ScaleTransform(scaleFactor, scaleFactor);

                    // フォーカス枠を描画
                    DrawOverlay(g);
                }

                // 元のサイズにダウンスケール
                using (Bitmap finalBitmap = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    using (Graphics finalG = Graphics.FromImage(finalBitmap))
                    {
                        finalG.SmoothingMode = SmoothingMode.HighQuality;
                        finalG.CompositingQuality = CompositingQuality.HighQuality;
                        finalG.PixelOffsetMode = PixelOffsetMode.HighQuality;

                        finalG.DrawImage(bitmap, new Rectangle(0, 0, Width, Height),
                                       new Rectangle(0, 0, scaledWidth, scaledHeight), GraphicsUnit.Pixel);
                    }

                    // レイヤードウィンドウを更新
                    IntPtr screenDc = GetDC(IntPtr.Zero);
                    IntPtr memDc = CreateCompatibleDC(screenDc);
                    IntPtr hBitmap = finalBitmap.GetHbitmap(Color.FromArgb(0));
                    IntPtr hOldBitmap = SelectObject(memDc, hBitmap);

                    Point ptSrc = new Point(0, 0);
                    Point ptDst = new Point(Left, Top);
                    Size size = new Size(Width, Height);
                    BLENDFUNCTION blend = new BLENDFUNCTION
                    {
                        BlendOp = AC_SRC_OVER,
                        BlendFlags = 0,
                        SourceConstantAlpha = 255,
                        AlphaFormat = AC_SRC_ALPHA
                    };

                    UpdateLayeredWindow(Handle, screenDc, ref ptDst, ref size, memDc, ref ptSrc, 0, ref blend, ULW_ALPHA);

                    // リソースを解放
                    SelectObject(memDc, hOldBitmap);
                    DeleteObject(hBitmap);
                    DeleteDC(memDc);
                    ReleaseDC(IntPtr.Zero, screenDc);
                }
            }
        }

        /// <summary>
        /// フォーカス枠を描画する。
        /// 角丸部分のみアンチエイリアシング、直線部分はシャープとする。
        /// </summary>
        /// <param name="g">グラフィックス。</param>
        private void DrawOverlay(Graphics g)
        {
            // LayeredFocusWindow内での相対座標でフォーカス枠のフルサイズを描画
            // ウィンドウがクリッピングされていても、フォーカス枠は完全な形状で描画される

            // LayeredFocusWindow内での相対座標を計算
            float focusX = _overlayRect.X - Left;
            float focusY = _overlayRect.Y - Top;
            float focusWidth = _overlayRect.Width;
            float focusHeight = _overlayRect.Height;

            // ボタン領域の相対座標を計算
            float buttonX = _contentRect.X - Left;
            float buttonY = _contentRect.Y - Top;
            float buttonWidth = _contentRect.Width;
            float buttonHeight = _contentRect.Height;

            // 整数座標でシャープな描画
            RectangleF focusRectF = new RectangleF(focusX, focusY, focusWidth, focusHeight);
            RectangleF buttonRectF = new RectangleF(buttonX, buttonY, buttonWidth, buttonHeight);

            using (SolidBrush focusBrush = new SolidBrush(OverlayColor))
            {
                // 角丸がある場合のみパスを使用（アンチエイリアシング有効）
                if (CornerRadius.TopLeft > 0 ||
                    CornerRadius.TopRight > 0 ||
                    CornerRadius.BottomRight > 0 ||
                    CornerRadius.BottomLeft > 0)
                {
                    // 角丸部分はアンチエイリアシング有効で描画
                    var oldSmoothing = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    using (GraphicsPath focusPath = CreateRoundedPath(focusRectF, CornerRadius.TopLeft,
                        CornerRadius.TopRight, CornerRadius.BottomLeft, CornerRadius.BottomRight))
                    {
                        using (GraphicsPath buttonPath = CreateRoundedPath(buttonRectF, CornerRadius.TopLeft,
                            CornerRadius.TopRight, CornerRadius.BottomLeft, CornerRadius.BottomRight))
                        {
                            using (Region focusRegion = new Region(focusPath))
                            {
                                focusRegion.Exclude(buttonPath);
                                g.FillRegion(focusBrush, focusRegion);
                            }
                        }
                    }

                    g.SmoothingMode = oldSmoothing;
                }
                else
                {
                    // 角丸なしの場合は直線のみ（アンチエイリアシング無効）
                    g.SmoothingMode = SmoothingMode.None;

                    // 上下左右の直線を個別に描画
                    int fx = (int)focusX;
                    int fy = (int)focusY;
                    int fw = (int)focusWidth;
                    int fh = (int)focusHeight;
                    int bx = (int)buttonX;
                    int by = (int)buttonY;
                    int bw = (int)buttonWidth;
                    int bh = (int)buttonHeight;

                    // 上辺
                    if (by > fy)
                    {
                        g.FillRectangle(focusBrush, fx, fy, fw, by - fy);
                    }

                    // 下辺
                    if (by + bh < fy + fh)
                    {
                        g.FillRectangle(focusBrush, fx, by + bh, fw, (fy + fh) - (by + bh));
                    }

                    // 左辺
                    if (bx > fx)
                    {
                        g.FillRectangle(focusBrush, fx, by, bx - fx, bh);
                    }

                    // 右辺
                    if (bx + bw < fx + fw)
                    {
                        g.FillRectangle(focusBrush, bx + bw, by, (fx + fw) - (bx + bw), bh);
                    }
                }
            }
        }

        /// <summary>
        /// 角丸パスを作成する。
        /// </summary>
        private GraphicsPath CreateRoundedPath(RectangleF rect, float topLeftRadius, float topRightRadius, float bottomLeftRadius, float bottomRightRadius)
        {
            GraphicsPath path = new GraphicsPath();

            if (rect.Width <= 0 || rect.Height <= 0)
                return path;

            if (topLeftRadius <= 0 &&
                topRightRadius <= 0 &&
                bottomLeftRadius <= 0 &&
                bottomRightRadius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            float maxRadius = Math.Min(rect.Width, rect.Height) / 2f;
            topLeftRadius = Math.Min(topLeftRadius, maxRadius);
            topRightRadius = Math.Min(topRightRadius, maxRadius);
            bottomLeftRadius = Math.Min(bottomLeftRadius, maxRadius);
            bottomRightRadius = Math.Min(bottomRightRadius, maxRadius);

            float x = rect.X;
            float y = rect.Y;
            float width = rect.Width;
            float height = rect.Height;

            // 高精度で角丸パスを構築
            // 左上角
            if (topLeftRadius > 0)
            {
                path.AddArc(x, y, topLeftRadius * 2f, topLeftRadius * 2f, 180f, 90f);
            }
            else
            {
                path.AddLine(x, y, x, y);
            }

            // 上辺
            path.AddLine(x + topLeftRadius, y, x + width - topRightRadius, y);

            // 右上角
            if (topRightRadius > 0)
            {
                path.AddArc(x + width - topRightRadius * 2f, y,
                    topRightRadius * 2f, topRightRadius * 2f, 270f, 90f);
            }

            // 右辺
            path.AddLine(x + width, y + topRightRadius, x + width,
                y + height - bottomRightRadius);

            // 右下角
            if (bottomRightRadius > 0)
            {
                path.AddArc(x + width - bottomRightRadius * 2f, y + height - bottomRightRadius * 2f,
                    bottomRightRadius * 2f, bottomRightRadius * 2f, 0f, 90f);
            }

            // 下辺
            path.AddLine(x + width - bottomRightRadius, y + height,
                x + bottomLeftRadius, y + height);

            // 左下角
            if (bottomLeftRadius > 0)
            {
                path.AddArc(x, y + height - bottomLeftRadius * 2f, bottomLeftRadius * 2f,
                    bottomLeftRadius * 2f, 90f, 90f);
            }

            // 左辺
            path.AddLine(x, y + height - bottomLeftRadius,
                x, y + topLeftRadius);

            path.CloseFigure();
            return path;
        }
    }
}
