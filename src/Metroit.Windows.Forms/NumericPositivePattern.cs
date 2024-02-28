using System.ComponentModel;
using System.Text;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 数値コントロールのプラス時の記号表現を扱います。
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class NumericPositivePattern
    {
        /// <summary>
        /// パターンが変更された時に実行されます。
        /// </summary>
        public event System.EventHandler PatternChanged;

        private CurrencyPositivePatternType currencyPositivePatternType = CurrencyPositivePatternType.LeftSymbol;
        private PercentPositivePatternType percentPositivePatternType = PercentPositivePatternType.RightSymbol;

        /// <summary>
        /// 通貨プラス時の記号表現を指定します。
        /// </summary>
        [Browsable(true)]
        [Description("通貨プラス時の記号表現を指定します。")]
        [DefaultValue(CurrencyPositivePatternType.LeftSymbol)]
        public CurrencyPositivePatternType CurrencyPositivePattern
        {
            get => currencyPositivePatternType;
            set
            {
                currencyPositivePatternType = value;
                OnPatternChanged();
            }
        }

        /// <summary>
        /// パーセントプラス時の記号表現を指定します。
        /// </summary>
        [Browsable(true)]
        [Description("パーセントプラス時の記号表現を指定します。")]
        [DefaultValue(PercentPositivePatternType.RightSymbol)]
        public PercentPositivePatternType PercentPositivePattern
        {
            get => percentPositivePatternType;
            set
            {
                percentPositivePatternType = value;
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
            propertyInfo.Append(CurrencyPositivePattern.ToString());
            propertyInfo.Append(",");
            propertyInfo.Append(PercentPositivePattern.ToString());

            return propertyInfo.ToString();
        }
    }
}
