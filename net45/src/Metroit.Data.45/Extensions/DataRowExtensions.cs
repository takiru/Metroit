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
        /// <param name="shouldImport">値を設定すべきかどうかを判断する式。</param>
        /// <returns>エンティティクラスオブジェクト。</returns>
        public static T ToEntity<T>(this DataRow dataRow, Func<ShouldDataRowImportArgs, bool> shouldImport = null) where T : new()
        {
            var resultData = new T();
            var t = resultData.GetType();
            var pis = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

            foreach (var pi in pis)
            {
                var columnName = pi.Name;

                // ColumnAttribute が設定されている場合、そのカラム名を使用する
                if (Attribute.GetCustomAttribute(pi, typeof(ColumnAttribute)) is ColumnAttribute attr)
                {
                    columnName = attr.Name;
                }

                // 列名が存在しない場合は値の設定を行わない
                if (!dataRow.Table.Columns.Contains(columnName))
                {
                    continue;
                }

                // 設定すべきでない列はスキップする
                if (!(shouldImport?.Invoke(new ShouldDataRowImportArgs(columnName, dataRow[columnName])) ?? true))
                {
                    continue;
                }

                try
                {
                    if (dataRow[columnName] == DBNull.Value)
                    {
                        pi.SetValue(resultData, null);
                        continue;
                    }

                    if (pi.PropertyType == dataRow[columnName].GetType())
                    {
                        pi.SetValue(resultData, dataRow[columnName]);
                        continue;
                    }

                    var safeType = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;
                    if (safeType.IsEnum)
                    {
                        pi.SetValue(resultData, Enum.ToObject(safeType,
                            Convert.ChangeType(dataRow[columnName], safeType.GetField("value__").FieldType)));
                        continue;
                    }
                    pi.SetValue(resultData, Convert.ChangeType(dataRow[columnName], safeType));

                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message, columnName, ex);
                }
            }
            return resultData;
        }

        /// <summary>
        /// 指定したオブジェクトの値を DataRow オブジェクトに設定します。
        /// </summary>
        /// <param name="dataRow">DataRow オブジェクト。</param>
        /// <param name="entity">エンティティクラスオブジェクト。</param>
        /// <param name="shouldImport">値を設定すべきかどうかを判断する式。</param>
        public static void FromEntity(this DataRow dataRow, object entity, Func<ShouldDataRowImportArgs, bool> shouldImport = null)
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

                // 設定すべきでない列はスキップする
                if (!(shouldImport?.Invoke(new ShouldDataRowImportArgs(columnName, value)) ?? true))
                {
                    continue;
                }

                try
                {
                    if (value == null)
                    {
                        dataRow[columnName] = DBNull.Value;
                        continue;
                    }

                    if (pi.PropertyType.IsEnum)
                    {
                        dataRow[columnName] = Convert.ChangeType(value, Convert.GetTypeCode(value));
                        continue;
                    }

                    dataRow[columnName] = value;

                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message, columnName, ex);
                }
            }
        }
    }
}
