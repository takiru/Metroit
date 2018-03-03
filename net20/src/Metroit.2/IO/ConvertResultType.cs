namespace Metroit.IO
{
    /// <summary>
    /// 変換結果を定義します。
    /// </summary>
    public enum ConvertResultType
    {
        /// <summary>
        /// 変換の成功を示します。
        /// </summary>
        Succeed,
        /// <summary>
        /// 変換の失敗を示します。
        /// </summary>
        Failed,
        /// <summary>
        /// 変換のキャンセルを示します。
        /// </summary>
        Cancelled
    }
}
