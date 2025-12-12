using Metroit.Annotations;
using Metroit.ChangeTracking.Generic;

namespace Metroit.ChangeTracking
{
    /// <summary>
    /// 状態管理機能を備えた変更追跡が可能なオブジェクトを提供します。
    /// </summary>
    /// <typeparam name="T1">変更追跡対象となるオブジェクト。</typeparam>
    /// <typeparam name="T2">トラッカーオブジェクト。</typeparam>
    public class StatefulTrackingObject<T1, T2> : TrackingObject<T1, T2>, IStateObject where T1 : class where T2 : PropertyChangeTracker<T1>, new()
    {
        private ItemState _state = ItemState.New;

        /// <summary>
        /// 現在の状態を取得します。
        /// </summary>
        [NoTracking]
        public ItemState State => _state;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public StatefulTrackingObject() : base() { }

        /// <summary>
        /// 状態を変更します。
        /// </summary>
        /// <param name="state">状態。</param>
        public void ChangeState(ItemState state)
        {
            _state = state;
        }
    }
}
