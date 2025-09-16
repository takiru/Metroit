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

        private Color _mouseOverBackColor = Color.FromArgb(11, 94, 215);

        /// <summary>
        /// マウス ポインターがコントロールの境界内にあるときの、ボタンのクライアント領域の色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "11, 94, 215")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MetCategory("MetAppearance")]
        [MetDescription("ExtendsAppearanceBaseMouseOverBackColor")]
        public virtual Color MouseOverBackColor
        {
            get => _mouseOverBackColor;
            set
            {
                if (value.Equals(Color.Transparent) || value.Equals(Color.Empty))
                {
                    throw new NotSupportedException(ExceptionResources.GetString("NotSupportedColor"));
                }

                if (_mouseOverBackColor != value)
                {
                    _mouseOverBackColor = value;
                    Owner.Invalidate();
                }
            }
        }

        private Color _mouseOverForeColor = Color.White;

        /// <summary>
        /// マウス ポインターがコントロールの境界内にあるときの、前景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "White")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MetCategory("MetAppearance")]
        [MetDescription("ExtendsAppearanceBaseMouseOverForeColor")]
        public virtual Color MouseOverForeColor
        {
            get => _mouseOverForeColor;
            set
            {
                if (value.Equals(Color.Transparent) || value.Equals(Color.Empty))
                {
                    throw new NotSupportedException(ExceptionResources.GetString("NotSupportedColor"));
                }

                if (_mouseOverForeColor != value)
                {
                    _mouseOverForeColor = value;
                    Owner.Invalidate();
                }
            }
        }

        private Color _mouseDownBackColor = Color.FromArgb(10, 88, 202);

        /// <summary>
        /// マウスがコントロールの境界内でクリックされたときの、ボタンのクライアント領域の色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "10, 88, 202")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MetCategory("MetAppearance")]
        [MetDescription("ExtendsAppearanceBaseMouseDownBackColor")]
        public virtual Color MouseDownBackColor
        {
            get => _mouseDownBackColor;
            set
            {
                if (value.Equals(Color.Transparent) || value.Equals(Color.Empty))
                {
                    throw new NotSupportedException(ExceptionResources.GetString("NotSupportedColor"));
                }

                if (_mouseDownBackColor != value)
                {
                    _mouseDownBackColor = value;
                    Owner.Invalidate();
                }
            }
        }

        private Color _mouseDownForeColor = Color.White;

        /// <summary>
        /// マウスがコントロールの境界内でクリックされたときの、前景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "White")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MetCategory("MetAppearance")]
        [MetDescription("ExtendsAppearanceBaseMouseDownForeColor")]
        public virtual Color MouseDownForeColor
        {
            get => _mouseDownForeColor;
            set
            {
                if (value.Equals(Color.Transparent) || value.Equals(Color.Empty))
                {
                    throw new NotSupportedException(ExceptionResources.GetString("NotSupportedColor"));
                }

                if (_mouseDownForeColor != value)
                {
                    _mouseDownForeColor = value;
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
