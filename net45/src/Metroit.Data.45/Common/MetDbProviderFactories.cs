using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroit.Data.Common
{
    /// <summary>
    /// DbProviderFactory クラスの1つ以上のインスタンスを作成するための静的メソッドのセットを表します。
    /// </summary>
    public static class MetDbProviderFactories
    {
        /// <summary>
        /// データベース種類から DbProviderFactory のインスタンスを返します。
        /// </summary>
        /// <param name="databaseType">データベース種類。</param>
        /// <returns>DbProviderFactory オブジェクト。</returns>
        public static DbProviderFactory GetFactory(DatabaseType databaseType)
        {
            var databaseName = databaseType.ToString();
            var providerInvariantName = typeof(DbProviderName).GetProperty(databaseName).GetValue(databaseName).ToString();
            return DbProviderFactories.GetFactory(providerInvariantName);
        }
    }
}
