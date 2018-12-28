using System;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 候補が選択された時のデータを提供します。
    /// </summary>
    public class CandidateSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// 新しい CandidateSelectedEventArgs インスタンスを生成します。
        /// </summary>
        /// <param name="selectedItem">選択アイテムオブジェクト。</param>
        /// <param name="selectedValue">選択値オブジェクト。</param>
        /// <param name="selectedText">選択値テキスト。</param>
        /// <param name="selectedIndex">選択値インデックス。</param>
        public CandidateSelectedEventArgs(object selectedItem, object selectedValue, string selectedText, int selectedIndex)
        {
            this.SelectedItem = selectedItem;
            this.SelectedValue = selectedValue;
            this.SelectedText = selectedText;
            this.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// 選択したアイテムを取得します。
        /// </summary>
        public object SelectedItem { get; }

        /// <summary>
        /// 選択したアイテムの実際の値を取得します。
        /// </summary>
        public object SelectedValue { get; }

        /// <summary>
        /// 選択したアイテムの入力値を取得します。
        /// </summary>
        public string SelectedText { get; }

        /// <summary>
        /// 選択したアイテムのインデックスを取得します。
        /// </summary>
        public int SelectedIndex { get; }
    }
}
