using System;

namespace Metroit
{
    /// <summary>
    /// <see cref="IStateObject.StateChanged"/> イベントのデータを提供します。
    /// </summary>
    public class StateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// データの状態を取得します。
        /// </summary>
        public ItemState State { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="state">データの状態。</param>
        public StateChangedEventArgs(ItemState state)
        {
            State = state;
        }
    }
}
