namespace Metroit.IO
{
    /// <summary>
    /// ファイルの変換パラメーターを提供します。
    /// </summary>
    public class FileConvertParameter : IConvertParameter
    {
        /// <summary>
        /// 変換元ファイルのパスを取得または設定します。
        /// </summary>
        public string SourceFileName { get; set; }

        /// <summary>
        /// <para>変換元ファイルとして一時ファイルを利用するかどうかを取得または設定します。</para>
        /// <para>利用する場合、変換元ファイルを一時ディレクトリにコピーしてから変換を行います。</para>
        /// </summary>
        public bool UseSourceTemporary { get; set; }

        /// <summary>
        /// <para>変換元の一時ディレクトリを取得または設定します。</para>
        /// <para>設定しない場合、Windows標準の一時ディレクトリを使用します。</para>
        /// </summary>
        public string SourceTempDirectory { get; set; }

        /// <summary>
        /// <para>変換を行う元ファイルパスを取得します。</para>
        /// <para>UseSourceTemporary = true の場合、SourceTempDirectory のファイルパスとなります。</para>
        /// </summary>
        public string SourceConvertFileName { get; internal set; }

        /// <summary>
        /// 変換先ファイルのパスの取得または設定します。
        /// </summary>
        public string DestFileName { get; set; }

        /// <summary>
        /// <para>変換先として一時ディレクトリを利用するかどうかの取得または設定します。</para>
        /// <para>利用する場合、一時ディレクトリに変換してから変換先ファイルへ移動します。</para>
        /// </summary>
        public bool UseDestTemporary { get; set; }

        /// <summary>
        /// <para>変換先の一時ディレクトリの取得または設定します。</para>
        /// <para>設定しない場合、Windows標準の一時ディレクトリを使用します。</para>
        /// </summary>
        public string DestTempDirectory { get; set; }

        /// <summary>
        /// <para>変換先となるファイルパスの取得を行います。</para>
        /// <para>UseDestTemporary = true の場合、DestTempDirectory のファイルパスとなります。</para>
        /// </summary>
        public string DestConvertFileName { get; internal set; }

        /// <summary>
        /// <para>変換先ファイルが既に存在する時、上書きするかどうかの取得または設定します。</para>
        /// <para>上書きしない時、変換先ファイルが存在する場合は変換は行われません。</para>
        /// </summary>
        public bool Overwrite { get; set; }

        /// <summary>
        /// FileConvertParameter クラスの新しいインスタンスを初期化します。
        /// </summary>
        public FileConvertParameter()
        {
            SourceFileName = null;
            UseSourceTemporary = false;
            SourceTempDirectory = null;
            SourceConvertFileName = null;

            DestFileName = null;
            UseDestTemporary = false;
            DestTempDirectory = null;
            DestConvertFileName = null;
            Overwrite = false;
        }

        /// <summary>
        /// FileConvertParameter クラスの新しいインスタンスを初期化します。
        /// <param name="sourceFileName">変換元ファイル。</param>
        /// <param name="destFileName">変換先ファイル。</param>
        /// <param name="overwrite">上書きするかどうか。</param>
        /// </summary>
        public FileConvertParameter(string sourceFileName, string destFileName, bool overwrite = false)
            : this()
        {
            SourceFileName = sourceFileName;
            DestFileName = destFileName;
            Overwrite = overwrite;
        }
    }
}
