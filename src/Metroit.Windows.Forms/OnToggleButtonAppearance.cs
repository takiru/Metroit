using System.ComponentModel;
using System.Drawing;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// <see cref="MetToggleSwitch"/> の外観を指定するプロパティを用意します。
    /// </summary>
    public class OnToggleButtonAppearance : ToggleButtonAppearanceBase
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="owner"><see cref="MetToggleSwitch"/>。</param>
        internal OnToggleButtonAppearance(MetToggleSwitch owner) : base(owner)
        {
            BackColor = Color.FromArgb(13, 110, 253);
            DisabledBackColor = Color.FromArgb(133, 182, 254);
            DisabledThumbColor = Color.White;
            BorderColor = Color.FromArgb(13, 110, 253);
            ChangeTextWithoutUpdateControlSize("ON");
            ThumbColor = Color.White;
        }

        /// <summary>
        /// トグルスイッチの背景色を取得または設定します。
        /// </summary>
        [DefaultValue(typeof(Color), "13, 110, 253")]
        public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }

        /// <summary>
        /// トグルスイッチが無効なときの背景色を取得または設定します。
        /// </summary>
        [DefaultValue(typeof(Color), "133, 182, 254")]
        public override Color DisabledBackColor { get => base.DisabledBackColor; set => base.DisabledBackColor = value; }

        /// <summary>
        /// トグルスイッチが無効なときのサムの色を取得または設定します。
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        public override Color DisabledThumbColor { get => base.DisabledThumbColor; set => base.DisabledThumbColor = value; }

        /// <summary>
        /// トグルスイッチのボーダー色を取得または設定します。
        /// </summary>
        [DefaultValue(typeof(Color), "13, 110, 253")]
        public override Color BorderColor { get => base.BorderColor; set => base.BorderColor = value; }

        /// <summary>
        /// トグルスイッチのテキストを取得または設定します。
        /// </summary>
        [DefaultValue("ON")]
        public override string Text { get => base.Text; set => base.Text = value; }

        /// <summary>
        /// トグルスイッチのサムの色を取得または設定します。
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        public override Color ThumbColor { get => base.ThumbColor; set => base.ThumbColor = value; }
    }
}
