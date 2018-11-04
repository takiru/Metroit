using System;
using System.Collections.Generic;
using System.Text;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// マッチパターン種類を定義します。
    /// </summary>
    [Serializable]
    public enum MatchPatternType
    {
        /// <summary>
        /// 先頭が合致することを示します。
        /// </summary>
        StartsWith,
        /// <summary>
        /// 末尾が合致することを示します。
        /// </summary>
        EndsWith,
        /// <summary>
        /// 一部が合致することを示します。
        /// </summary>
        Partial,
        /// <summary>
        /// すべてが合致することを示します。
        /// </summary>
        Equal
    }
}
