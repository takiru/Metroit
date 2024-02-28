using System;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 開閉が行われた時のイベントパラメーターを提供します。
    /// </summary>
    public class ExpandStateEventArgs : EventArgs
    {
        /// <summary>
        /// 開閉ステータスを取得します。
        /// </summary>
        public ExpandState State { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="state">開閉ステータス。</param>
        public ExpandStateEventArgs(ExpandState state)
        {
            State = state;
        }
    }
}
