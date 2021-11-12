namespace Metroit.IO
{
    /// <summary>
    /// 変換パラメーター用のインターフェースを提供します。
    /// </summary>
    public interface IConvertParameter
    {
        /// <summary>
        /// 任意の変換パラメーターを取得または設定します。
        /// </summary>
        object Params { get; set; }
    }
}
