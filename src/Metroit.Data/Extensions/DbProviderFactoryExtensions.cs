using System.Data;
using System.Data.Common;

namespace Metroit.Data.Extensions
{
    /// <summary>
    /// DbProviderFactory クラスの拡張メソッドを提供します。
    /// </summary>
    public static class DbProviderFactoryExtensions
    {
        /// <summary>
        /// DbProviderFactory に基づく DbParameter を生成します。
        /// </summary>
        /// <param name="providerFactory">DbProviderFactory オブジェクト。</param>
        /// <param name="parameterName">パラメーター名。</param>
        /// <param name="obj">パラメーター。</param>
        /// <returns>DbParameter オブジェクト。</returns>
        public static DbParameter CreateParameter(this DbProviderFactory providerFactory, string parameterName, object obj)
        {
            var p = providerFactory.CreateParameter();
            p.ParameterName = parameterName;
            p.Value = obj;
            return p;
        }

        /// <summary>
        /// DbProviderFactory に基づく DbParameter を生成します。
        /// </summary>
        /// <param name="providerFactory">DbProviderFactory オブジェクト。</param>
        /// <param name="parameterName">パラメーター名。</param>
        /// <param name="dbType">DbType 値。</param>
        /// <returns>DbParameter オブジェクト。</returns>
        public static DbParameter CreateParameter(this DbProviderFactory providerFactory, string parameterName, DbType dbType)
        {
            var p = providerFactory.CreateParameter();
            p.ParameterName = parameterName;
            p.DbType = dbType;
            return p;
        }

        /// <summary>
        /// DbProviderFactory に基づく DbParameter を生成します。
        /// </summary>
        /// <param name="providerFactory">DbProviderFactory オブジェクト。</param>
        /// <param name="parameterName">パラメーター名。</param>
        /// <param name="dbType">DbType 値。</param>
        /// <param name="obj">パラメーター。</param>
        /// <param name="direction">パラメーターの入出力方向。</param>
        /// <returns>DbParameter オブジェクト。</returns>
        public static DbParameter CreateParameter(this DbProviderFactory providerFactory, string parameterName, DbType dbType, object obj, ParameterDirection direction)
        {
            var p = providerFactory.CreateParameter(parameterName, dbType);
            p.Value = obj;
            p.Direction = direction;
            return p;
        }

        /// <summary>
        /// DbProviderFactory に基づく DbParameter を生成します。
        /// </summary>
        /// <param name="providerFactory">DbProviderFactory オブジェクト。</param>
        /// <param name="parameterName">パラメーター名。</param>
        /// <param name="dbType">DbType 値。</param>
        /// <param name="size">バッファサイズ。</param>
        /// <returns>DbParameter オブジェクト。</returns>
        public static DbParameter CreateParameter(this DbProviderFactory providerFactory, string parameterName, DbType dbType, int size)
        {
            var p = providerFactory.CreateParameter(parameterName, dbType);
            p.Size = size;
            return p;
        }

        /// <summary>
        /// DbProviderFactory に基づく DbParameter を生成します。
        /// </summary>
        /// <param name="providerFactory">DbProviderFactory オブジェクト。</param>
        /// <param name="parameterName">パラメーター名。</param>
        /// <param name="dbType">DbType 値。</param>
        /// <param name="size">バッファサイズ。</param>
        /// <param name="obj">パラメーター。</param>
        /// <param name="direction">パラメーターの入出力方向。</param>
        /// <returns>DbParameter オブジェクト。</returns>
        public static DbParameter CreateParameter(this DbProviderFactory providerFactory, string parameterName, DbType dbType, int size, object obj, ParameterDirection direction)
        {
            var p = providerFactory.CreateParameter(parameterName, dbType);
            p.Size = size;
            p.Value = obj;
            p.Direction = direction;
            return p;
        }

        /// <summary>
        /// DbProviderFactory に基づく DbParameter を生成します。
        /// </summary>
        /// <param name="providerFactory">DbProviderFactory オブジェクト。</param>
        /// <param name="parameterName">パラメーター名。</param>
        /// <param name="dbType">DbType 値。</param>
        /// <param name="direction">パラメーターの入出力方向。</param>
        /// <returns>DbParameter オブジェクト。</returns>
        public static DbParameter CreateParameter(this DbProviderFactory providerFactory, string parameterName, DbType dbType, ParameterDirection direction)
        {
            var p = providerFactory.CreateParameter(parameterName, dbType);
            p.Direction = direction;
            return p;
        }
    }
}
