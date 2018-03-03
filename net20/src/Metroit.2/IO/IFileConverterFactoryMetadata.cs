namespace Metroit.IO
{
    /// <summary>
    /// ファイル変換コンバーターを決定する為の属性インターフェースを提供します。
    /// </summary>
    public interface IFileConverterFactoryMetadata
    {
        /// <summary>
        /// 変換コンバーター名を取得します。
        /// </summary>
        string ConverterName { get; }

        /// <summary>
        /// 変換元ファイル種類を取得します。
        /// </summary>
        string FromType { get; }

        /// <summary>
        /// 変換先ファイル種類を取得します。
        /// </summary>
        string ToType { get; }
    }
}
