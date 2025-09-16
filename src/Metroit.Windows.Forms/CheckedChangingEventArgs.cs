using System.ComponentModel;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// Checked の状態が変更されるときのイベントのイベントデータを提供します。
    /// </summary>
    public class CheckedChangingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// 現在のチェック状態を取得します。
        /// </summary>
        public bool CurrentChecked { get; }

        /// <summary>
        /// 処理を継続したときのチェック状態を取得します。
        /// </summary>
        public bool NextChacked { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="cancel">既定値をキャンセルとするかどうか。</param>
        /// <param name="currentChecked">現在のチェック状態。</param>
        /// <param name="nextChecked">処理を継続したときのチェック状態。</param>
        public CheckedChangingEventArgs(bool cancel, bool currentChecked, bool nextChecked)
            : base(cancel)
        {
            CurrentChecked = currentChecked;
            NextChacked = nextChecked;
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="currentChecked">現在のチェック状態。</param>
        /// <param name="nextChecked">処理を継続したときのチェック状態。</param>
        public CheckedChangingEventArgs(bool currentChecked, bool nextChecked)
            : base()
        {
            CurrentChecked = currentChecked;
            NextChacked = nextChecked;
        }
    }
}
