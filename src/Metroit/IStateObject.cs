namespace Metroit
{
    /// <summary>
    /// 状態を持つインターフェースを提供します。
    /// </summary>
    public interface IStateObject
    {
        /// <summary>
        /// 現在の状態を取得します。
        /// </summary>
        ItemState State { get; }

        /// <summary>
        /// 状態を変更します。
        /// </summary>
        /// <param name="state">変更する状態。</param>
        void ChangeState(ItemState state);

        /// <summary>
        /// データの状態に変更があったときに発生します。
        /// </summary>
        event StateChangedEventHandler StateChanged;
    }
}
