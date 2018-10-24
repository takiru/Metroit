using System;
using System.Collections.Generic;
using System.Text;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// カスタムオートコンプリートのモードを定義します。
    /// </summary>
    public enum CustomAutoCompleteMode
    {
        /// <summary>
        /// 利用しない。
        /// </summary>
        None,
        /// <summary>
        /// キーによる利用。
        /// </summary>
        Keys,
        /// <summary>
        /// サジェストによる利用。
        /// </summary>
        Suggest,
        /// <summary>
        /// キーとサジェストによる利用。
        /// </summary>
        KeysSuggest
    }
}
