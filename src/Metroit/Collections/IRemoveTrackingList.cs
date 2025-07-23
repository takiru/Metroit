using System.Collections;
using System.ComponentModel;

namespace Metroit.Collections
{
    /// <summary>
    /// 削除されたアイテムを把握可能なインターフェースを提供します。
    /// </summary>
    public interface IRemoveTrackingList : IBindingList
    {
        /// <summary>
        /// 削除されたリストデータを取得します。
        /// </summary>
        IList Removed { get; }
    }
}
