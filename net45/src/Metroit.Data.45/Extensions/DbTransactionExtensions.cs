using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace Metroit.Data.Extensions
{
    /// <summary>
    /// DbTransaction クラスの拡張メソッドを提供します。
    /// </summary>
    public static class DbTransactionExtensions
    {
        /// <summary>
        /// DbCommand オブジェクトを作成します。
        /// </summary>
        /// <param name="trans">DbTransaction オブジェクト。</param>
        /// <param name="bindByName">名前付きパラメーターの使用を行うかどうか。</param>
        /// <returns>DbCommand オブジェクト。</returns>
        public static DbCommand CreateCommand(this DbTransaction trans, bool bindByName = true)
        {
            var command = trans.Connection.CreateCommand();
            if (bindByName)
            {
                command.BindByName();
            }
            command.Transaction = trans;
            return command;
        }

        /// <summary>
        /// DbCommand オブジェクトを作成します。
        /// </summary>
        /// <param name="trans">DbTransaction オブジェクト。</param>
        /// <param name="commandText">クエリ。</param>
        /// <param name="commandType">クエリの種類。</param>
        /// <param name="bindByName">名前付きパラメーターの使用を行うかどうか。</param>
        /// <returns>DbCommand オブジェクト。</returns>
        public static DbCommand CreateCommand(this DbTransaction trans, string commandText = "", CommandType commandType = CommandType.Text, bool bindByName = true)
        {
            var command = trans.CreateCommand(bindByName);
            command.CommandText = commandText;
            command.CommandType = commandType;
            return command;
        }

        /// <summary>
        /// 通常クエリ発行用の DbCommand オブジェクトを作成します。
        /// </summary>
        /// <param name="trans">DbTransaction オブジェクト。</param>
        /// <param name="commandText">クエリ。</param>
        /// <returns>DbCommand オブジェクト。</returns>
        public static DbCommand CreateQueryCommand(this DbTransaction trans, string commandText)
        {
            return trans.CreateCommand(commandText, CommandType.Text, true);
        }

        /// <summary>
        /// プロシージャ発行用の DbCommand オブジェクトを作成します。
        /// </summary>
        /// <param name="trans">DbTransaction オブジェクト。</param>
        /// <param name="commandText">プロシージャー名。</param>
        /// <returns>DbCommand オブジェクト。</returns>
        public static DbCommand CreateProcedureCommand(this DbTransaction trans, string commandText)
        {
            return trans.CreateCommand(commandText, CommandType.StoredProcedure, true);
        }
    }
}
