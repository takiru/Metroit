using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// <see cref="CheckBox"/> の状態が変更されるときのイベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="sender">イベントのソース。</param>
    /// <param name="e">イベントデータ。</param>
    public delegate void CheckStateChangingEventHandler(object sender, CheckStateChangingEventArgs e);
}
