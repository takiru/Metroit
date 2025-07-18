using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Metroit.Collections.Generic
{
    /// <summary>
    /// 削除されたアイテムを把握可能なリストを提供します。
    /// </summary>
    /// <typeparam name="T">把握可能とするクラス。</typeparam>
    public class ItemRemovedKnownList<T> : BindingList<T>, IItemRemovedKnownList<T> where T : IStateObject
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
        /// 外部からの利用は不要です。
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        IList IItemRemovedKnownList.Removed => (IList)_removed;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public ItemRemovedKnownList()
        {
            AddingNew += ItemRemovedKnownList_AddingNew;
            ListChanged += ItemRemovedKnownList_ListChanged;
        }

        /// <summary>
        /// 削除指示を受けたアイテム。
        /// </summary>
        private T _removingItem;

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
        private void ItemRemovedKnownList_AddingNew(object sender, AddingNewEventArgs e)
        {
            _isAddingNew = true;
        }

        /// <summary>
        /// リセットされたら削除リストをクリア、リストが削除されたら削除されたオブジェクトを削除リストに追加する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemRemovedKnownList_ListChanged(object sender, ListChangedEventArgs e)
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
                    if (e.OldIndex == -1)
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
                return;
            }

            // Add(), Insert() による行追加したとき
            this[index].ChangeState(ItemState.NotModified);
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
            }

            // 無変更行の値を変更したとき
            if (item.State == ItemState.NotModified)
            {
                item.ChangeState(ItemState.Modified);
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
                return;
            }

            if (_removingItem.State == ItemState.NewModified)
            {
                return;
            }

            _removed.Add(_removingItem);
        }
    }
}
