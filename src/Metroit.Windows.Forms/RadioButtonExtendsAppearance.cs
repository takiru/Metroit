using System.ComponentModel;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// <see cref="MetRoundedRadioButton"/> の外観を指定するプロパティを用意します。
    /// </summary>
    public class RadioButtonExtendsAppearance : ExtendsAppearanceBase
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="owner"><see cref="ButtonBase"/>。</param>
        internal RadioButtonExtendsAppearance(ButtonBase owner) : base(owner) { }

        /// <summary>
        /// ボタンを囲む境界線のサイズをピクセル単位で指定する値を取得または設定します。
        /// </summary>
        [DefaultValue(1)]
        public override int BorderSize { get => base.BorderSize; set => base.BorderSize = value; }
    }
}
