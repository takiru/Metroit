namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 通貨のプラス値の表現パターンを定義します。
    /// </summary>
    public enum CurrencyPositivePatternType
    {
        /// <summary>
        /// $n を示します。
        /// </summary>
        LeftSymbol = 0,
        /// <summary>
        /// n$ を示します。
        /// </summary>
        RightSymbol = 1,
        /// <summary>
        /// $ n を示します。
        /// </summary>
        LeftSymbolSpace = 2,
        /// <summary>
        /// n $ を示します。
        /// </summary>
        RightSpaceSymbol = 3
    }
}
