namespace Metroit.Windows.Forms
{
    /// <summary>
    /// パーセントのプラス値の表現パターンを定義します。
    /// </summary>
    /// <remarks>3の表現%nが2の表現と一緒なので、列挙体には定義無し</remarks>
    public enum PercentPositivePatternType
    {
        /// <summary>
        /// n % を示します。
        /// </summary>
        RightSpaceSymbol = 0,
        /// <summary>
        /// n% を示します。
        /// </summary>
        RightSymbol = 1,
        /// <summary>
        /// %n を示します。
        /// </summary>
        LeftSymbol = 2
    }
}
