using System.ComponentModel;
using System.Drawing;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// <see cref="MetToggleSwitch"/> の外観を指定するプロパティの基底情報を用意します。
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class ToggleButtonAppearanceBase
    {
        private readonly MetToggleSwitch Owner;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="owner"><see cref="MetToggleSwitch"/>。</param>
        internal ToggleButtonAppearanceBase(MetToggleSwitch owner)
        {
            Owner = owner;
        }

        private Color _backColor = Color.White;

        /// <summary>
        /// トグルスイッチの背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffBackColor")]
        public virtual Color BackColor
        {
            get { return _backColor; }
            set
            {
                if (_backColor != value)
                {
                    _backColor = value;
                    Owner.Invalidate();
                }
            }
        }

        private Color _disabledBackColor = Color.FromArgb(243, 243, 243);

        /// <summary>
        /// トグルスイッチが無効なときの背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffDisabledBackColor")]
        public virtual Color DisabledBackColor
        {
            get { return _disabledBackColor; }
            set
            {
                if (_disabledBackColor != value)
                {
                    _disabledBackColor = value;
                    Owner.Invalidate();
                }
            }
        }

        private Color _disabledThumbColor = Color.FromArgb(155, 155, 155);

        /// <summary>
        /// トグルスイッチが無効なときのサムの色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffDisabledThumbColor")]
        public virtual Color DisabledThumbColor
        {
            get { return _disabledThumbColor; }
            set
            {
                if (_disabledThumbColor != value)
                {
                    _disabledThumbColor = value;
                    Owner.Invalidate();
                }
            }
        }

        private Color _borderColor = Color.FromArgb(118, 118, 118);

        /// <summary>
        /// トグルスイッチのボーダー色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffSwitchBorderColor")]
        public virtual Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    Owner.Invalidate();
                }
            }
        }

        private string _text = "OFF";

        /// <summary>
        /// トグルスイッチのテキストを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffText")]
        public virtual string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    //if (Owner.AutoSize)
                    //{
                    //    Owner.UpdateControlSize();
                    //}
                    Owner.Invalidate();
                }
            }
        }

        /// <summary>
        /// コントロールサイズの変更を通知せずトグルスイッチのテキストを変更します。
        /// </summary>
        /// <param name="text">テキスト。</param>
        protected void ChangeTextWithoutUpdateControlSize(string text)
        {
            _text = text;
        }

        private Color _textForeColor = SystemColors.ControlText;

        /// <summary>
        /// トグルスイッチのテキストの前景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffTextForeColor")]
        public virtual Color TextForeColor
        {
            get { return _textForeColor; }
            set
            {
                if (_textForeColor != value)
                {
                    _textForeColor = value;
                    Owner.Invalidate();
                }
            }
        }

        /// <summary>
        /// TextForeColor プロパティがデフォルト値でない場合に true を返します。
        /// </summary>
        /// <returns>変更がある場合は true, それ以外は false を返却する。</returns>
        private bool ShouldSerializeTextForeColor()
        {
            return _textForeColor != SystemColors.ControlText;
        }

        /// <summary>
        /// TextForeColor プロパティをデフォルト値にリセットします。
        /// </summary>
        private void ResetTextForeColor()
        {
            TextForeColor = SystemColors.ControlText;
        }

        // Empty の場合は自動色
        private Color _thumbColor = Color.Empty;

        /// <summary>
        /// トグルスイッチのサムの色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("MetToggleSwitchOffThumbColor")]
        public virtual Color ThumbColor
        {
            get { return _thumbColor; }
            set
            {
                if (_thumbColor != value)
                {
                    _thumbColor = value;
                    Owner.Invalidate();
                }
            }
        }

        /// <summary>
        /// 空の文字列を取得します。
        /// </summary>
        /// <returns>空の文字列。</returns>
        public override string ToString()
        {
            return string.Empty;
        }
    }
}
