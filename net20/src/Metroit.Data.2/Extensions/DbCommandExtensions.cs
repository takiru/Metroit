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
        /// <param name="providerFactory">DbProviderFactory オブジェクト。</param>
        /// <param name="dataTable">DataTable オブジェクト。</param>
        /// <param name="schemaMapping">スキーマ情報のマッピングを行うかどうか。</param>
        /// <param name="commandCreate">各更新コマンドの生成を行うかどうか。</param>
        /// <returns>DbDataAdapter オブジェクト。</returns>
        public static DbDataAdapter Fill(this DbCommand command, DbProviderFactory providerFactory,
                DataTable dataTable, bool schemaMapping = false, bool commandCreate = false)
        {
            var da = providerFactory.CreateDataAdapter();
            da.SelectCommand = command;
            
            if (commandCreate)
            {
                var cb = providerFactory.CreateCommandBuilder();
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
        /// <param name="providerFactory">DbProviderFactory オブジェクト。</param>
        /// <param name="dataSet">DataSet オブジェクト。</param>
        /// <param name="tableName">テーブル名。</param>
        /// <param name="schemaMapping">スキーマ情報のマッピングを行うかどうか。</param>
        /// <param name="commandCreate">各更新コマンドの生成を行うかどうか。</param>
        /// <returns>DbDataAdapter オブジェクト。</returns>
        public static DbDataAdapter FillToDataSet(this DbCommand command, DbProviderFactory providerFactory,
                DataSet dataSet, string tableName = null, bool schemaMapping = false, bool commandCreate = false)
        {
            var da = providerFactory.CreateDataAdapter();
            da.SelectCommand = command;

            if (commandCreate)
            {
                var cb = providerFactory.CreateCommandBuilder();
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
            prop.SetValue(command, true, null);
        }
    }
}
