using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Metroit.Data.Extensions
{
    /// <summary>
    /// DbCommand クラスの拡張メソッドを提供します。
    /// </summary>
    public static class DbCommandExtensions
    {
        /// <summary>
        /// <para>DbCommand の実行結果を DataTable へ投入します。</para>
        /// <para>スキーマ情報をマッピングする場合は mapppingSchema を true にします。</para>
        /// <para>各更新コマンドを生成する場合は createCommand を true にします。</para>
        /// <para>mappingSchema, createCommand は単一テーブルの操作の時のみ有効です。サブクエリによる単一テーブルの操作も有効にはなりません。</para>
        /// </summary>
        /// <param name="command">DbCommand オブジェクト。</param>
        /// <param name="dataTable">DataTable オブジェクト。</param>
        /// <param name="schemaMapping">スキーマ情報のマッピングを行うかどうか。</param>
        /// <param name="commandCreate">各更新コマンドの生成を行うかどうか。</param>
        /// <returns>DbDataAdapter オブジェクト。</returns>
        public static DbDataAdapter Fill(this DbCommand command,
                DataTable dataTable, bool schemaMapping = false, bool commandCreate = false)
        {
            var pf = command.Connection.GetType().GetProperty("DbProviderFactory",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .GetValue(command.Connection) as DbProviderFactory;
            var da = pf.CreateDataAdapter();
            da.SelectCommand = command;
            
            if (commandCreate)
            {
                var cb = pf.CreateCommandBuilder();
                cb.DataAdapter = da;
                da.InsertCommand = cb.GetInsertCommand(true);
                da.UpdateCommand = cb.GetUpdateCommand(true);
                da.DeleteCommand = cb.GetDeleteCommand(true);
            }

            if (schemaMapping)
            {
                da.FillSchema(dataTable, SchemaType.Mapped);
            }
            da.Fill(dataTable);

            return da;
        }

        /// <summary>
        /// <para>DbCommand の実行結果を DataSet へ投入します。</para>
        /// <para>スキーマ情報をマッピングする場合は mapppingSchema を true にします。</para>
        /// <para>各更新コマンドを生成する場合は createCommand を true にします。</para>
        /// <para>mappingSchema, createCommand は単一テーブルの操作の時のみ有効です。サブクエリによる単一テーブルの操作も有効にはなりません。</para>
        /// </summary>
        /// <param name="command">DbCommand オブジェクト。</param>
        /// <param name="dataSet">DataSet オブジェクト。</param>
        /// <param name="tableName">テーブル名。</param>
        /// <param name="schemaMapping">スキーマ情報のマッピングを行うかどうか。</param>
        /// <param name="commandCreate">各更新コマンドの生成を行うかどうか。</param>
        /// <returns>DbDataAdapter オブジェクト。</returns>
        public static DbDataAdapter FillToDataSet(this DbCommand command,
                DataSet dataSet, string tableName = null, bool schemaMapping = false, bool commandCreate = false)
        {
            var pf = command.Connection.GetType().GetProperty("DbProviderFactory", 
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .GetValue(command.Connection) as DbProviderFactory;
            var da = pf.CreateDataAdapter();
            da.SelectCommand = command;

            if (commandCreate)
            {
                var cb = pf.CreateCommandBuilder();
                cb.DataAdapter = da;
                da.InsertCommand = cb.GetInsertCommand(true);
                da.UpdateCommand = cb.GetUpdateCommand(true);
                da.DeleteCommand = cb.GetDeleteCommand(true);
            }

            if (tableName == string.Empty || tableName == null)
            {
                if (schemaMapping)
                {
                    da.FillSchema(dataSet, SchemaType.Mapped);
                }
                da.Fill(dataSet);
            }
            else
            {
                if (schemaMapping)
                {
                    da.FillSchema(dataSet, SchemaType.Mapped, tableName);
                }
                da.Fill(dataSet, tableName);
            }

            return da;
        }

        /// <summary>
        /// プロシージャー実行による DbCommand の実行結果を取得します。
        /// </summary>
        public static ProcedureResult GetProcedureResult(this DbCommand command)
        {
            var result = new ProcedureResult();

            // INOUT、OUTPUT、戻り値を格納する
            foreach (DbParameter parameter in command.Parameters)
            {
                if (parameter.Direction == ParameterDirection.ReturnValue)
                {
                    result.ReturnValue = parameter.Value;
                }
                if (parameter.Direction == ParameterDirection.InputOutput ||
                        parameter.Direction == ParameterDirection.Output)
                {
                    result.Output.Add(parameter.ParameterName, parameter.Value);
                }
            }
            
            return result;
        }

        /// <summary>
        /// <para>BindByName プロパティの設定を行います。</para>
        /// <para>Oracleで名前付きパラメーターを利用したクエリの実行で必要になります。</para>
        /// </summary>
        public static void BindByName(this DbCommand command)
        {
            var prop = command.GetType().GetProperty("BindByName") ?? null;
            if (prop == null)
            {
                return;
            }
            prop.SetValue(command, true);
        }

        /// <summary>
        /// DbParameter オブジェクトの新しいインスタンスを作成します。
        /// </summary>
        /// <param name="command">DbCommand オブジェクト。</param>
        /// <param name="parameterName">パラメーター名。</param>
        /// <param name="obj">パラメーター。</param>
        /// <returns>DbParameter オブジェクト。</returns>
        public static DbParameter CreateParameter(this DbCommand command, string parameterName, object obj)
        {
            var p = command.CreateParameter();
            p.ParameterName = parameterName;
            p.Value = obj;
            return p;
        }

        /// <summary>
        /// DbParameter オブジェクトの新しいインスタンスを作成します。
        /// </summary>
        /// <param name="command">DbCommand オブジェクト。</param>
        /// <param name="parameterName">パラメーター名。</param>
        /// <param name="dbType">DbType 値。</param>
        /// <returns>DbParameter オブジェクト。</returns>
        public static DbParameter CreateParameter(this DbCommand command, string parameterName, DbType dbType)
        {
            var p = command.CreateParameter();
            p.ParameterName = parameterName;
            p.DbType = dbType;
            return p;
        }

        /// <summary>
        /// DbParameter オブジェクトの新しいインスタンスを作成します。
        /// </summary>
        /// <param name="command">DbCommand オブジェクト。</param>
        /// <param name="parameterName">パラメーター名。</param>
        /// <param name="dbType">DbType 値。</param>
        /// <param name="obj">パラメーター。</param>
        /// <param name="direction">パラメーターの入出力方向。</param>
        /// <returns>DbParameter オブジェクト。</returns>
        public static DbParameter CreateParameter(this DbCommand command, string parameterName, DbType dbType, object obj, ParameterDirection direction)
        {
            var p = CreateParameter(command, parameterName, dbType);
            p.Value = obj;
            p.Direction = direction;
            return p;
        }

        /// <summary>
        /// DbParameter オブジェクトの新しいインスタンスを作成します。
        /// </summary>
        /// <param name="command">DbCommand オブジェクト。</param>
        /// <param name="parameterName">パラメーター名。</param>
        /// <param name="dbType">DbType 値。</param>
        /// <param name="size">バッファサイズ。</param>
        /// <returns>DbParameter オブジェクト。</returns>
        public static DbParameter CreateParameter(this DbCommand command, string parameterName, DbType dbType, int size)
        {
            var p = CreateParameter(command, parameterName, dbType);
            p.Size = size;
            return p;
        }

        /// <summary>
        /// DbParameter オブジェクトの新しいインスタンスを作成します。
        /// </summary>
        /// <param name="command">DbCommand オブジェクト。</param>
        /// <param name="parameterName">パラメーター名。</param>
        /// <param name="dbType">DbType 値。</param>
        /// <param name="size">バッファサイズ。</param>
        /// <param name="obj">パラメーター。</param>
        /// <param name="direction">パラメーターの入出力方向。</param>
        /// <returns>DbParameter オブジェクト。</returns>
        public static DbParameter CreateParameter(this DbCommand command, string parameterName, DbType dbType, int size, object obj, ParameterDirection direction)
        {
            var p = CreateParameter(command, parameterName, dbType);
            p.Size = size;
            p.Value = obj;
            p.Direction = direction;
            return p;
        }

        /// <summary>
        /// DbParameter オブジェクトの新しいインスタンスを作成します。
        /// </summary>
        /// <param name="command">DbCommand オブジェクト。</param>
        /// <param name="parameterName">パラメーター名。</param>
        /// <param name="dbType">DbType 値。</param>
        /// <param name="direction">パラメーターの入出力方向。</param>
        /// <returns>DbParameter オブジェクト。</returns>
        public static DbParameter CreateParameter(this DbCommand command, string parameterName, DbType dbType, ParameterDirection direction)
        {
            var p = CreateParameter(command, parameterName, dbType);
            p.Direction = direction;
            return p;
        }
    }
}
