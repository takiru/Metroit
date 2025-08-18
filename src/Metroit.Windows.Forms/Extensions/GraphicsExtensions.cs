using System.Drawing;
using System.Drawing.Drawing2D;

namespace Metroit.Windows.Forms.Extensions
{
    /// <summary>
    /// 角丸四角形の描画用拡張メソッドを提供します。
    /// </summary>
    public static class GraphicsExtensions
    {
        /// <summary>
        /// 角丸四角形を描画します。
        /// </summary>
        /// <param name="g">グラフィック。</param>
        /// <param name="brush">ブラシ。</param>
        /// <param name="rect">描画領域。</param>
        /// <param name="radius">角丸半径。</param>
        public static void FillRoundedRectangle(this Graphics g, Brush brush, RectangleF rect, float radius)
        {
            if (radius <= 0)
            {
                g.FillRectangle(brush, rect);
                return;
            }

            using (var path = CreateRoundedRectanglePath(rect, radius))
            {
                g.FillPath(brush, path);
            }
        }

        /// <summary>
        /// 角丸四角形の境界を描画します。
        /// </summary>
        /// <param name="g">グラフィック。</param>
        /// <param name="pen">ペン。</param>
        /// <param name="rect">描画領域。</param>
        /// <param name="radius">角丸半径。</param>
        public static void DrawRoundedRectangle(this Graphics g, Pen pen, RectangleF rect, float radius)
        {
            if (radius <= 0)
            {
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                return;
            }

            using (var path = CreateRoundedRectanglePath(rect, radius))
            {
                g.DrawPath(pen, path);
            }
        }

        /// <summary>
        /// 角丸四角形のパスを作成します。
        /// </summary>
        /// <param name="rect">描画領域。</param>
        /// <param name="radius">角丸半径。</param>
        /// <returns></returns>
        private static GraphicsPath CreateRoundedRectanglePath(RectangleF rect, float radius)
        {
            var path = new GraphicsPath();
            var diameter = radius * 2;

            var arcRect = new RectangleF(rect.Location, new SizeF(diameter, diameter));

            // 左上の角
            path.AddArc(arcRect, 180, 90);

            // 右上の角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // 右下の角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            // 左下の角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}
