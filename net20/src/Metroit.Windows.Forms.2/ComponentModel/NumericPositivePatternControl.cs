using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Metroit.Windows.Forms.ComponentModel
{
    /// <summary>
    /// プラス数値の記号表現を設定するためのユーザーコントロールを提供します。
    /// </summary>
    [ToolboxItem(false)]
    public partial class NumericPositivePatternControl : UserControl
    {
        /// <summary>
        /// NumericPositivePatternControl クラスの新しいインスタンスを初期化します。
        /// </summary>
        internal NumericPositivePatternControl()
        {
            InitializeComponent();

            // 文字をローカライズ
            CurrencyPatternLabel.Text = DesignResources.GetString("CurrencyPattern");
            PercentPatternLabel.Text = DesignResources.GetString("PercentPattern");

            setCurrencyList();
            setPercentList();
        }

        /// <summary>
        /// NumericPositivePatternControl クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="npp">NumericPositivePattern オブジェクト。</param>
        public NumericPositivePatternControl(NumericPositivePattern npp)
            : this()
        {
            NumericPositivePattern = npp;
            comboCPattern.SelectedValue = npp.CurrencyPositivePattern;
            comboPPattern.SelectedValue = npp.PercentPositivePattern;
        }

        /// <summary>
        /// プラス数値の記号表現を取得または設定します。
        /// </summary>
        public NumericPositivePattern NumericPositivePattern { get; set; } = new NumericPositivePattern();

        /// <summary>
        /// 通貨時のプラス記号表現を決定する。
        /// </summary>
        /// <param name="sender">object オブジェクト。</param>
        /// <param name="e">EventArgs オブジェクト。</param>
        private void comboCPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumericPositivePattern.CurrencyPositivePattern = ((KeyValuePair<string,
                    CurrencyPositivePatternType>)comboCPattern.SelectedItem).Value;
        }

        /// <summary>
        /// パーセント時のプラス記号表現を決定する。
        /// </summary>
        /// <param name="sender">object オブジェクト。</param>
        /// <param name="e">EventArgs オブジェクト。</param>
        private void comboPPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumericPositivePattern.PercentPositivePattern = ((KeyValuePair<string, 
                    PercentPositivePatternType>)comboPPattern.SelectedItem).Value;
        }

        /// <summary>
        /// 通貨時のプラス記号表現リストを生成する。
        /// </summary>
        private void setCurrencyList()
        {
            List<KeyValuePair<string, CurrencyPositivePatternType>> list = new List<KeyValuePair<string, CurrencyPositivePatternType>>();
            list.Add(new KeyValuePair<string, CurrencyPositivePatternType>("$n", CurrencyPositivePatternType.LeftSymbol));
            list.Add(new KeyValuePair<string, CurrencyPositivePatternType>("n$", CurrencyPositivePatternType.RightSymbol));
            list.Add(new KeyValuePair<string, CurrencyPositivePatternType>("$ n", CurrencyPositivePatternType.LeftSymbolSpace));
            list.Add(new KeyValuePair<string, CurrencyPositivePatternType>("n $", CurrencyPositivePatternType.RightSpaceSymbol));

            comboCPattern.DisplayMember = "key";
            comboCPattern.ValueMember = "value";
            comboCPattern.DataSource = list;
        }

        /// <summary>
        /// パーセント時のプラス記号表現リストを生成する。
        /// </summary>
        private void setPercentList()
        {
            List<KeyValuePair<string, PercentPositivePatternType>> list = new List<KeyValuePair<string, PercentPositivePatternType>>();
            list.Add(new KeyValuePair<string, PercentPositivePatternType>("n %", PercentPositivePatternType.RightSpaceSymbol));
            list.Add(new KeyValuePair<string, PercentPositivePatternType>("n%", PercentPositivePatternType.RightSymbol));
            list.Add(new KeyValuePair<string, PercentPositivePatternType>("%n", PercentPositivePatternType.LeftSymbol));

            comboPPattern.DisplayMember = "key";
            comboPPattern.ValueMember = "value";
            comboPPattern.DataSource = list;
        }
    }
}
