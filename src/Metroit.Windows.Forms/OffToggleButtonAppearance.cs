using System.ComponentModel;
using System.Drawing;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// <see cref="MetToggleSwitch"/> の外観を指定するプロパティを用意します。
    /// </summary>
    public class OffToggleButtonAppearance : ToggleButtonAppearanceBase
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="owner"><see cref="MetToggleSwitch"/>。</param>
        internal OffToggleButtonAppearance(MetToggleSwitch owner) : base(owner)
        {
            BackColor = Color.White;
            DisabledBackColor = Color.White;
            DisabledThumbColor = Color.FromArgb(223, 223, 223);
            BorderColor = Color.FromArgb(222, 226, 230);
            ChangeTextWithoutUpdateControlSize("OFF");
            ThumbColor = Color.FromArgb(191, 191, 191);
        }

        /// <summary>
        /// トグルスイッチの背景色を取得または設定します。
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }

        /// <summary>
        /// トグルスイッチが無効なときの背景色を取得または設定します。
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        public override Color DisabledBackColor { get => base.DisabledBackColor; set => base.DisabledBackColor = value; }

        /// <summary>
        /// トグルスイッチが無効なときのサムの色を取得または設定します。
        /// </summary>
        [DefaultValue(typeof(Color), "223, 223, 223")]
        public override Color DisabledThumbColor { get => base.DisabledThumbColor; set => base.DisabledThumbColor = value; }

        /// <summary>
        /// トグルスイッチのボーダー色を取得または設定します。
        /// </summary>
        [DefaultValue(typeof(Color), "222, 226, 230")]
        public override Color BorderColor { get => base.BorderColor; set => base.BorderColor = value; }

        /// <summary>
        /// トグルスイッチのテキストを取得または設定します。
        /// </summary>
        [DefaultValue("OFF")]
        public override string Text { get => base.Text; set => base.Text = value; }

        /// <summary>
        /// トグルスイッチのサムの色を取得または設定します。
        /// </summary>
        [DefaultValue(typeof(Color), "191, 191, 191")]
        public override Color ThumbColor { get => base.ThumbColor; set => base.ThumbColor = value; }
    }
}
