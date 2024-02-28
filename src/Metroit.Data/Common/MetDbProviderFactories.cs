using System.Data.Common;

namespace Metroit.Data.Common
{
#if (NETSTANDARD2_1_OR_GREATER || NET45_OR_GREATER)
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
#endif
}
