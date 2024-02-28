using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Metroit.Windows.Forms.ComponentModel
{
    /// <summary>
    /// マイナス数値の記号表現を設定するためのユーザーコントロールを提供します。
    /// </summary>
    [ToolboxItem(false)]
    public partial class NumericNegativePatternControl : UserControl
    {
        /// <summary>
        /// NumericNegativePatternControl クラスの新しいインスタンスを初期化します。
        /// </summary>
        internal NumericNegativePatternControl()
        {
            InitializeComponent();

            // 文字をローカライズ
            NumericPatternLabel.Text = DesignResources.GetString("NumericPattern");
            CurrencyPatternLabel.Text = DesignResources.GetString("CurrencyPattern");
            PercentPatternLabel.Text = DesignResources.GetString("PercentPattern");
        }

        /// <summary>
        /// NumericNegativePatternControl クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="nnp">NumericNegativePattern オブジェクト。</param>
        public NumericNegativePatternControl(NumericNegativePattern nnp)
            : this()
        {
            NumericNegativePattern = nnp;

            // コンボボックスのリストを設定するとNumericNegativePatternオブジェクト内の設定値も
            // 変わってしまうため、表示直後のパターンを保持して、リスト設定後に差し替える
            var currentNumberPattern = nnp.NumberNegativePattern;
            var currentCurrencyPattern = nnp.CurrencyNegativePattern;
            var currentPpercentPattern = nnp.PercentNegativePattern;

            setNumberList();
            setCurrencyList();
            setPercentList();

            comboNPattern.SelectedValue = currentNumberPattern;
            comboCPattern.SelectedValue = currentCurrencyPattern;
            comboPPattern.SelectedValue = currentPpercentPattern;
        }

        /// <summary>
        /// マイナス数値の記号表現を取得または設定します。
        /// </summary>
        public NumericNegativePattern NumericNegativePattern { get; set; }

        /// <summary>
        /// 数値時のマイナス記号表現を決定する。
        /// </summary>
        /// <param name="sender">object オブジェクト。</param>
        /// <param name="e">EventArgs オブジェクト。</param>
        private void comboNPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumericNegativePattern.NumberNegativePattern = (NumericNegativePatternType)comboNPattern.SelectedValue;
        }

        /// <summary>
        /// 通貨時のマイナス記号表現を決定する。
        /// </summary>
        /// <param name="sender">object オブジェクト。</param>
        /// <param name="e">EventArgs オブジェクト。</param>
        private void comboCPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumericNegativePattern.CurrencyNegativePattern = (CurrencyNegativePatternType)comboCPattern.SelectedValue;
        }

        /// <summary>
        /// パーセント時のマイナス記号表現を決定する。
        /// </summary>
        /// <param name="sender">object オブジェクト。</param>
        /// <param name="e">EventArgs オブジェクト。</param>
        private void comboPPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumericNegativePattern.PercentNegativePattern = (PercentNegativePatternType)comboPPattern.SelectedValue;
        }

        /// <summary>
        /// 数値時のマイナス記号表現リストを生成する。
        /// </summary>
        private void setNumberList()
        {
            List<KeyValuePair<string, NumericNegativePatternType>> list = new List<KeyValuePair<string, NumericNegativePatternType>>();
            list.Add(new KeyValuePair<string, NumericNegativePatternType>("(n)", NumericNegativePatternType.Inclose));
            list.Add(new KeyValuePair<string, NumericNegativePatternType>("-n", NumericNegativePatternType.LeftSign));
            list.Add(new KeyValuePair<string, NumericNegativePatternType>("- n", NumericNegativePatternType.LeftSignSpace));
            list.Add(new KeyValuePair<string, NumericNegativePatternType>("n-", NumericNegativePatternType.RightSign));
            list.Add(new KeyValuePair<string, NumericNegativePatternType>("n -", NumericNegativePatternType.RightSpaceSign));

            comboNPattern.DisplayMember = "key";
            comboNPattern.ValueMember = "value";
            comboNPattern.DataSource = list;
        }

        /// <summary>
        /// 通貨時のマイナス記号表現リストを生成する。
        /// </summary>
        private void setCurrencyList()
        {
            List<KeyValuePair<string, CurrencyNegativePatternType>> list = new List<KeyValuePair<string, CurrencyNegativePatternType>>();
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("($n)", CurrencyNegativePatternType.IncloseLeftSymbol));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("-$n", CurrencyNegativePatternType.LeftSignSymbol));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("$-n", CurrencyNegativePatternType.LeftSymbolSign));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("$n-", CurrencyNegativePatternType.LeftSymbolRightSign));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("(n$)", CurrencyNegativePatternType.IncloseRightSymbol));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("-n$", CurrencyNegativePatternType.LeftSignRightSymbol));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("n-$", CurrencyNegativePatternType.RightSignSymbol));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("n$-", CurrencyNegativePatternType.RightSymbolSign));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("-n $", CurrencyNegativePatternType.LeftSignRightSpaceSymbol));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("-$ n", CurrencyNegativePatternType.LeftSignSymbolSpace));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("n $-", CurrencyNegativePatternType.RightSpaceSymbolSign));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("$ n-", CurrencyNegativePatternType.LeftSymbolSpaceRightSign));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("$ -n", CurrencyNegativePatternType.LeftSymbolSpaceSign));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("n- $", CurrencyNegativePatternType.RightSignSpaceSymbol));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("($ n)", CurrencyNegativePatternType.IncloseLeftSymbolSpace));
            list.Add(new KeyValuePair<string, CurrencyNegativePatternType>("(n $)", CurrencyNegativePatternType.IncloseRightSpaceSymbol));

            comboCPattern.DisplayMember = "key";
            comboCPattern.ValueMember = "value";
            comboCPattern.DataSource = list;
        }

        /// <summary>
        /// パーセント時のマイナス記号表現リストを生成する。
        /// </summary>
        private void setPercentList()
        {
            List<KeyValuePair<string, PercentNegativePatternType>> list = new List<KeyValuePair<string, PercentNegativePatternType>>();
            list.Add(new KeyValuePair<string, PercentNegativePatternType>("-n %", PercentNegativePatternType.LeftSignRightSpaceSymbol));
            list.Add(new KeyValuePair<string, PercentNegativePatternType>("-n%", PercentNegativePatternType.LeftSignRightSymbol));
            list.Add(new KeyValuePair<string, PercentNegativePatternType>("-%n", PercentNegativePatternType.LeftSignSymbol));
            list.Add(new KeyValuePair<string, PercentNegativePatternType>("%-n", PercentNegativePatternType.LeftSymbolSign));
            list.Add(new KeyValuePair<string, PercentNegativePatternType>("%n-", PercentNegativePatternType.LeftSymbolRightSign));
            list.Add(new KeyValuePair<string, PercentNegativePatternType>("n-%", PercentNegativePatternType.RightSignSymbol));
            list.Add(new KeyValuePair<string, PercentNegativePatternType>("n%-", PercentNegativePatternType.RightSymbolSign));
            list.Add(new KeyValuePair<string, PercentNegativePatternType>("n %-", PercentNegativePatternType.RightSpaceSymbolSign));
            list.Add(new KeyValuePair<string, PercentNegativePatternType>("% n-", PercentNegativePatternType.LeftSymbolSpaceRightSign));
            list.Add(new KeyValuePair<string, PercentNegativePatternType>("% -n", PercentNegativePatternType.LeftSymbolSpaceSign));
            list.Add(new KeyValuePair<string, PercentNegativePatternType>("n- %", PercentNegativePatternType.RightSignSpaceSymbol));

            comboPPattern.DisplayMember = "key";
            comboPPattern.ValueMember = "value";
            comboPPattern.DataSource = list;
        }
    }
}
