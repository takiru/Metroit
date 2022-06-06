using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Metroit.Data.Extensions
{
    /// <summary>
    /// IDataReader クラスの拡張メソッドを提供します。
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// IDataReader の現在のレコードをエンティティクラスに変換します。
        /// </summary>
        /// <typeparam name="T">エンティティクラス。</typeparam>
        /// <param name="dataReader">IDataReader オブジェクト。</param>
        /// <param name="shouldImport">値を設定すべきかどうかを判断する式。</param>
        /// <returns>エンティティクラスオブジェクト。</returns>
        public static T ToEntity<T>(this IDataReader dataReader, Func<ShouldDataRowImportArgs, bool> shouldImport = null) where T : new()
        {
            // DataReader の情報を取得
            var drItems = Enumerable.Range(0, dataReader.FieldCount)
                    .Select((i) => new
                    {
                        Name = dataReader.GetName(i),
                        Value = dataReader.GetValue(i)
                    })
                    .ToList();

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

                var drItem = drItems.Where((x) => x.Name == columnName).FirstOrDefault();

                // 列名が存在しない場合は値の設定を行わない
                if (drItem == null)
                {
                    continue;
                }

                // 設定すべきでない列はスキップする
                if (!(shouldImport?.Invoke(new ShouldDataRowImportArgs(columnName, drItem.Value)) ?? true))
                {
                    continue;
                }

                try
                {
                    if (drItem.Value == DBNull.Value)
                    {
                        pi.SetValue(resultData, null);
                        continue;
                    }

                    if (pi.PropertyType == drItem.Value.GetType())
                    {
                        pi.SetValue(resultData, drItem.Value);
                        continue;
                    }

                    var safeType = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;
                    if (safeType.IsEnum)
                    {
                        pi.SetValue(resultData, Enum.ToObject(safeType,
                            Convert.ChangeType(drItem.Value, safeType.GetField("value__").FieldType)));
                        continue;
                    }
                    pi.SetValue(resultData, Convert.ChangeType(drItem.Value, safeType));

                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message,columnName, ex);
                }
            }
            return resultData;
        }
    }
}
