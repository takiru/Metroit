using System;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Metroit.Data.Extensions
{
    /// <summary>
    /// DataRowCollection クラスの拡張メソッドを提供します。
    /// </summary>
    public static class DataRowCollectionExtensions
    {
        /// <summary>
        /// 指定したオブジェクトを DataRowCollection オブジェクトに追加します。
        /// </summary>
        /// <param name="dataRowCollection">DataRowCollection オブジェクト。</param>
        /// <param name="entity">エンティティクラスオブジェクト。</param>
        public static void AddEntity(this DataRowCollection dataRowCollection, object entity)
        {
            // DataRowCollection に含まれる private table を取得
            var tablePis = dataRowCollection.GetType().GetField("table", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            var table = tablePis.GetValue(dataRowCollection) as DataTable;

            var row = table.NewRow();

            var t = entity.GetType();
            var pis = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            foreach (var pi in pis)
            {
                var columnName = pi.Name;

                // ColumnAttribute が設定されている場合、そのカラム名を使用する
                if (Attribute.GetCustomAttribute(pi, typeof(ColumnAttribute)) is ColumnAttribute attr)
                {
                    columnName = attr.Name;
                }

                // 列名が存在しない場合は値の設定を行わない
                if (!table.Columns.Contains(columnName))
                {
                    continue;
                }

                var value = pi.GetValue(entity, null);
                if (value == null)
                {
                    row[columnName] = DBNull.Value;
                } else
                {
                    row[columnName] = value;
                }
            }

            table.Rows.Add(row);
        }
    }
}
