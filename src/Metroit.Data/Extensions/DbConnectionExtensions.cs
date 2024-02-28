using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Metroit.Data.Extensions
{
    /// <summary>
    /// DbConnection クラスの拡張メソッドを提供します。
    /// </summary>
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// 接続文字列の設定を行います。
        /// </summary>
        /// <param name="connection">DbConnection オブジェクト。</param>
        /// <param name="connectionValues">接続文字列。</param>
        public static void SetConnectionString(this DbConnection connection, Dictionary<string, string> connectionValues)
        {
            var pf = GetProviderFactory(connection);
            var builder = pf.CreateConnectionStringBuilder();
            foreach (KeyValuePair<string, string> config in connectionValues)
            {
                builder[config.Key] = config.Value;
            }
            connection.ConnectionString = builder.ConnectionString;
        }

        /// <summary>
        /// DbCommand オブジェクトを作成します。
        /// </summary>
        /// <param name="connection">DbConnection オブジェクト。</param>
        /// <param name="bindByName">名前付きパラメーターの使用を行うかどうか。</param>
        /// <returns>DbCommand オブジェクト。</returns>
        public static DbCommand CreateCommand(this DbConnection connection, bool bindByName = true)
        {
            var command = connection.CreateCommand();
            if (bindByName)
            {
                command.BindByName();
            }

            return command;
        }

        /// <summary>
        /// DbCommand オブジェクトを作成します。
        /// </summary>
        /// <param name="connection">DbConnection オブジェクト。</param>
        /// <param name="commandText">クエリ。</param>
        /// <param name="commandType">クエリの種類。</param>
        /// <param name="bindByName">名前付きパラメーターの使用を行うかどうか。</param>
        /// <returns>DbCommand オブジェクト。</returns>
        public static DbCommand CreateCommand(this DbConnection connection, string commandText = "", CommandType commandType = CommandType.Text, bool bindByName = true)
        {
            var command = connection.CreateCommand(bindByName);
            command.CommandText = commandText;
            command.CommandType = commandType;

            return command;
        }

        /// <summary>
        /// 通常クエリ発行用の DbCommand オブジェクトを作成します。
        /// </summary>
        /// <param name="connection">DbConnection オブジェクト。</param>
        /// <param name="commandText">クエリ。</param>
        /// <returns>DbCommand オブジェクト。</returns>
        public static DbCommand CreateQueryCommand(this DbConnection connection, string commandText)
        {
            return connection.CreateCommand(commandText, CommandType.Text);
        }

        /// <summary>
        /// プロシージャ発行用の DbCommand オブジェクトを作成します。
        /// </summary>
        /// <param name="connection">DbConnection オブジェクト。</param>
        /// <param name="commandText">プロシージャー名。</param>
        /// <returns>DbCommand オブジェクト。</returns>
        public static DbCommand CreateProcedureCommand(this DbConnection connection, string commandText)
        {
            return connection.CreateCommand(commandText, CommandType.StoredProcedure);
        }

        /// <summary>
        /// DbConnection に紐づいている DbProviderFactory を取得する。
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        internal static DbProviderFactory GetProviderFactory(this DbConnection connection)
        {
            var connectionType = connection.GetType();

            var pf = connectionType
                        .GetProperty("DbProviderFactory", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetValue(connection) as DbProviderFactory;

            // プロバイダの種類によってDbProviderFactoryから取得できなかった場合、ConnectionFactoryから取得する
            if (pf == null)
            {
                var connectionFactory = connectionType
                        .GetProperty("ConnectionFactory", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetValue(connection);
                pf = connectionFactory.GetType()
                        .GetProperty("ProviderFactory", BindingFlags.Instance | BindingFlags.Public)
                        .GetValue(connectionFactory) as DbProviderFactory;
            }

            return pf;
        }
    }
}
