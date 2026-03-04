using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// <see cref="MetRoundedButton"/> の外観を指定するプロパティを用意します。
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ExtendsAppearanceBase
    {
        /// <summary>
        /// オーナーのボタンオブジェクトを取得します。
        /// </summary>
        protected readonly ButtonBase Owner;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="owner">オーナーのボタンオブジェクト。</param>
        internal ExtendsAppearanceBase(ButtonBase owner)
        {
            Owner = owner;
        }

        private Color _borderColor = Color.FromArgb(13, 110, 253);

        /// <summary>
        /// ボタンを囲む境界線の色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "13, 110, 253")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MetCategory("MetAppearance")]
        [MetDescription("ExtendsAppearanceBaseBorderColor")]
        public virtual Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (value.Equals(Color.Transparent) || value.Equals(Color.Empty))
                {
                    throw new NotSupportedException(ExceptionResources.GetString("NotSupportedColor"));
                }

                if (_borderColor != value)
                {
                    _borderColor = value;
                    Owner.Invalidate();
                }
            }
        }

        private int _borderSize = 0;

        /// <summary>
        /// ボタンを囲む境界線のサイズをピクセル単位で指定する値を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MetCategory("MetAppearance")]
        [MetDescription("ExtendsAppearanceBaseBorderSize")]
        public virtual int BorderSize
        {
            get
            {
                return _borderSize;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(BorderSize), value, string.Format(ExceptionResources.GetString("InvalidIndex"), 0));
                }

                if (_borderSize != value)
                {
                    _borderSize = value;
                    Owner.Invalidate();
                }
            }
        }

        private Color _focusedBackColor = Color.FromArgb(11, 94, 215);

        /// <summary>
        /// フォーカスを有しているか、マウス ポインターがコントロールの境界内にあるときの、ボタンのクライアント領域の色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "11, 94, 215")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MetCategory("MetAppearance")]
        [MetDescription("ExtendsAppearanceBaseFocusedBackColor")]
        public virtual Color FocusedBackColor
        {
            get => _focusedBackColor;
            set
            {
                if (value.Equals(Color.Transparent) || value.Equals(Color.Empty))
                {
                    throw new NotSupportedException(ExceptionResources.GetString("NotSupportedColor"));
                }

                if (_focusedBackColor != value)
                {
                    _focusedBackColor = value;
                    Owner.Invalidate();
                }
            }
        }

        private Color _focusedForeColor = Color.White;

        /// <summary>
        /// フォーカスを有しているか、マウス ポインターがコントロールの境界内にあるときの、前景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "White")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MetCategory("MetAppearance")]
        [MetDescription("ExtendsAppearanceBaseFocusedForeColor")]
        public virtual Color FocusedForeColor
        {
            get => _focusedForeColor;
            set
            {
                if (value.Equals(Color.Transparent) || value.Equals(Color.Empty))
                {
                    throw new NotSupportedException(ExceptionResources.GetString("NotSupportedColor"));
                }

                if (_focusedForeColor != value)
                {
                    _focusedForeColor = value;
                    Owner.Invalidate();
                }
            }
        }

        private Color _pressedBackColor = Color.FromArgb(10, 88, 202);

        /// <summary>
        /// スペースキーが押されたか、マウスがコントロールの境界内でクリックされたときの、ボタンのクライアント領域の色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "10, 88, 202")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MetCategory("MetAppearance")]
        [MetDescription("ExtendsAppearanceBasePressedBackColor")]
        public virtual Color PressedBackColor
        {
            get => _pressedBackColor;
            set
            {
                if (value.Equals(Color.Transparent) || value.Equals(Color.Empty))
                {
                    throw new NotSupportedException(ExceptionResources.GetString("NotSupportedColor"));
                }

                if (_pressedBackColor != value)
                {
                    _pressedBackColor = value;
                    Owner.Invalidate();
                }
            }
        }

        private Color _pressedForeColor = Color.White;

        /// <summary>
        /// スペースキーが押されたか、マウスがコントロールの境界内でクリックされたときの、前景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "White")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MetCategory("MetAppearance")]
        [MetDescription("ExtendsAppearanceBasePressedForeColor")]
        public virtual Color PressedForeColor
        {
            get => _pressedForeColor;
            set
            {
                if (value.Equals(Color.Transparent) || value.Equals(Color.Empty))
                {
                    throw new NotSupportedException(ExceptionResources.GetString("NotSupportedColor"));
                }

                if (_pressedForeColor != value)
                {
                    _pressedForeColor = value;
                    Owner.Invalidate();
                }
            }
        }

        private int _focusOverlayWidth = 4;

        /// <summary>
        /// フォーカスを得た時に表示されるオーバーレイの幅を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(4)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MetCategory("MetAppearance")]
        [MetDescription("ExtendsAppearanceBaseFocusOverlayWidth")]
        public virtual int FocusOverlayWidth
        {
            get => _focusOverlayWidth;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(FocusOverlayWidth));
                }

                if (_focusOverlayWidth != value)
                {
                    _focusOverlayWidth = value;
                    Owner.Invalidate();
                }
            }
        }

        private Color _focusOverlayColor = Color.FromArgb(152, 193, 254);

        /// <summary>
        /// フォーカスを得た時に表示されるオーバーレイの色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "152, 193, 254")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MetCategory("MetAppearance")]
        [MetDescription("ExtendsAppearanceBaseFocusOverlayColor")]
        public virtual Color FocusOverlayColor
        {
            get => _focusOverlayColor;
            set
            {
                if (value.Equals(Color.Transparent) || value.Equals(Color.Empty))
                {
                    throw new NotSupportedException(ExceptionResources.GetString("NotSupportedColor"));
                }

                if (_focusOverlayColor != value)
                {
                    _focusOverlayColor = value;
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
