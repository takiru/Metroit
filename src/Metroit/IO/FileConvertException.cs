using System;

namespace Metroit.IO
{
    /// <summary>
    /// ファイルの変換に失敗した時に発生します。
    /// </summary>
    public class FileConvertException : Exception
    {
        /// <summary>
        /// FileConvertException クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">変換に失敗した理由。</param>
        /// <param name="parameter">変換パラメーター。</param>
        public FileConvertException(string message, FileConvertParameter parameter) : base(message)
        {
            Parameter = parameter;
        }

        /// <summary>
        /// ファイルの変換に失敗した時の変換パラメーターを取得します。
        /// </summary>
        public FileConvertParameter Parameter { get; private set; }
    }
}
