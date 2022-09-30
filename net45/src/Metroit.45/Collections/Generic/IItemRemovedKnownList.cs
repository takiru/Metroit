using System.Collections.Generic;

namespace Metroit.Collections.Generic
{
    /// <summary>
    /// 削除されたアイテムを把握可能なインターフェースを提供します。
    /// </summary>
    /// <typeparam name="T">StateKnownItemBase を有する型。</typeparam>
    public interface IItemRemovedKnownList<T> : IItemRemovedKnownList where T : StateKnownItemBase
    {
        /// <summary>
        /// 削除されたリストデータ を取得します。
        /// </summary>
        new IList<T> Removed { get; }
    }
}
