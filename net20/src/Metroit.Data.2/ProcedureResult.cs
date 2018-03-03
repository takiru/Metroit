using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace Metroit.Data
{
    /// <summary>
    /// プロシージャの実行結果を提供します。
    /// </summary>
    public class ProcedureResult
    {
        /// <summary>
        /// MetDbProcedureResult クラスの新しいインスタンスを初期化します。
        /// </summary>
        internal ProcedureResult() { }

        /// <summary>
        /// 戻り値を取得します。
        /// </summary>
        public object ReturnValue { get; internal set; } = null;

        /// <summary>
        /// OUTPUT, INOUT の値を取得します。
        /// </summary>
        public Dictionary<string, object> Output { get; internal set; } = new Dictionary<string, object>();
    }
}
