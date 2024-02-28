using System;
using System.Collections.Generic;
using System.Data;

namespace Metroit.Data.Extensions
{
    /// <summary>
    /// DataTable クラスの拡張メソッドを提供します。
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// DataRow をエンティティクラスに変換します。
        /// </summary>
        /// <typeparam name="T">エンティティクラス。</typeparam>
        /// <param name="dataTable">DataTable オブジェクト。</param>
        /// <param name="shouldImport">値を設定すべきかどうかを判断する式。</param>
        /// <returns>エンティティクラスオブジェクト。</returns>
        public static IEnumerable<T> AsEnumerableEntity<T>(this DataTable dataTable, Func<ShouldDataRowImportArgs, bool> shouldImport = null) where T : new()
        {
            int i = 0;
            while (i < dataTable.Rows.Count)
            {
                var r = dataTable.Rows[i].ToEntity<T>(shouldImport);
                i++;
                yield return r;
            }
        }
    }
}
