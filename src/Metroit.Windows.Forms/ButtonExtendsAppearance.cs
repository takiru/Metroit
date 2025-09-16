using System.ComponentModel;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// <see cref="MetRoundedButton"/> の外観を指定するプロパティを用意します。
    /// </summary>
    public class ButtonExtendsAppearance : ExtendsAppearanceBase
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="owner"><see cref="ButtonBase"/>。</param>
        internal ButtonExtendsAppearance(ButtonBase owner) : base(owner) { }

        /// <summary>
        /// ボタンを囲む境界線のサイズをピクセル単位で指定する値を取得または設定します。
        /// </summary>
        [DefaultValue(0)]
        public override int BorderSize { get => base.BorderSize; set => base.BorderSize = value; }
    }
}
