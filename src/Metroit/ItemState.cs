namespace Metroit
{
    /// <summary>
    /// データの状態を提供します。
    /// </summary>
    public enum ItemState
    {
        /// <summary>
        /// 新規行 を示します。
        /// </summary>
        New,

        /// <summary>
        /// 新規修正行 を示します。
        /// 新規行の値を変更した時にこのステータスになります。
        /// </summary>
        NewModified,

        /// <summary>
        /// 未修正行 を示します。
        /// </summary>
        NotModified,

        /// <summary>
        /// 修正行 を示します。
        /// 未修正行の値を変更した時にこのステータスになります。
        /// </summary>
        Modified
    }
}
