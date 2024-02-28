namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 数値のマイナス値の表現パターンを定義します。
    /// </summary>
    public enum NumericNegativePatternType
    {
        /// <summary>
        /// (n) を示します。
        /// </summary>
        Inclose = 0,
        /// <summary>
        /// -n を示します。
        /// </summary>
        LeftSign = 1,
        /// <summary>
        /// - n を示します。
        /// </summary>
        LeftSignSpace = 2,
        /// <summary>
        /// n- を示します。
        /// </summary>
        RightSign = 3,
        /// <summary>
        /// n - を示します。
        /// </summary>
        RightSpaceSign = 4
    }
}
