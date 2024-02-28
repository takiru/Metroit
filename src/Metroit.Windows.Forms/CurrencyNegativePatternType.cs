namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 通貨のマイナス値の表現パターンを定義します。
    /// </summary>
    public enum CurrencyNegativePatternType
    {
        /// <summary>
        /// ($n) を示します。
        /// </summary>
        IncloseLeftSymbol = 0,
        /// <summary>
        /// -$n を示します。
        /// </summary>
        LeftSignSymbol = 1,
        /// <summary>
        /// $-n を示します。
        /// </summary>
        LeftSymbolSign = 2,
        /// <summary>
        /// $n- を示します。
        /// </summary>
        LeftSymbolRightSign = 3,
        /// <summary>
        /// (n$) を示します。
        /// </summary>
        IncloseRightSymbol = 4,
        /// <summary>
        /// -n$ を示します。
        /// </summary>
        LeftSignRightSymbol = 5,
        /// <summary>
        /// n-$ を示します。
        /// </summary>
        RightSignSymbol = 6,
        /// <summary>
        /// n$- を示します。
        /// </summary>
        RightSymbolSign = 7,
        /// <summary>
        /// -n $ を示します。
        /// </summary>
        LeftSignRightSpaceSymbol = 8,
        /// <summary>
        /// -$ n を示します。
        /// </summary>
        LeftSignSymbolSpace = 9,
        /// <summary>
        /// n $- を示します。
        /// </summary>
        RightSpaceSymbolSign = 10,
        /// <summary>
        /// $ n- を示します。
        /// </summary>
        LeftSymbolSpaceRightSign = 11,
        /// <summary>
        /// $ -n を示します。
        /// </summary>
        LeftSymbolSpaceSign = 12,
        /// <summary>
        /// n- $ を示します。
        /// </summary>
        RightSignSpaceSymbol = 13,
        /// <summary>
        /// ($ n) を示します。
        /// </summary>
        IncloseLeftSymbolSpace = 14,
        /// <summary>
        /// (n $) を示します。
        /// </summary>
        IncloseRightSpaceSymbol = 15
    }
}
