using System.Drawing;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 要素の色の組み合わせを提供します。
    /// </summary>
    public class ElementColorCombination
    {
        /// <summary>
        /// ボーダー色を取得します。
        /// </summary>
        public Color BorderColor { get; }

        /// <summary>
        /// 背景色を取得します。
        /// </summary>
        public Color BackColor { get; }

        /// <summary>
        /// 前景色を取得します。
        /// </summary>
        public Color ForeColor { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="borderColor">ボーダー色。</param>
        /// <param name="backColor">背景色。</param>
        /// <param name="foreColor">前景色。</param>
        public ElementColorCombination(Color borderColor, Color backColor, Color foreColor)
        {
            BorderColor = borderColor;
            BackColor = backColor;
            ForeColor = foreColor;
        }
    }
}
