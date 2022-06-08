﻿using Metroit.Windows.Forms.ComponentModel;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Metroit.Windows.Forms.Extensions;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// ユーザーが数値のみを入力できるようにしたテキストエリアを提供します。
    /// </summary>
    /// <remarks>
    /// テキストボックスとしての機能をいくつか拡張し、利用することができます。<br />
    /// [拡張機能]<br />
    /// 　・入力可能範囲の設定。<br />
    /// 　・null許可制御。<br />
    /// 　・入力値の表示フォーマット設定。<br />
    /// 　・マイナス値の文字色設定。<br />
    /// 　・以下の標準プロパティは利用できません。<br />
    /// 　　　・MaxLength<br />
    /// 　　　・Multiline<br />
    /// 　　　・PasswordChar<br />
    /// 　　　・UseSystemPasswordChar<br />
    /// 　　　・AcceptsReturn<br />
    /// 　　　・AcceptsTab<br />
    /// 　　　・CharacterCasing<br />
    /// </remarks>
    [ToolboxItem(true)]
    public class MetNumericTextBox : MetTextBox, ISupportInitialize
    {
        /// <summary>
        /// MetNumberFormatTextBox の新しいインスタンスを初期化します。
        /// </summary>
        public MetNumericTextBox() : base()
        {
            this.TextAlign = HorizontalAlignment.Right;
            this.textFormatting = true;
            base.Text = "0";
            this.textFormatting = false;
            base.ImeMode = ImeMode.Disable;

            if (this.IsDesignMode())
            {
                return;
            }

            this.Enter += MetNumericFormatTextBox_Enter;
            this.TextChanged += MetNumericFormatTextBox_TextChanged;
        }

        #region イベント

        // フォーカスを失った時のキャレット位置
        private int leavedSelectionStart = 0;
        private int leavedSelectionLength = 0;
        private decimal? enterValue = null;

        /// <summary>
        /// フォーカスを得た時、数値で描画しなおす。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetNumericFormatTextBox_Enter(object sender, EventArgs e)
        {
            if (!IsValidatingCanceled)
            {
                this.enterValue = this.value;
            }

            this.textFormatting = true;
            if (this.value.HasValue)
            {
                base.Text = this.value.ToString();
            }
            else
            {
                base.Text = "";
            }
            this.textFormatting = false;
            this.ChangeDisplayColor();
            if (!this.FocusSelect)
            {
                this.SelectionStart = this.leavedSelectionStart;
                this.SelectionLength = this.leavedSelectionLength;
            }
        }

        /// <summary>
        /// フォーカスを失った時、数値をフォーマット化する。
        /// NOTE: NumericUpDown コントロールに準拠し、ウィンドウから離れた場合に値が確定されるようにする。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            this.leavedSelectionStart = this.SelectionStart;
            this.leavedSelectionLength = this.SelectionLength;

            // 小数記号、負数記号しか入力がない場合は空文字を見なす
            if (base.Text == "." || base.Text == "-")
            {
                this.textFormatting = true;
                base.Text = "";
                this.textFormatting = false;
            }

            // 入力値を保存し、フォーマットされた値で描画しなおす
            if (string.IsNullOrEmpty(base.Text))
            {
                if (this.AcceptNull)
                {
                    this.value = null;
                }
                else
                {
                    this.value = MinValue;
                }
            }
            else
            {
                this.value = decimal.Parse(base.Text);
            }

            // NOTE: isValidRangeValue() によって値が範囲外の場合は強制的に最小値/最大値とする
            if (value > MaxValue)
            {
                this.value = this.MaxValue;
            }
            if (value < MinValue)
            {
                this.value = this.MinValue;
            }

            // 内部値と異なるもしくは、フォーカスを得た時に値が入っており、nullに変更した場合に変化があったとする
            if ((this.value != this.internalValue) || (this.value == null && this.internalValue == null && this.enterValue != null))
            {
                this.internalValue = this.value;

                // 入力値チェックイベント
                var e2 = new TextChangeValidationEventArgs();
                e2.Cancel = false;
                e2.Before = this.value.ToString();
                e2.Input = value.ToString();
                e2.After = value.ToString();
                this.OnTextChangeValidation(e2);
                if (e2.Cancel)
                {
                    throw new DeniedTextException(
                            ExceptionResources.GetString("DeniedTextException"), value.ToString());
                }

                OnValueChanged(new EventArgs());
            }

            this.textFormatting = true;
            base.Text = this.createFormattedText(this.value);
            this.textFormatting = false;

            this.ChangeDisplayColor();

            base.OnLostFocus(e);
        }

        /// <summary>
        /// テキストが変わったらValueに値を設定し、背景色・文字色を変更する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetNumericFormatTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.valueDirectSetting)
            {
                this.ChangeDisplayColor();
                return;
            }

            decimal value;
            if (base.Text == "" || !decimal.TryParse(base.Text, out value))
            {
                if (this.AcceptNull)
                {
                    this.value = null;
                }
                else
                {
                    this.value = 0;
                }
            }
            else
            {
                this.value = value;
            }
            this.ChangeDisplayColor();
        }

        /// <summary>
        /// マイナス記号表現が変更された時、表示を再描画する。
        /// </summary>
        /// <param name="sender">object オブジェクト。</param>
        /// <param name="e">EventArgs オブジェクト。</param>
        private void NegativePattern_PatternChanged(object sender, EventArgs e)
        {
            if (this.Value.HasValue && this.value < 0)
            {
                this.textFormatting = true;
                base.Text = this.createFormattedText(this.value);
                this.textFormatting = false;
            }
        }

        /// <summary>
        /// プラス記号表現が変更された時、表示を再描画する。
        /// </summary>
        /// <param name="sender">object オブジェクト。</param>
        /// <param name="e">EventArgs オブジェクト。</param>
        private void PositivePattern_PatternChanged(object sender, EventArgs e)
        {
            if (this.Value.HasValue && this.value >= 0)
            {
                this.textFormatting = true;
                base.Text = this.createFormattedText(this.value);
                this.textFormatting = false;
            }
        }

        #endregion

        #region 追加イベント

        /// <summary>
        /// 値が変更された時に発生します。
        /// </summary>
        [MetCategory("MetPropertyChange")]
        [MetDescription("MetNumericTextBoxTextValueChanged")]
        public event EventHandler ValueChanged;

        /// <summary>
        /// 値が変更された時に走行します。
        /// </summary>
        /// <param name="e">イベントオブジェクト。</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        #endregion

        #region プロパティ

        private HorizontalAlignment _textAlign = HorizontalAlignment.Right;

        /// <summary>
        /// エディットコントロールに対してどのようにテキストを配置するかを示します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(HorizontalAlignment.Right)]
        public new HorizontalAlignment TextAlign
        {
            get { return base.TextAlign; }
            set
            {
                _textAlign = value;
                base.TextAlign = _textAlign;
            }
        }

        /// <summary>
        /// 現在のテキストを取得します。数値の取得または設定は、Valueプロパティを利用してください。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Text
        {
            get => base.Text;
        }

        /// <summary>
        /// このプロパティの使用は禁止されています。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("このプロパティの使用は禁止されています。", true)]
        public new ImeMode ImeMode
        {
            get { return ImeMode.Disable; }
        }

        /// <summary>
        /// このプロパティの使用は禁止されています。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("このプロパティの使用は禁止されています。", true)]
        public new int MaxLength
        {
            get { return 0; }
        }

        /// <summary>
        /// このプロパティの使用は禁止されています。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("このプロパティの使用は禁止されています。", true)]
        public new bool Multiline
        {
            get { return false; }
        }

        /// <summary>
        /// このプロパティの使用は禁止されています。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("このプロパティの使用は禁止されています。", true)]
        public new char PasswordChar
        {
            get { return new char(); }
        }

        /// <summary>
        /// このプロパティの使用は禁止されています。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("このプロパティの使用は禁止されています。", true)]
        public new bool UseSystemPasswordChar
        {
            get { return false; }
        }

        /// <summary>
        /// このプロパティの使用は禁止されています。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("このプロパティの使用は禁止されています。", true)]
        public new bool AcceptsReturn
        {
            get { return false; }
        }

        /// <summary>
        /// このプロパティの使用は禁止されています。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("このプロパティの使用は禁止されています。", true)]
        public new bool AcceptsTab
        {
            get { return false; }
        }

        /// <summary>
        /// このプロパティの使用は禁止されています。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("このプロパティの使用は禁止されています。", true)]
        public new CharacterCasing CharacterCasing
        {
            get { return CharacterCasing.Normal; }
        }

        /// <summary>
        /// このプロパティの使用は禁止されています。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("このプロパティの使用は禁止されています。", true)]
        public new string[] Lines
        {
            get { return new string[] { this.Text }; }
        }

        /// <summary>
        /// このプロパティの使用は禁止されています。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("このプロパティの使用は禁止されています。", true)]
        public new ScrollBars ScrollBars
        {
            get { return ScrollBars.None; }
        }

        /// <summary>
        /// このプロパティの使用は禁止されています。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("このプロパティの使用は禁止されています。", true)]
        public new RightToLeft RightToLeft
        {
            get { return RightToLeft.No; }
        }

        /// <summary>
        /// このプロパティの使用は禁止されています。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("このプロパティの使用は禁止されています。", true)]
        public new bool MultilineSelectAll
        {
            get { return false; }
        }

        #endregion

        #region 追加プロパティ

        private const int MAX_DIGITS = 10;
        private const string DECIMAL_SEPARATOR = @".";
        private const string GROUP_SEPARATOR = @",";
        private const string NEGATIVE_SIGN = @"-";
        private const string CURRENCY_SYMBOL = @"\";
        private const string PERCENT_SYMBOL = @"%";

        private bool valueDirectSetting = false;
        private bool textFormatting = false;
        private decimal? value = 0;
        private NumericFormatMode mode = NumericFormatMode.Numeric;
        private Color negativeForeColor = Color.Red;
        private bool acceptNegative = true;
        private decimal maxValue = decimal.MaxValue;
        private decimal minValue = decimal.MinValue;
        private int decimalDigits = 0;
        private bool acceptNull = false;
        private string decimalSeparator = DECIMAL_SEPARATOR;
        private string groupSeparator = GROUP_SEPARATOR;
        private int[] groupSizes = new int[] { 3 };
        private NumericPositivePattern positivePattern = new NumericPositivePattern();
        private NumericNegativePattern negativePattern = new NumericNegativePattern();
        private string currencySymbol = CURRENCY_SYMBOL;
        private string percentSymbol = PERCENT_SYMBOL;
        private string negativeSign = NEGATIVE_SIGN;

        private decimal? internalValue;

        /// <summary>
        /// 現在の数値を取得または設定します。
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [DefaultValue(typeof(decimal), "0")]
        [MetDescription("ControlValue")]
        public decimal? Value
        {
            get
            {
                // NOTE: MenuStrip などのフォーカス遷移が行われないコントロールから値を取得しようとした時、値を確定する必要がある
                //       isValidRangeValue() によって値が範囲外の場合は強制的に最小値/最大値とする
                if (value < MinValue)
                {
                    value = MinValue;
                }
                if (value > MaxValue)
                {
                    value = MaxValue;
                }

                textFormatting = true;
                var formattedText = createFormattedText(value);
                base.Text = formattedText;

                if (value != internalValue)
                {
                    internalValue = value;

                    // 入力値チェックイベント
                    var e = new TextChangeValidationEventArgs();
                    e.Cancel = false;
                    e.Before = value.ToString();
                    e.Input = value.ToString();
                    e.After = value.ToString();
                    OnTextChangeValidation(e);
                    if (e.Cancel)
                    {
                        throw new DeniedTextException(
                                ExceptionResources.GetString("DeniedTextException"), value.ToString());
                    }

                    valueDirectSetting = true;
                    OnValueChanged(new EventArgs());
                    valueDirectSetting = false;
                }
                textFormatting = false;

                return this.value;
            }
            set
            {
                // 設定値に変更がない場合は処理しない
                if (this.value == value)
                {
                    return;
                }

                // 入力値チェックイベント
                var e = new TextChangeValidationEventArgs();
                e.Cancel = false;
                e.Before = this.value.ToString();
                e.Input = value.ToString();
                e.After = value.ToString();
                this.OnTextChangeValidation(e);
                if (e.Cancel)
                {
                    throw new DeniedTextException(
                            ExceptionResources.GetString("DeniedTextException"), value.ToString());
                }

                this.valueDirectSetting = true;
                this.textFormatting = true;
                this.value = value;
                this.internalValue = value;
                base.Text = this.createFormattedText(value);
                OnValueChanged(new EventArgs());
                this.textFormatting = false;
                this.valueDirectSetting = false;
                this.ChangeDisplayColor();
            }
        }

        /// <summary>
        /// フォーカスがない時に表示される形式を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [DefaultValue(NumericFormatMode.Numeric)]
        [MetDescription("ControlMode")]
        public NumericFormatMode Mode
        {
            get { return this.mode; }
            set
            {
                this.mode = value;
                this.textFormatting = true;
                base.Text = this.createFormattedText(this.value);
                this.textFormatting = false;
            }
        }

        /// <summary>
        /// マイナス時の文字色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [DefaultValue(typeof(Color), "Red")]
        [MetDescription("ControlNegativeForeColor")]
        public Color NegativeForeColor
        {
            get { return this.negativeForeColor; }
            set
            {
                this.negativeForeColor = value;
                if (this.FindForm() != null)
                {
                    this.ChangeDisplayColor();
                }
            }
        }

        /// <summary>
        /// マイナス値の入力許可を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(true)]
        [MetDescription("ControlAcceptNegative")]
        public bool AcceptNegative
        {
            get { return this.acceptNegative; }
            set
            {
                // Value値が負数の場合はNG
                if (this.value.HasValue && this.value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                            ExceptionResources.GetString("AcceptNegativeCanNotChange"));
                }

                this.acceptNegative = value;
            }
        }

        /// <summary>
        /// 入力可能な最大値を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(typeof(decimal), "79228162514264337593543950335")]
        [MetDescription("ControlMaxValue")]
        public decimal MaxValue
        {
            get { return this.maxValue; }
            set
            {
                // MinValueを下回る
                if (this.MinValue > value)
                {
                    throw new ArgumentOutOfRangeException(
                            ExceptionResources.GetString("MinValueSmallerCanNotChange"));
                }

                // デザイン時にValueがMinValue, MaxValue の範囲外となる場合
                if (this.IsDesignMode() && this.value.HasValue && !(this.value >= this.MinValue && this.value <= value))
                {
                    this.maxValue = value;
                    this.Value = this.MinValue;
                    return;
                }

                // Value値を下回る
                if (this.value.HasValue && this.value > value && Initialized)
                {
                    throw new ArgumentOutOfRangeException(
                            ExceptionResources.GetString("ValueSmallerCanNotChange"));
                }

                this.maxValue = value;
            }
        }

        /// <summary>
        /// 入力可能な最小値を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(typeof(decimal), "-79228162514264337593543950335")]
        [MetDescription("ControlMinValue")]
        public decimal MinValue
        {
            get { return this.minValue; }
            set
            {
                // MaxValueを上回る
                if (this.MaxValue < value)
                {
                    throw new ArgumentOutOfRangeException(
                            ExceptionResources.GetString("MaxValueSmallerCanNotChange"));
                }

                // デザイン時にValueがMinValue, MaxValue の範囲外となる場合
                if (this.IsDesignMode() && this.value.HasValue && !(this.value >= value && this.value <= this.MaxValue))
                {
                    this.minValue = value;
                    this.Value = this.MinValue;
                    return;
                }

                // Value値を上回る
                if (this.value.HasValue && this.value < value && Initialized)
                {
                    throw new ArgumentOutOfRangeException(
                            ExceptionResources.GetString("ValueLargerCanNotChange"));
                }

                this.minValue = value;
            }
        }

        /// <summary>
        /// 入力可能な小数桁数の取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(0)]
        [MetDescription("ControlDecimalDigits")]
        public int DecimalDigits
        {
            get { return this.decimalDigits; }
            set
            {
                int digitValue = value;

                // 最大小数桁数を超えた場合は最大小数桁数とする
                if (digitValue > MAX_DIGITS)
                {
                    digitValue = MAX_DIGITS;
                }

                this.decimalDigits = digitValue;
                if (this.FindForm() != null)
                {
                    this.textFormatting = true;
                    base.Text = this.createFormattedText(this.value);
                    this.textFormatting = false;
                }
            }
        }

        /// <summary>
        /// NULLの入力を受け入れるかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(false)]
        [MetDescription("ControlAcceptNull")]
        public bool AcceptNull
        {
            get => this.acceptNull;
            set
            {
                this.acceptNull = value;
                if (!value && !this.value.HasValue)
                {
                    this.Value = this.MinValue;
                }
            }
        }

        /// <summary>
        /// 整数部と小数部の区切り文字を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(DECIMAL_SEPARATOR)]
        [MetDescription("ControlDecimalSeparator")]
        public string DecimalSeparator
        {
            get { return this.decimalSeparator; }
            set
            {
                this.decimalSeparator = value;
                if (this.FindForm() != null)
                {
                    this.textFormatting = true;
                    base.Text = this.createFormattedText(this.value);
                    this.textFormatting = false;
                }
            }
        }

        /// <summary>
        /// 整数部の桁区切り文字を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(GROUP_SEPARATOR)]
        [MetDescription("ControlGroupSeparator")]
        public string GroupSeparator
        {
            get { return this.groupSeparator; }
            set
            {
                this.groupSeparator = value;
                if (this.FindForm() != null)
                {
                    this.textFormatting = true;
                    base.Text = this.createFormattedText(this.value);
                    this.textFormatting = false;
                }
            }
        }

        /// <summary>
        /// 整数部の区切り桁数を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(new int[] { 3 })]
        [MetDescription("ControlGroupSizes")]
        public int[] GroupSizes
        {
            get { return this.groupSizes; }
            set
            {
                this.groupSizes = value;
                if (this.FindForm() != null)
                {
                    this.textFormatting = true;
                    base.Text = this.createFormattedText(this.value);
                    this.textFormatting = false;
                }
            }
        }

        /// <summary>
        /// 正数の時の記号表現を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [MetDescription("ControlPositivePattern")]
        [Editor(typeof(NumericPositivePatternEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public NumericPositivePattern PositivePattern
        {
            get { return this.positivePattern; }
            set
            {
                this.positivePattern = value;
                this.textFormatting = true;
                base.Text = this.createFormattedText(this.value);
                this.textFormatting = false;
            }
        }

        /// <summary>
        /// 負数の時の記号表現を設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [MetDescription("ControlNegativePattern")]
        [Editor(typeof(NumericNegativePatternEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public NumericNegativePattern NegativePattern
        {
            get { return this.negativePattern; }
            set
            {
                this.negativePattern = value;
                this.textFormatting = true;
                base.Text = this.createFormattedText(this.value);
                this.textFormatting = false;
            }
        }

        /// <summary>
        /// 通貨表現の時の記号を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(CURRENCY_SYMBOL)]
        [MetDescription("ControlCurrencySymbol")]
        public string CurrencySymbol
        {
            get { return this.currencySymbol; }
            set
            {
                this.currencySymbol = value;
                if (this.FindForm() != null)
                {
                    this.textFormatting = true;
                    base.Text = this.createFormattedText(this.value);
                    this.textFormatting = false;
                }
            }
        }

        /// <summary>
        /// パーセント表現の時の記号を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(PERCENT_SYMBOL)]
        [MetDescription("ControlPercentSymbol")]
        public string PercentSymbol
        {
            get { return percentSymbol; }
            set
            {
                this.percentSymbol = value;
                if (this.FindForm() != null)
                {
                    this.textFormatting = true;
                    base.Text = this.createFormattedText(this.value);
                    this.textFormatting = false;
                }
            }
        }

        /// <summary>
        /// 負数の時の記号を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetBehavior")]
        [DefaultValue(NEGATIVE_SIGN)]
        [MetDescription("ControlNegativeSign")]
        public string NegativeSign
        {
            get { return this.negativeSign; }
            set
            {
                this.negativeSign = value;
                if (this.FindForm() != null)
                {
                    this.textFormatting = true;
                    base.Text = this.createFormattedText(this.value);
                    this.textFormatting = false;
                }
            }
        }

        /// <summary>
        /// 数値データを記号表現に対応したフォーマットに変換する
        /// </summary>
        /// <param name="value">数値データ</param>
        private string createFormattedText(decimal? value)
        {
            if (!value.HasValue)
            {
                return "";
            }

            string formattedText = value.ToString();

            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = this.DecimalSeparator;
            nfi.CurrencyDecimalSeparator = this.DecimalSeparator;
            nfi.PercentDecimalSeparator = this.DecimalSeparator;
            nfi.NumberGroupSeparator = this.GroupSeparator;
            nfi.CurrencyGroupSeparator = this.GroupSeparator;
            nfi.PercentGroupSeparator = this.GroupSeparator;
            nfi.NumberGroupSizes = this.GroupSizes;
            nfi.CurrencyGroupSizes = this.GroupSizes;
            nfi.PercentGroupSizes = this.GroupSizes;
            nfi.NumberDecimalDigits = this.DecimalDigits;
            nfi.CurrencyDecimalDigits = this.DecimalDigits;
            nfi.PercentDecimalDigits = this.DecimalDigits;
            nfi.CurrencySymbol = this.CurrencySymbol;
            nfi.PercentSymbol = this.PercentSymbol;
            nfi.NumberNegativePattern = (int)this.NegativePattern.NumberNegativePattern;
            nfi.CurrencyNegativePattern = (int)this.NegativePattern.CurrencyNegativePattern;
            nfi.PercentNegativePattern = (int)this.NegativePattern.PercentNegativePattern;
            nfi.CurrencyPositivePattern = (int)this.PositivePattern.CurrencyPositivePattern;
            nfi.PercentPositivePattern = (int)this.PositivePattern.PercentPositivePattern;
            nfi.NegativeSign = this.NegativeSign;
            switch (this.Mode)
            {
                case NumericFormatMode.Numeric:
                    formattedText = value.Value.ToString("N", nfi);
                    break;

                case NumericFormatMode.Currency:
                    formattedText = value.Value.ToString("C", nfi);
                    break;

                case NumericFormatMode.Percent:
                    var formatValue = value / 100;
                    formattedText = formatValue.Value.ToString("P", nfi);
                    break;

            }

            return formattedText;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 記号表現が変更された時のイベントを登録します。
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new void EndInit()
        {
            base.EndInit();

            NegativePattern.PatternChanged += NegativePattern_PatternChanged;
            PositivePattern.PatternChanged += PositivePattern_PatternChanged;
        }

        /// <summary>
        /// ロールバック済みかどうかを取得します。
        /// </summary>
        /// <returns>true:ロールバック済み, false:未ロールバック。</returns>
        public override bool IsRollbacked => this.value == this.enterValue;

        /// <summary>
        /// フォーカスを得た時の値にロールバックを行います。
        /// </summary>
        public override void Rollback()
        {
            Rollbacking = true;
            if (this.enterValue.HasValue)
            {
                base.Text = this.enterValue.ToString();
                this.value = this.enterValue;
            }
            else
            {
                base.Text = "";
                this.value = null;
            }
            this.internalValue = null;
            this.ChangeDisplayColor();
            Rollbacking = false;
        }

        /// <summary>
        /// 特定動作が行われた時のTextChanged イベントを制御する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            // コントロールの生成準備完了前はTextChangedイベントを走行させない
            if (!this.Initialized)
            {
                return;
            }

            // valueの直接変更時はtextFormatting=trueだが、TextChangedイベントを走行させる
            if (this.valueDirectSetting)
            {
                base.OnTextChanged(e);
                return;
            }

            // Textプロパティのコードによる変更中はTextChangedイベントを走行させない
            if (this.textFormatting)
            {
                return;
            }

            base.OnTextChanged(e);
        }

        /// <summary>
        /// テキスト変更中、許可文字チェックを行います。
        /// </summary>
        /// <param name="e">入力値過程オブジェクト</param>
        protected override sealed void OnTextChangeValidation(TextChangeValidationEventArgs e)
        {
            // Textプロパティのコードによる変更中はTextChangeValidationイベントを走行させない
            if (this.textFormatting)
            {
                return;
            }

            // AcceptNull中は許可する
            if (this.AcceptNull && string.IsNullOrEmpty(e.After))
            {
                base.OnTextChangeValidation(e);
                return;
            }

            // 小数値の打ち始めのみ許可する
            if (this.DecimalDigits > 0 && e.After == ".")
            {
                base.OnTextChangeValidation(e);
                return;
            }

            // マイナスの打ち始めのみ許可する
            if (this.AcceptNegative && e.After == "-")
            {
                base.OnTextChangeValidation(e);
                return;
            }

            // 小数桁を許可しない場合は許可しない
            if (this.DecimalDigits == 0 && e.Input == ".")
            {
                e.Cancel = true;
                return;
            }

            // 数値チェック
            if (!this.isNumeric(e.After))
            {
                e.Cancel = true;
                return;
            }

            // 形式チェック
            var value = decimal.Parse(e.After);
            if (!this.isValidNegative(value))
            {
                e.Cancel = true;
                return;
            }
            if (!this.isValidDecimalDigits(value))
            {
                e.Cancel = true;
                return;
            }
            if (!this.isValidRangeValue(value))
            {
                e.Cancel = true;
                return;
            }

            base.OnTextChangeValidation(e);
        }

        /// <summary>
        /// 数値桁数を考慮して、オートフォーカスを行うか確認します。
        /// </summary>
        /// <returns>true:オートフォーカス可, false:オートフォーカス不可</returns>
        protected override bool CanAutoFocus()
        {
            // ロールバック中は実施しない
            if (Rollbacking)
            {
                return false;
            }

            // フォーマット変更中は実施しない
            if (this.textFormatting)
            {
                return false;
            }

            // 数値変換できない場合は入力中
            decimal value;
            if (!decimal.TryParse(base.Text, out value))
            {
                return false;
            }

            // MaxValueと値が一緒だったらOK
            if (this.MaxValue == value)
            {
                return true;
            }

            // MinValueと値が一緒だったらOK
            if (this.MinValue == value)
            {
                return true;
            }

            var valueIntLength = value.ToString("+###0;-###0;").Length;
            var maxValueIntLength = this.MaxValue.ToString("+###0;-###0;").Length;
            var minValueIntLength = this.MinValue.ToString("+###0;-###0;").Length;

            // 小数桁数が0の時、Valueの符号によってMaxValue, MinValue と比較して判定する
            if (this.DecimalDigits == 0)
            {
                if (value == 0)
                {
                    return true;
                }

                if (this.value > 0 && maxValueIntLength == valueIntLength)
                {
                    return true;
                }
                if (this.value < 0 && minValueIntLength == valueIntLength)
                {
                    return true;
                }

                return false;
            }

            // MaxValueより整数桁が少ない場合は、小数がフル桁入ったらOK
            // MaxValueと整数桁が同じだが、値が小さい場合は、小数がフル桁入ったらOK
            if ((maxValueIntLength > valueIntLength) ||
                (maxValueIntLength == valueIntLength && this.MaxValue > value))
            {
                int valueDigits = decimal.GetBits(value)[3] >> 16 & 0x00FF;
                if (this.DecimalDigits == valueDigits)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // MinValueより整数桁が少ない場合は、小数がフル桁入ったらOK
            // MinValueと整数桁が同じだが、値が大きい場合は、小数がフル桁入ったらOK
            if ((minValueIntLength > valueIntLength) ||
                (minValueIntLength == valueIntLength && this.MinValue < value))
            {
                int valueDigits = decimal.GetBits(value)[3] >> 16 & 0x00FF;
                if (this.DecimalDigits == valueDigits)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// コントロールのアクティブ状態に応じた表示色に変更する。
        /// </summary>
        protected override void ChangeDisplayColor()
        {
            base.ChangeDisplayColor();

            // マイナス値は強制的にマイナス色に変更
            if ((this.value.HasValue && this.value < 0) || base.Text.IndexOf(this.NegativeSign) >= 0)
            {
                base.ForeColor = this.NegativeForeColor;
            }
        }

        /// <summary>
        /// 入力値が数値かどうか。
        /// </summary>
        /// <param name="after">入力値</param>
        /// <returns>true:数値, false:数値でない</returns>
        private bool isNumeric(string after)
        {
            var pattern = @"^[-]?[.]?\d+[.]?\d*$";
            if (!this.AcceptNegative)
            {
                pattern = @"^[.]?\d+[.]?\d*$";
            }

            var regx = new System.Text.RegularExpressions.Regex(pattern);
            if (!regx.IsMatch(after))
            {
                return false;
            }

            decimal result;
            if (!decimal.TryParse(after, out result))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 入力値にマイナス許可があるかどうか。
        /// </summary>
        /// <param name="value">入力値</param>
        /// <returns>true:許可あり, false:許可なし</returns>
        private bool isValidNegative(decimal value)
        {
            if (!this.AcceptNegative && value < 0)
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 入力値が有効小数桁数かどうか。
        /// </summary>
        /// <param name="value">入力値</param>
        /// <returns>true:有効, false:無効</returns>
        private bool isValidDecimalDigits(decimal value)
        {
            if (getDecimalDigitsLength(value) > this.DecimalDigits)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 入力値の小数桁数を取得する。
        /// </summary>
        /// <param name="value">入力値。</param>
        /// <returns>小数桁数。</returns>
        private int getDecimalDigitsLength(decimal value)
        {
            return decimal.GetBits(value)[3] >> 16 & 0x00FF;
        }

        /// <summary>
        /// 最小値 ≦ 入力値 ≦ 最大値かどうか。
        /// NOTE: MinValue:10, MaxValue:100などの時、Value: 1とすることが可能なため、Leaveイベント内で、範囲外の値の場合は最小値 / 最大値に切り替える。
        /// </summary>
        /// <param name="value">入力値</param>
        /// <returns>true:有効, false:無効</returns>
        private bool isValidRangeValue(decimal value)
        {
            // 整数部の値検証
            if (!hasRangeIntValue(value))
            {
                return false;
            }

            // 小数部の評価が不要な場合は範囲内
            if (getDecimalDigitsLength(value) == 0)
            {
                return true;
            }

            // 小数部の値検証
            if (!hasRangeDecimalDigitsValue(value))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 整数部が範囲内かどうかを取得する。
        /// </summary>
        /// <param name="value">入力値。</param>
        /// <returns>true:範囲内, false:範囲外。</returns>
        private bool hasRangeIntValue(decimal value)
        {
            var valueInt = Math.Truncate(value);
            var maxValueInt = Math.Truncate(this.MaxValue);
            var minValueInt = Math.Truncate(this.MinValue);

            var valueIntLength = valueInt == 0 ? 1 : (int)Math.Log10((double)Math.Abs(valueInt)) + 1;
            var maxValueIntLength = maxValueInt == 0 ? 1 : (int)Math.Log10((double)Math.Abs(maxValueInt)) + 1;
            var minValueIntLength = minValueInt == 0 ? 1 : (int)Math.Log10((double)Math.Abs(minValueInt)) + 1;

            // NOTE: valueInt の桁数分と+1桁の値で比較する
            var maxPow = (decimal)Math.Truncate(Math.Pow(10, maxValueIntLength - valueIntLength));
            var maxPow2 = (decimal)Math.Truncate(Math.Pow(10, maxValueIntLength - valueIntLength - 1));
            var minPow = (decimal)Math.Truncate(Math.Pow(10, minValueIntLength - valueIntLength));
            var minPow2 = (decimal)Math.Truncate(Math.Pow(10, minValueIntLength - valueIntLength - 1));

            var compareMaxValue = maxValueInt;
            var compareNextMaxValue = maxValueInt;
            if (maxPow > 0)
            {
                compareMaxValue = Math.Truncate(compareMaxValue / maxPow);
            }
            if (maxPow2 > 0)
            {
                compareNextMaxValue = Math.Truncate(compareNextMaxValue / maxPow2);
            }

            var compareMinValue = minValueInt;
            var compareNextMinValue = minValueInt;
            if (minPow > 0)
            {
                compareMinValue = Math.Truncate(compareMinValue / minPow);
            }
            if (minPow2 > 0)
            {
                compareNextMinValue = Math.Truncate(compareNextMinValue / minPow2);
            }

            // MaxValue 整数部の範囲外
            if (valueInt > compareMaxValue && valueInt > compareNextMaxValue)
            {
                return false;
            }

            // MinValue 整数部の範囲外
            if (valueInt < compareMinValue && valueInt < compareNextMinValue)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 小数部が範囲内かどうかを取得する。
        /// </summary>
        /// <param name="value">入力値。</param>
        /// <returns>true:範囲内, false:範囲外。</returns>
        private bool hasRangeDecimalDigitsValue(decimal value)
        {
            // 小数部の桁数を求める
            var valueDecimalDigitsLength = getDecimalDigitsLength(value);
            var maxDecimalDigitsLength = getDecimalDigitsLength(MaxValue);
            var minDecimalDigitsLength = getDecimalDigitsLength(MinValue);

            // value の小数値より1桁多い比較を行う小数桁数を求める
            var nextMaxDecimalDigitsLength = valueDecimalDigitsLength + 1 > maxDecimalDigitsLength ? maxDecimalDigitsLength : valueDecimalDigitsLength + 1;
            var nextMinDecimalDigitsLength = valueDecimalDigitsLength + 1 > minDecimalDigitsLength ? minDecimalDigitsLength : valueDecimalDigitsLength + 1;

            // 比較用の値を求める
            var compareMaxValue = MetMath.Truncate(MaxValue, valueDecimalDigitsLength);
            var compareNextMaxValue = MetMath.Truncate(MaxValue, nextMaxDecimalDigitsLength);
            var compareMinValue = MetMath.Truncate(MinValue, valueDecimalDigitsLength);
            var compareNextMinValue = MetMath.Truncate(MinValue, nextMinDecimalDigitsLength);

            // MaxValue 小数部の範囲外
            if (value > compareMaxValue && value > compareNextMaxValue)
            {
                return false;
            }

            // MinValue 小数部の範囲外
            if (value < compareMinValue && value < compareNextMinValue)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
