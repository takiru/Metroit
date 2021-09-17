using System;
using System.IO;

namespace Metroit.IO
{
    /// <summary>
    /// ファイルのコンバーターを表します。
    /// </summary>
    public abstract class FileConverterBase : ConverterBase<FileConvertParameter>
    {
        /// <summary>
        /// エラーメッセージの取得または設定します。
        /// </summary>
        protected string ErrorMessage { get; set; }

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        protected override sealed void DoConvert(FileConvertParameter parameter)
        {
            if (!CanConvert(parameter))
            {
                throw new ArgumentException(ErrorMessage);
            }

            // 変換元の一時パスを決定
            parameter.SourceConvertFileName = parameter.SourceFileName;
            if (parameter.UseSourceTemporary)
            {
                parameter.SourceConvertFileName = CreateTempFileName(parameter.SourceTempDirectory, parameter.SourceFileName);
            }

            // 変換先の一時パスを決定
            parameter.DestConvertFileName = parameter.DestFileName;
            if (parameter.UseDestTemporary)
            {
                parameter.DestConvertFileName = CreateTempFileName(parameter.DestTempDirectory, parameter.DestFileName);
            }

            // 変換元ファイルを一時パスへコピー
            if (parameter.UseSourceTemporary)
            {
                File.Copy(parameter.SourceFileName, parameter.SourceConvertFileName);

            }

            // 変換実行
            ConvertFile(parameter);

            // 一時パスのファイルを変換後ファイルパスへ移動
            if (parameter.UseSourceTemporary)
            {
                File.Delete(parameter.SourceConvertFileName);

                var tempDirectory = Path.GetDirectoryName(parameter.SourceConvertFileName);
                if (Directory.GetFiles(tempDirectory).Length == 0)
                {
                    Directory.Delete(tempDirectory);
                }
            }
            if (parameter.UseDestTemporary)
            {
                try
                {
                    TempFileToDestFile(parameter);
                }
                catch
                {
                    File.Delete(parameter.DestConvertFileName);
                    throw;
                }
            }
        }

        /// <summary>
        /// 変換実行が有効かどうかを取得します。
        /// </summary>
        /// <returns>true:有効, false:無効</returns>
        protected virtual bool CanConvert(FileConvertParameter parameter)
        {
            if (parameter == null)
            {
                ErrorMessage = ExceptionResources.GetString("InvalidConvertParameter");
                return false;
            }
            if (string.IsNullOrEmpty(parameter.SourceFileName) ||
                    string.IsNullOrEmpty(parameter.DestFileName))
            {
                ErrorMessage = ExceptionResources.GetString("InvalidConvertParameter");
                return false;
            }
            if (!File.Exists(parameter.SourceFileName))
            {
                ErrorMessage = string.Format(ExceptionResources.GetString("NotExistsFilePath"), parameter.SourceFileName);
                return false;
            }

            if (!parameter.Overwrite && File.Exists(parameter.DestFileName))
            {
                ErrorMessage = ExceptionResources.GetString("ExistsDestFilePath");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 一時ファイルパスを生成する。
        /// </summary>
        /// <param name="tempDirectory"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string CreateTempFileName(string tempDirectory, string fileName)
        {
            var sourceTempDirectory = tempDirectory;

            // 一時ディレクトリが未指定時はWindows標準の一時ディレクトリを使用
            if (string.IsNullOrEmpty(sourceTempDirectory))
            {
                sourceTempDirectory = Path.GetTempPath();
            }

            string tempFileName = Path.Combine(sourceTempDirectory, Path.GetRandomFileName() + Path.GetExtension(fileName));
            Directory.CreateDirectory(sourceTempDirectory);
            return tempFileName;
        }

        /// <summary>
        /// ファイルの変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        protected abstract void ConvertFile(FileConvertParameter parameter);

        /// <summary>
        /// 一時ファイルパスを変換後ファイルパスへ移動させる。
        /// </summary>
        private void TempFileToDestFile(FileConvertParameter parameter)
        {
            var fileExists = File.Exists(parameter.DestFileName);

            // 上書きでない時、変換後に変換先ファイルパスに同名のファイルがある場合
            if (!parameter.Overwrite && fileExists)
            {
                throw new ArgumentException(ExceptionResources.GetString("ExistsDestFilePath"));
            }

            // 変換後ファイルパスへ移動
            if (parameter.Overwrite && fileExists)
            {
                File.Delete(parameter.DestFileName);
            }
            var directory = Path.GetDirectoryName(parameter.DestFileName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.Move(parameter.DestConvertFileName, parameter.DestFileName);

            var tempDirectory = Path.GetDirectoryName(parameter.DestConvertFileName);
            if (Directory.GetFiles(tempDirectory).Length == 0)
            {
                Directory.Delete(tempDirectory);
            }
        }
    }
}
