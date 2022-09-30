using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Metroit.Collections.Generic
{
    /// <summary>
    /// 削除されたアイテムを把握可能なリストを提供します。
    /// </summary>
    /// <typeparam name="T">StateKnownItemBase を有する型。</typeparam>
    public sealed class ItemRemovedKnownList<T> : BindingList<T>, IItemRemovedKnownList<T> where T : StateKnownItemBase
    {
        /// <summary>
        /// 削除されたリストデータを取得します。
        /// </summary>
        public IList<T> Removed { get; } = new List<T>();

        /// <summary>
        /// 削除されたリストデータ を取得します。
        /// 外部からの利用は不要です。
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        IList IItemRemovedKnownList.Removed => (IList)Removed;

        /// <summary>
        /// 削除指示を受けたリストデータ。
        /// </summary>
        private T removedItem;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public ItemRemovedKnownList()
        {

        }

        /// <summary>
        /// リストが削除された時に必ず走行する。削除されたオブジェクトを把握する。
        /// </summary>
        /// <param name="index">インデックス。</param>
        protected sealed override void RemoveItem(int index)
        {
            removedItem = this[index];
            base.RemoveItem(index);
        }

        /// <summary>
        /// リセットされたら削除リストをクリア、リストが削除されたら削除されたオブジェクトを削除リストに追加する。
        /// </summary>
        /// <param name="e"></param>
        protected sealed override void OnListChanged(ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    Removed.Clear();
                    break;

                case ListChangedType.ItemDeleted:
                    if (removedItem.State == ItemState.NotModified || removedItem.State == ItemState.Modified)
                    {
                        Removed.Add(removedItem);
                    }
                    break;
            }

            base.OnListChanged(e);
        }
    }
}
