using Metroit.Annotations;
using Metroit.ChangeTracking.Generic;
using System.ComponentModel;

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
            if (_state == state)
            {
                return;
            }
            _state = state;
            OnStateChanged(new StateChangedEventArgs(_state));
        }

        /// <summary>
        /// データの状態に変更があったときに発生します。
        /// </summary>
        public event StateChangedEventHandler StateChanged;

        /// <summary>
        /// <see cref="StateChanged"/> イベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStateChanged(StateChangedEventArgs e)
        {
            StateChanged?.Invoke(this, e);
        }

        /// <summary>
        /// 変更通知が行われたプロパティまたはフィールドを追跡する。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            ChangeStateOnPropertyChanged();
            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// プロパティ変更時に状態を変更します。
        /// </summary>
        private void ChangeStateOnPropertyChanged()
        {
            // 新規オブジェクトの値を変更したとき
            if (State == ItemState.New)
            {
                ChangeState(ItemState.NewModified);
                return;
            }

            // 無変更オブジェクトの値を変更したとき
            if (State == ItemState.NotModified)
            {
                ChangeState(ItemState.Modified);
                return;
            }

            // 新規オブジェクトの値を編集して元の値に戻ったとき
            if (State == ItemState.NewModified)
            {
                if (!ChangeTracker.IsSomethingValueChanged)
                {
                    ChangeState(ItemState.New);
                }
                return;
            }

            // 無変更オブジェクトの値を編集して元の値に戻ったとき
            if (State == ItemState.Modified)
            {
                if (!ChangeTracker.IsSomethingValueChanged)
                {
                    ChangeState(ItemState.NotModified);
                }
                return;
            }
        }
    }
}
