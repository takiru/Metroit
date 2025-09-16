using System;
using System.Drawing;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// フォーカス用のオーバーレイを提供します。
    /// </summary>
    internal class FocusOverlay : IDisposable
    {
        private readonly Control _control;
        private readonly int OverlayWidth;
        private readonly Color OverlayColor;
        private readonly RoundedCornerRadius CornerRadius;
        private readonly Form ParentForm;

        private FocusOverlayWindow _focusOverlayWindow = null;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="control">オーバーレイを利用するコントロール。</param>
        /// <param name="overlayWidth">オーバーレイの幅。</param>
        /// <param name="overlayColor">オーバーレイの色。</param>
        /// <param name="cornerRadius">角丸設定。</param>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> が <see langword="null"/> です。</exception>
        public FocusOverlay(Control control, int overlayWidth, Color overlayColor, RoundedCornerRadius cornerRadius)
        {
            _control = control ?? throw new ArgumentNullException(nameof(control));
            ParentForm = control.FindForm();
            OverlayWidth = overlayWidth;
            OverlayColor = overlayColor;
            CornerRadius = cornerRadius;
        }

        /// <summary>
        /// フォーカスオーバーレイの表示を更新します。
        /// </summary>
        public void Show()
        {
            if (_control.Focused && _control.Visible)
            {
                if (ParentForm == null)
                {
                    throw new ArgumentException(ExceptionResources.GetString("ControlIsNotOnForm"));
                }

                var (clippedOverlayRect, contentRect) = CalculateFocusRectangles();

                // オーバーレイが有効な場合は描画位置やサイズを更新する
                if (_focusOverlayWindow != null && !_focusOverlayWindow.IsDisposed)
                {
                    _focusOverlayWindow.Redraw(clippedOverlayRect, contentRect);
                    return;
                }

                ShowOverlay(clippedOverlayRect, contentRect);
            }
            else
            {
                HideOverlay();
            }
        }

        /// <summary>
        /// フォーカスオーバーレイを表示する。
        /// </summary>
        /// <param name="clippedOverlayRect">クリッピングされたオーバーレイの表示領域。</param>
        /// <param name="contentRect">コントロールの表示領域。</param>
        private void ShowOverlay(Rectangle clippedOverlayRect, Rectangle contentRect)
        {
            ParentForm.Paint += (s, e) => Show();
            ParentForm.LocationChanged += (s, e) => Show();
            ParentForm.SizeChanged += (s, e) => Show();

            _focusOverlayWindow = new FocusOverlayWindow(OverlayWidth, OverlayColor, CornerRadius);
            _focusOverlayWindow.Show(clippedOverlayRect, contentRect);
        }

        /// <summary>
        /// フォーカスオーバーレイを非表示にする。
        /// </summary>
        private void HideOverlay()
        {
            if (ParentForm != null)
            {
                ParentForm.Paint -= (s, e) => Show();
                ParentForm.LocationChanged -= (s, e) => Show();
                ParentForm.SizeChanged -= (s, e) => Show();
            }

            if (_focusOverlayWindow != null && !_focusOverlayWindow.IsDisposed)
            {
                _focusOverlayWindow.Hide();
                _focusOverlayWindow.Close();
                _focusOverlayWindow.Dispose();
                _focusOverlayWindow = null;
            }
        }

        /// <summary>
        /// フォーカス枠とコンテンツ領域の矩形を計算する。
        /// </summary>
        /// <returns>フォーカス矩形とコンテンツ矩形のタプル。</returns>
        private (Rectangle clippedOverlayRect, Rectangle contentRect) CalculateFocusRectangles()
        {
            // コントロールの表示領域
            Rectangle contentRect = _control.RectangleToScreen(
                new Rectangle(0, 0, _control.Width, _control.Height));

            // オーバーレイの表示領域
            Rectangle overlayRect = new Rectangle(
                contentRect.X - OverlayWidth,
                contentRect.Y - OverlayWidth,
                contentRect.Width + (OverlayWidth * 2),
                contentRect.Height + (OverlayWidth * 2)
            );

            // フォームの描画領域から、オーバーレイの表示領域をクリップする
            Rectangle formRect = ParentForm.RectangleToScreen(ParentForm.ClientRectangle);
            Rectangle clippedOverlayRect = Rectangle.Intersect(overlayRect, formRect);

            return (clippedOverlayRect, contentRect);
        }

        /// <summary>
        /// オーバーレイを破棄します。
        /// </summary>
        public void Dispose()
        {
            HideOverlay();
        }
    }
}
