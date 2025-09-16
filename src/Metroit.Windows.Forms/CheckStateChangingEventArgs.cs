using System.ComponentModel;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// <see cref="CheckBox"/> の状態が変更されるときのイベントのイベントデータを提供します。
    /// </summary>
    public class CheckStateChangingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// 現在のチェック状態を取得します。
        /// </summary>
        public CheckState CurrentCheckState { get; }

        /// <summary>
        /// 処理を継続したときのチェック状態を取得します。
        /// </summary>
        public CheckState NextChackState { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="cancel">既定値をキャンセルとするかどうか。</param>
        /// <param name="currentCheckState">現在のチェック状態。</param>
        /// <param name="nextCheckState">処理を継続したときのチェック状態。</param>
        public CheckStateChangingEventArgs(bool cancel, CheckState currentCheckState, CheckState nextCheckState)
            : base(cancel)
        {
            CurrentCheckState = currentCheckState;
            NextChackState = nextCheckState;
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="currentCheckState">現在のチェック状態。</param>
        /// <param name="nextCheckState">処理を継続したときのチェック状態。</param>
        public CheckStateChangingEventArgs(CheckState currentCheckState, CheckState nextCheckState)
            : base()
        {
            CurrentCheckState = currentCheckState;
            NextChackState = nextCheckState;
        }
    }
}
