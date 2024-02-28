using System.ComponentModel;
using System.Text;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 数値コントロールのマイナス値の記号表現を提供します。
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class NumericNegativePattern
    {
        /// <summary>
        /// パターンが変更された時に実行されます。
        /// </summary>
        public event System.EventHandler PatternChanged;

        private NumericNegativePatternType numericNegativePatternType = NumericNegativePatternType.LeftSign;
        private CurrencyNegativePatternType currencyNegativePatternType = CurrencyNegativePatternType.LeftSymbolSign;
        private PercentNegativePatternType percentNegativePatternType = PercentNegativePatternType.LeftSignRightSymbol;

        /// <summary>
        /// 数値マイナス時の記号表現を指定します。
        /// </summary>
        [Browsable(true)]
        [Description("数値マイナス時の記号表現を指定します。")]
        [DefaultValue(NumericNegativePatternType.LeftSign)]
        public NumericNegativePatternType NumberNegativePattern
        {
            get => numericNegativePatternType;
            set
            {
                numericNegativePatternType = value;
                OnPatternChanged();
            }
        }

        /// <summary>
        /// 通貨マイナス時の記号表現を指定します。
        /// </summary>
        [Browsable(true)]
        [Description("通貨マイナス時の記号表現を指定します。")]
        [DefaultValue(CurrencyNegativePatternType.LeftSymbolSign)]
        public CurrencyNegativePatternType CurrencyNegativePattern
        {
            get => currencyNegativePatternType;
            set
            {
                currencyNegativePatternType = value;
                OnPatternChanged();
            }
        }

        /// <summary>
        /// パーセントマイナス時の記号表現を指定します。
        /// </summary>
        [Browsable(true)]
        [Description("パーセントマイナス時の記号表現を指定します。")]
        [DefaultValue(PercentNegativePatternType.LeftSignRightSymbol)]
        public PercentNegativePatternType PercentNegativePattern
        {
            get => percentNegativePatternType;
            set
            {
                percentNegativePatternType = value;
                OnPatternChanged();
            }
        }

        /// <summary>
        /// パターンが変更された時にイベントを実行します。
        /// </summary>
        protected void OnPatternChanged()
        {
            PatternChanged?.Invoke(this, new System.EventArgs());
        }

        /// <summary>
        /// プロパティエディタ上の見た目を変更します。
        /// </summary>
        /// <remarks>
        /// コードを書く上で、特に利用する場面はないでしょう。
        /// </remarks>
        /// <returns>設定値文字列</returns>
        public override string ToString()
        {
            StringBuilder propertyInfo = new StringBuilder();
            propertyInfo.Append(CurrencyNegativePattern.ToString());
            propertyInfo.Append(",");
            propertyInfo.Append(NumberNegativePattern.ToString());
            propertyInfo.Append(",");
            propertyInfo.Append(PercentNegativePattern.ToString());

            return propertyInfo.ToString();
        }
    }
}
