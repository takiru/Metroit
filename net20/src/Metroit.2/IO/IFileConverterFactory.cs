namespace Metroit.IO
{
    /// <summary>
    /// ファイル変換コンバーター生成用のインターフェースを提供します。
    /// </summary>
    public interface IFileConverterFactory
    {
        /// <summary>
        /// 変換コンバーターを生成します。
        /// </summary>
        /// <returns>変換コンバーター。</returns>
        FileConverterBase Create();
    }
}
