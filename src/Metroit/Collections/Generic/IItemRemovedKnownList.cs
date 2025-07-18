using System.Collections.Generic;

namespace Metroit.Collections.Generic
{
    /// <summary>
    /// 削除されたアイテムを把握可能なインターフェースを提供します。
    /// </summary>
    /// <typeparam name="T">把握可能とするクラス。</typeparam>
    public interface IItemRemovedKnownList<T> : IItemRemovedKnownList where T : IStateObject
    {
        /// <summary>
        /// 削除されたリストデータ を取得します。
        /// </summary>
        new IReadOnlyList<T> Removed { get; }
    }
}
