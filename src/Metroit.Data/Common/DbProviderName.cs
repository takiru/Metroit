using System;

namespace Metroit.Data.Common
{
    /// <summary>
    /// 既知のデータベースプロバイダーの不変名を定義します。
    /// </summary>
    public static class DbProviderName
    {
        /// <summary>
        /// System.Data.OleDb を示します。
        /// </summary>
        public static string Ole => @"System.Data.OleDb";

        /// <summary>
        /// System.Data.SqlClient を示します。
        /// </summary>
        public static string MsSql => @"System.Data.SqlClient";

        /// <summary>
        /// Oracle.DataAccess.Client を示します。
        /// </summary>
        public static string Oracle => @"Oracle.DataAccess.Client";

        /// <summary>
        /// Oracle.ManagedDataAccess.Client を示します。
        /// </summary>
        public static string OracleManaged => @"Oracle.ManagedDataAccess.Client";

        /// <summary>
        /// MySql.Data.MySqlClient を示します。
        /// </summary>
        public static String MySql => @"MySql.Data.MySqlClient";

        /// <summary>
        /// Npgsql を示します。
        /// </summary>
        public static string Npgsql => @"Npgsql";

        /// <summary>
        /// IBM.Data.DB2 を示します。
        /// </summary>
        public static string Db2 => @"IBM.Data.DB2";

        /// <summary>
        /// System.Data.SQLite を示します。
        /// </summary>
        public static string Sqlite => @"System.Data.SQLite";

        /// <summary>
        /// FirebirdSql.Data.FirebirdClient を示します。
        /// </summary>
        public static string Firebird => @"FirebirdSql.Data.FirebirdClient";

        /// <summary>
        /// Hitachi.HiRDB を示します。
        /// </summary>
        public static string HirDB => @"Hitachi.HiRDB";
    }
}
