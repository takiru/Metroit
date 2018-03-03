using System;
using System.Collections.Generic;
using System.Text;

namespace Metroit.Data
{
    /// <summary>
    /// データベース固有のバインドパラメータ識別子を定義します。
    /// </summary>
    public enum QueryBindVariableType
    {
        /// <summary>
        /// @パラメータ名 を示します。
        /// </summary>
        AtmarkWithParam,
        /// <summary>
        /// :パラメータ名 を示します。
        /// </summary>
        ColonWithParam,
        /// <summary>
        /// ? を示します。
        /// </summary>
        Question
    }
}
