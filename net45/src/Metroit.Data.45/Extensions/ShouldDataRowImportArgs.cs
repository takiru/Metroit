using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroit.Data.Extensions
{
    /// <summary>
    /// DataRow へのデータ設定判断用の情報を提供します。
    /// </summary>
    public class ShouldDataRowImportArgs
    {
        /// <summary>
        /// 項目名を取得します。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 項目値を取得します。
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// 新しい ShouldDataRowImportArgs インスタンスを生成します。
        /// </summary>
        /// <param name="name">項目名。</param>
        /// <param name="value">項目値。</param>
        public ShouldDataRowImportArgs(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
