using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// パーセントのマイナス値の表現パターンを定義します。
    /// </summary>
    /// <remarks>7の表現-%nが2の表現と一緒なので、列挙体には定義無し。</remarks>
    public enum PercentNegativePatternType
    {
        /// <summary>
        /// -n % を示します。
        /// </summary>
        LeftSignRightSpaceSymbol = 0,
        /// <summary>
        /// -n% を示します。
        /// </summary>
        LeftSignRightSymbol = 1,
        /// <summary>
        /// -%n を示します。
        /// </summary>
        LeftSignSymbol = 2,
        /// <summary>
        /// %-n を示します。
        /// </summary>
        LeftSymbolSign = 3,
        /// <summary>
        /// %n- を示します。
        /// </summary>
        LeftSymbolRightSign = 4,
        /// <summary>
        /// n-% を示します。
        /// </summary>
        RightSignSymbol = 5,
        /// <summary>
        /// n%- を示します。
        /// </summary>
        RightSymbolSign = 6,
        /// <summary>
        /// n %- を示します。
        /// </summary>
        RightSpaceSymbolSign = 8,
        /// <summary>
        /// % n- を示します。
        /// </summary>
        LeftSymbolSpaceRightSign = 9,
        /// <summary>
        /// % -n を示します。
        /// </summary>
        LeftSymbolSpaceSign = 10,
        /// <summary>
        /// n- % を示します。
        /// </summary>
        RightSignSpaceSymbol = 11
    }
}
