using Metroit.Windows.Forms.ComponentModel;
using Metroit.Windows.Forms.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// ユーザーが数値のみを出力できるようにしたラベルエリアを提供します。
    /// </summary>
    /// <remarks>
    /// ラベルとしての機能をいくつか拡張し、利用することができます。<br />
    /// [拡張機能]<br />
    /// 　・入力可能範囲の設定。<br />
    /// 　・null許可制御。<br />
    /// 　・入力値の表示フォーマット設定。<br />
    /// 　・マイナス値の文字色設定。<br />
    /// </remarks>
    [ToolboxItem(true)]
    public class MetNumericLabel : MetLabel
    {
        /// <summary>
        /// MetNumericLabel の新しいインスタンスを初期化します。
        /// </summary>
        public MetNumericLabel() : base()
        {
            TextAlign = ContentAlignment.TopRight;
            base.Text = Value.Value.ToString();
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

        #region イベント

        /// <summary>
        /// マイナス記号表現が変更された時、表示を再描画する。
        /// </summary>
        /// <param name="sender">object オブジェクト。</param>
        /// <param name="e">EventArgs オブジェクト。</param>
        private void NegativePattern_PatternChanged(object sender, EventArgs e)
        {
            if (this.Value.HasValue && this.value < 0)
            {
                base.Text = this.createFormattedText(this.value);
            }
            this.ChangeDisplayColor();
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
                base.Text = this.createFormattedText(this.value);
            }
            this.ChangeDisplayColor();
        }

        #endregion

        #region 追加イベント

        /// <summary>
        /// Value プロパティの値が変更されているときに発生するイベントです。値を検証し、変更を拒否することができます。
        /// </summary>
        [MetCategory("MetPropertyChange")]
        [MetDescription("ValueChangeValidating")]
        public event ValueChangeValidatingEventHandler ValueChangeValidating;

        /// <summary>
        /// 値が変更された時に発生します。
        /// </summary>
        [MetCategory("MetPropertyChange")]
        [MetDescription("ValueChanged")]
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

        // FIXED: 例えば特定条件下で色を変えるなどのユーザー実装が行えなくなるため、コード実装は可能とする。
        /// <summary>
        /// コントロールの前景色を取得または特定のプロパティによって強制的に前景色を設定します。
        /// 意図的に前景色の変更が必要な場合のみ利用してください。
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        #endregion

        #region 追加プロパティ

        private const int MAX_DIGITS = 10;
        private const string DECIMAL_SEPARATOR = @".";
        private const string GROUP_SEPARATOR = @",";
        private const string NEGATIVE_SIGN = @"-";
        private const string CURRENCY_SYMBOL = @"\";
        private const string PERCENT_SYMBOL = @"%";

        private decimal? value = 0;
        private NumericFormatMode mode = NumericFormatMode.Numeric;
        private Color positiveForeColor = SystemColors.ControlText;
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
            get => this.value;
            set
            {
                // 設定値に変更がない場合は処理しない
                if (this.value == value)
                {
                    return;
                }

                // null を許容していない場合はNG
                if (!AcceptNull && !value.HasValue)
                {
                    throw new ArgumentNullException(nameof(Value));
                }

                // 入力値チェックイベント
                var e = new ValueChangeValidatingEventArgs()
                {
                    Cancel = false,
                    Before = this.value,
                    Input = value,
                    After = value
                };
                this.OnValueChangeValidating(e);
                if (e.Cancel)
                {
                    throw new DeniedTextException(
                            ExceptionResources.GetString("DeniedTextException"), value.ToString());
                }

                this.value = value;
                base.Text = this.createFormattedText(value);
                OnValueChanged(new EventArgs());
                this.ChangeDisplayColor();
            }
        }

        /// <summary>
        /// 表示される形式を取得または設定します。
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
                base.Text = this.createFormattedText(this.value);
            }
        }

        /// <summary>
        /// マイナス時の文字色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [DefaultValue(typeof(Color), "ControlText")]
        [MetDescription("ControlPositiveForeColor")]
        public Color PositiveForeColor
        {
            get { return this.positiveForeColor; }
            set
            {
                this.positiveForeColor = value;
                if (this.FindForm() != null)
                {
                    this.ChangeDisplayColor();
                }
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
            get => this.acceptNegative;
            set
            {
                // マイナス値を許可しない時にValue値が負数の場合はNG
                if (!value && this.value.HasValue && this.value < 0)
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
                if (this.value.HasValue && this.value > value && FindForm() != null && !this.IsDesignMode())
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
                if (this.value.HasValue && this.value < value && FindForm() != null && !this.IsDesignMode())
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
                    base.Text = this.createFormattedText(this.value);
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
                    base.Text = this.createFormattedText(this.value);
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
                    base.Text = this.createFormattedText(this.value);
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
                    base.Text = this.createFormattedText(this.value);
                }
            }
        }

        private bool initializedPositivePattern = false;

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
                if (!initializedPositivePattern)
                {
                    positivePattern.PatternChanged += PositivePattern_PatternChanged;
                    initializedPositivePattern = true;
                }

                base.Text = this.createFormattedText(this.value);
            }
        }

        private bool initializedNegativePattern = false;

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
                if (!initializedNegativePattern)
                {
                    negativePattern.PatternChanged += NegativePattern_PatternChanged;
                    initializedNegativePattern = true;
                }

                base.Text = this.createFormattedText(this.value);
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
                    base.Text = this.createFormattedText(this.value);
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
                    base.Text = this.createFormattedText(this.value);
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
                    base.Text = this.createFormattedText(this.value);
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
        /// テキスト変更中、許可文字チェックを行います。
        /// </summary>
        /// <param name="e">入力値過程オブジェクト</param>
        protected void OnValueChangeValidating(ValueChangeValidatingEventArgs e)
        {
            // AcceptNull中は許可する
            if (this.AcceptNull && !e.After.HasValue)
            {
                return;
            }

            // 形式チェック
            if (!this.isValidNegative(e.After.Value))
            {
                e.Cancel = true;
                return;
            }
            if (!this.isValidDecimalDigits(e.After.Value))
            {
                e.Cancel = true;
                return;
            }
            if (!this.isValidRangeValue(e.After.Value))
            {
                e.Cancel = true;
                return;
            }

            if (this.ValueChangeValidating != null)
            {
                this.ValueChangeValidating(this, e);
            }
        }

        /// <summary>
        /// コントロールのアクティブ状態に応じた表示色に変更する。
        /// </summary>
        protected void ChangeDisplayColor()
        {
            // 0またはプラス値
            if (this.value.HasValue && this.value >= 0)
            {
                base.ForeColor = this.PositiveForeColor;
                return;
            }

            // マイナス値
            if (this.value.HasValue && this.value < 0)
            {
                base.ForeColor = this.NegativeForeColor;
                return;
            }
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
