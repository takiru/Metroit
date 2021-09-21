using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Metroit.IO;

namespace ConvertTest
{
    /// <summary>
    /// ファイルをコピーするコンバータ。
    /// </summary>
    public class FileCopy : FileConverterBase
    {
        /// <summary>
        /// FileCopy の新しいインスタンスを初期化します。
        /// </summary>
        public FileCopy() : base() { }

        /// <summary>
        /// FileCopy の新しいインスタンスを初期化します。
        /// </summary>
        public FileCopy(FileConvertParameter parameter) : base(parameter) { }

        /// <summary>
        /// ファイルの変換処理を行います。
        /// </summary>
        protected override void ConvertFile(FileConvertParameter parameter)
        {
            File.Copy(parameter.SourceConvertFileName, parameter.DestConvertFileName);
        }
    }
}
