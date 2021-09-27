using System;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Metroit.Data.Extensions
{
    /// <summary>
    /// DataRow クラスの拡張メソッドを提供します。
    /// </summary>
    public static class DataRowExtensions
    {
        /// <summary>
        /// DataRow をエンティティクラスに変換します。
        /// </summary>
        /// <typeparam name="T">エンティティクラス。</typeparam>
        /// <param name="dataRow">DataRow オブジェクト。</param>
        /// <returns>エンティティクラスオブジェクト。</returns>
        public static T ToEntity<T>(this DataRow dataRow) where T : new()
        {
            var resultData = new T();
            var t = resultData.GetType();
            var pis = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
            
            foreach (var pi in pis)
            {
                var columnName = pi.Name;

                // PhysicalNameAttribute が設定されている場合、そのカラム名を使用する
                if (Attribute.GetCustomAttribute(pi, typeof(ColumnAttribute)) is ColumnAttribute attr)
                {
                    columnName = attr.Name;
                }

                // 列名が存在しない場合は値の設定を行わない
                if (!dataRow.Table.Columns.Contains(columnName))
                {
                    continue;
                }

                if (dataRow[columnName] == DBNull.Value)
                {
                    pi.SetValue(resultData, null);
                }
                else
                {
                    pi.SetValue(resultData, dataRow[columnName]);
                }
            }
            return resultData;
        }

        /// <summary>
        /// 指定したオブジェクトの値を DataRow オブジェクトに設定します。
        /// </summary>
        /// <param name="dataRow">DataRow オブジェクト。</param>
        /// <param name="entity">エンティティクラスオブジェクト。</param>
        public static void FromEntity(this DataRow dataRow, object entity)
        {
            var columns = dataRow.Table.Columns;
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
                if (!columns.Contains(columnName))
                {
                    continue;
                }

                var value = pi.GetValue(entity, null);
                if (value == null)
                {
                    dataRow[columnName] = DBNull.Value;
                }
                else
                {
                    dataRow[columnName] = value;
                }
            }
        }
    }
}
