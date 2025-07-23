using Metroit.ChangeTracking;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Metroit.Collections.Generic
{
    /// <summary>
    /// アイテムの状態と削除されたアイテムを把握可能なリストを提供します。
    /// </summary>
    /// <typeparam name="T">把握可能にするクラス。</typeparam>
    public class TrackingList<T> : BindingList<T>, IRemoveTrackingList<T>, ITrackingItem<T> where T : IPropertyChangeTrackerProvider, IStateObject
    {
        /// <summary>
        /// 削除されたリストデータ。
        /// </summary>
        private List<T> _removed = new List<T>();

        /// <summary>
        /// 削除されたリストデータを取得します。
        /// </summary>
        public IReadOnlyList<T> Removed => _removed;

        /// <summary>
        /// 削除されたリストデータ を取得します。
        /// </summary>
        IList IRemoveTrackingList.Removed => (IList)_removed;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public TrackingList()
        {
            AddingNew += TrackingList_AddingNew;
            ListChanged += TrackingList_ListChanged;
        }

        /// <summary>
        /// 削除指示を受けたアイテム。
        /// </summary>
        private T _removingItem;

        /// <summary>
        /// 最後に指示を受けたアイテムを取得します。
        /// </summary>
        public T LastAccessItem { get; private set; } = default(T);

        /// <summary>
        /// リストが削除された時に必ず走行する。削除されたオブジェクトを把握する。
        /// </summary>
        /// <param name="index">インデックス。</param>
        protected override void RemoveItem(int index)
        {
            _removingItem = this[index];
            base.RemoveItem(index);
        }

        private bool _isAddingNew = false;

        /// <summary>
        /// 新たな行が追加されたとき。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void TrackingList_AddingNew(object sender, AddingNewEventArgs e)
        {
            _isAddingNew = true;
        }

        /// <summary>
        /// リセットされたら削除リストをクリア、リストが削除されたら削除されたオブジェクトを削除リストに追加する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackingList_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    // Add(), AddNew(), Insert() で走行する
                    ChangeStateAddedItem(e.NewIndex);
                    break;

                case ListChangedType.ItemChanged:
                    // ResetItem(), INotifyPropertyChangedによって値変更が通知されたときに走行する
                    // ResetItem() による制御は行わない。
                    // NOTE: ResetItem() で走行する場合、OldIndexは -1 になる。
                    if (WasResetItem(e))
                    {
                        return;
                    }
                    ChangeStateChangeItem(e.NewIndex);
                    break;

                case ListChangedType.Reset:
                    // Clear() で走行する
                    _removed.Clear();
                    break;

                case ListChangedType.ItemDeleted:
                    // Remove(), RemoveAt(), CancelNew() で走行する
                    AddRemoveItem();
                    break;
            }
        }

        /// <summary>
        /// 行が追加されたときの状態を変更する。
        /// </summary>
        /// <param name="index">追加行インデックス。</param>
        private void ChangeStateAddedItem(int index)
        {
            // AddNew() による新規行を追加したとき
            if (_isAddingNew)
            {
                this[index].ChangeState(ItemState.New);
                _isAddingNew = false;
                LastAccessItem = this[index];
                return;
            }

            // Add(), Insert() による行追加したとき
            this[index].ChangeState(ItemState.NotModified);
            LastAccessItem = this[index];
        }

        /// <summary>
        /// <see cref="BindingList{T}.ResetItem(int)"/> を行ったかどうかを取得します。
        /// </summary>
        /// <param name="e"><see cref="ListChangedEventArgs"/>。</param>
        /// <returns><see cref="BindingList{T}.ResetItem(int)"/> を行った場合は true, それ以外は false を返却します。</returns>
        protected virtual bool WasResetItem(ListChangedEventArgs e)
        {
            if (e.OldIndex == -1)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 行が変更されたときの状態を変更する。
        /// </summary>
        /// <param name="index">変更行インデックス。</param>
        private void ChangeStateChangeItem(int index)
        {
            var item = this[index];

            // 新規行の値を変更したとき
            if (item.State == ItemState.New)
            {
                item.ChangeState(ItemState.NewModified);
                LastAccessItem = this[index];
                return;
            }

            // 無変更行の値を変更したとき
            if (item.State == ItemState.NotModified)
            {
                item.ChangeState(ItemState.Modified);
                LastAccessItem = this[index];
                return;
            }

            // 新規行の値を編集して元の値に戻ったとき
            if (item.State == ItemState.NewModified)
            {
                if (item.ChangeTracker.Entries.Where(x => x.Changed).Count() == 0)
                {
                    item.ChangeState(ItemState.New);
                }
                return;
            }

            // 無変更行の値を編集して元の値に戻ったとき
            if (item.State == ItemState.Modified)
            {
                if (item.ChangeTracker.Entries.Where(x => x.Changed).Count() == 0)
                {
                    item.ChangeState(ItemState.NotModified);
                }
                return;
            }
        }

        /// <summary>
        /// 削除されたアイテムを把握する。
        /// New, NewModified の状態の行を削除したときは削除行として把握しない。
        /// </summary>
        private void AddRemoveItem()
        {
            if (_removingItem.State == ItemState.New)
            {
                LastAccessItem = _removingItem;
                return;
            }

            if (_removingItem.State == ItemState.NewModified)
            {
                LastAccessItem = _removingItem;
                return;
            }

            _removed.Add(_removingItem);
            LastAccessItem = _removingItem;
        }
    }
}
