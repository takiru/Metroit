namespace Metroit.Data.Common
{
    /// <summary>
    /// 既知のデータベース種類を定義します。
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// OLE を示します。
        /// </summary>
        Ole,
        /// <summary>
        /// SQL Server を示します。
        /// </summary>
        MsSql,
        /// <summary>
        /// ODP.NET を示します。
        /// </summary>
        Oracle,
        /// <summary>
        /// ODP.NET Managed を示します。
        /// </summary>
        OracleManaged,
        /// <summary>
        /// MySQL を示します。
        /// </summary>
        MySql,
        /// <summary>
        /// PostgreSQL を示します。
        /// </summary>
        Npgsql,
        /// <summary>
        /// DB2 を示します。
        /// </summary>
        Db2,
        /// <summary>
        /// SQLite を示します。
        /// </summary>
        Sqlite,
        /// <summary>
        /// Firebird を示します。
        /// </summary>
        Firebird,
        /// <summary>
        /// HiRDB を示します。
        /// </summary>
        HirDB
    }
}
