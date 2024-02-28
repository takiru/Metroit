namespace Metroit.IO
{
    /// <summary>
    /// ファイルの変換パラメーターを提供します。
    /// </summary>
    public class FileConvertParameter : IConvertParameter
    {
        /// <summary>
        /// 任意の変換パラメーターを取得または設定します。
        /// </summary>
        public object Params { get; set; }

        /// <summary>
        /// どのファイルを処理ターゲットにするかを取得または設定します。
        /// </summary>
        public ReactiveFileTarget ReactiveTarget { get; set; } = ReactiveFileTarget.Original;

        /// <summary>
        /// 変換が実施されたオリジナルファイルのパスを取得します。
        /// 一時ディレクトリを利用している場合、一時ディレクトリにコピーしたオリジナルファイルのパスとなります。
        /// </summary>
        public string OriginalFileName { get; internal set; } = null;

        /// <summary>
        /// 変換元ファイルのパスを取得または設定します。
        /// </summary>
        public string SourceFileName { get; set; } = null;

        /// <summary>
        /// <para>変換元ファイルとして一時ファイルを利用するかどうかを取得または設定します。</para>
        /// <para>利用する場合、変換元ファイルを一時ディレクトリにコピーしてから変換を行います。</para>
        /// </summary>
        public bool UseSourceTemporary { get; set; } = false;

        /// <summary>
        /// <para>変換元の一時ディレクトリを取得または設定します。</para>
        /// <para>設定しない場合、Windows標準の一時ディレクトリを使用します。</para>
        /// </summary>
        public string SourceTempDirectory { get; set; } = null;

        /// <summary>
        /// <para>変換を行う元ファイルパスを取得します。</para>
        /// <para>UseSourceTemporary = true の場合、SourceTempDirectory のファイルパスとなります。</para>
        /// </summary>
        public string SourceConvertFileName { get; internal set; } = null;

        /// <summary>
        /// 変換先ファイルのパスの取得または設定します。
        /// </summary>
        public string DestFileName { get; set; } = null;

        /// <summary>
        /// <para>変換先として一時ディレクトリを利用するかどうかの取得または設定します。</para>
        /// <para>利用する場合、一時ディレクトリに変換してから変換先ファイルへ移動します。</para>
        /// </summary>
        public bool UseDestTemporary { get; set; } = false;

        /// <summary>
        /// <para>変換先の一時ディレクトリの取得または設定します。</para>
        /// <para>設定しない場合、Windows標準の一時ディレクトリを使用します。</para>
        /// </summary>
        public string DestTempDirectory { get; set; } = null;

        /// <summary>
        /// <para>変換先となるファイルパスの取得を行います。</para>
        /// <para>UseDestTemporary = true の場合、DestTempDirectory のファイルパスとなります。</para>
        /// </summary>
        public string DestConvertFileName { get; internal set; } = null;

        /// <summary>
        /// <para>変換先ファイルが既に存在する時、上書きするかどうかの取得または設定します。</para>
        /// <para>上書きしない時、変換先ファイルが存在する場合は変換は行われません。</para>
        /// </summary>
        public bool Overwrite { get; set; } = false;

        /// <summary>
        /// <para>変換先の一時ディレクトリを使用時、段階的な変換を必要とする時に、途中の変換結果をスルーするかどうかを取得または設定します。</para>
        /// <para>true が選択され、UseDestTemporary = true の場合、DestFileName にファイルが作られません。</para>
        /// </summary>
        public bool DestThrough { get; set; } = false;

        /// <summary>
        /// FileConvertParameter クラスの新しいインスタンスを初期化します。
        /// </summary>
        public FileConvertParameter() { }

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
