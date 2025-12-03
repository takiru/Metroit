using Metroit.Annotations;
using Metroit.ChangeTracking.Generic;

namespace Metroit.ChangeTracking
{
    //public class StatefulTrackingObject<T> : TrackingObject<T>, IStateObject where T : class
    public class StatefulTrackingObject<T, T2> : TrackingObject<T, T2>, IStateObject where T : class where T2 : PropertyChangeTracker<T>, new()
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
