namespace Metroit.Data.Common
{
    /// <summary>
    /// OLEDBエンジンの種類を定義します。
    /// </summary>
    public static class OleEngine
    {
        /// <summary>
        /// Microsoft.Jet.OLEDB.4.0 を示します。
        /// </summary>
        public static string JetOleDb40 => "Microsoft.Jet.OLEDB.4.0";

        /// <summary>
        /// Microsoft.ACE.OLEDB.12.0 を示します。
        /// </summary>
        public static string AceOleDb120 => "Microsoft.ACE.OLEDB.12.0";

    }
}
