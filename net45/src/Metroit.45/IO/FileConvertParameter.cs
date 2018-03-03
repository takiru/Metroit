namespace Metroit.IO
{
    /// <summary>
    /// ファイルの変換パラメーターを提供します。
    /// </summary>
    public class FileConvertParameter : IConvertParameter
    {
        /// <summary>
        /// 変換元ファイルのパスの取得または設定します。
        /// </summary>
        public string SourceFilePath { get; set; }

        /// <summary>
        /// ファイルを保存する変換先パスの取得または設定します。
        /// </summary>
        public string DestinationFilePath { get; set; }

        /// <summary>
        /// 一時ディレクトリを利用した変換を行うかどうかの取得または設定します。
        /// </summary>
        public bool UseTemporary { get; set; }

        /// <summary>
        /// <para>一時ディレクトリの取得または設定します。</para>
        /// <para>設定しない場合、Windows標準の一時ディレクトリを使用します。</para>
        /// </summary>
        public string TemporaryDirectory { get; set; }

        /// <summary>
        /// DestinationFilePath に既に同名のファイルがあった時、上書きするかどうかの取得または設定します。
        /// </summary>
        public bool Overwrite { get; set; }

        /// <summary>
        /// 一時ディレクトリを利用した際に、変換が行われたファイルのフルパスの取得を行います。
        /// </summary>
        public string TemporaryFilePath { get; internal set; }

        /// <summary>
        /// <para>変換を行うファイルパスの取得を行います。</para>
        /// <para>UseTemporary=trueの場合、TemporaryFilePath と同じです。</para>
        /// <para>UseTemporary=falseの場合、DestinationFilePath と同じです。</para>
        /// </summary>
        public string ConvertingPath { get; internal set; }

        /// <summary>
        /// FileConvertParameter クラスの新しいインスタンスを初期化します。
        /// </summary>
        public FileConvertParameter()
        {
            SourceFilePath = null;
            DestinationFilePath = null;
            UseTemporary = false;
            TemporaryDirectory = null;
            Overwrite = false;
            TemporaryFilePath = null;
            ConvertingPath = null;
        }

        /// <summary>
        /// FileConvertParameter クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="useTemporary">一時ディレクトリを利用するかどうか。</param>
        /// <param name="directory">一時ディレクトリ。</param>
        public FileConvertParameter(bool useTemporary, string directory = null)
            : this()
        {
            UseTemporary = useTemporary;
            if (useTemporary)
            {
                TemporaryDirectory = directory;
            }
        }

        /// <summary>
        /// FileConvertParameter クラスの新しいインスタンスを初期化します。
        /// <param name="sourceFilePath">変換元ファイルのフルパス。</param>
        /// <param name="useTemporary">一時ディレクトリを利用するかどうか。</param>
        /// <param name="directory">一時ディレクトリ。</param>
        /// </summary>
        public FileConvertParameter(string sourceFilePath, bool useTemporary = false, string directory = null)
            : this(useTemporary, directory)
        {
            SourceFilePath = sourceFilePath;
        }

        /// <summary>
        /// FileConvertParameter クラスの新しいインスタンスを初期化します。
        /// <param name="sourceFilePath">変換元ファイルのフルパス。</param>
        /// <param name="destinationFilePath">変換先ファイルのフルパス。</param>
        /// <param name="useTemporary">一時ディレクトリを利用するかどうか。</param>
        /// <param name="directory">一時ディレクトリ。</param>
        /// </summary>
        public FileConvertParameter(string sourceFilePath, string destinationFilePath, bool useTemporary = false,
                string directory = null)
            : this(sourceFilePath, useTemporary, directory)
        {
            DestinationFilePath = destinationFilePath;
        }
    }
}
