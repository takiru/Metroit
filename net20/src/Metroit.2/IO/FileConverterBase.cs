using System;
using System.IO;

namespace Metroit.IO
{
    /// <summary>
    /// ファイルのコンバーターを表します。
    /// </summary>
    public abstract class FileConverterBase : ConverterBase
    {
        /// <summary>
        /// ファイル変換を実施するパラメーター値を取得します。
        /// </summary>
        protected FileConvertParameter Parameter { get; private set; }

        /// <summary>
        /// エラーメッセージの取得または設定します。
        /// </summary>
        protected string ErrorMessage { get; set; }

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        protected override sealed void DoConvert(IConvertParameter parameter)
        {
            Parameter = parameter as FileConvertParameter;

            // 一時ディレクトリの設定
            Parameter.TemporaryDirectory = SetTemporaryDirectory();

            // 一時ファイルパスの生成
            Parameter.TemporaryFilePath = CreateTemporaryFilePath();

            // 変換先ファイルパスの決定
            Parameter.ConvertingPath = Parameter.UseTemporary
                    ? Parameter.TemporaryFilePath
                    : Parameter.DestinationFilePath;

            // 変換可能なパラメーターであるかを確認する
            if (!CanConvert())
            {
                throw new ArgumentException(ErrorMessage);
            }

            // 一時ファイルパスのディレクトリを作成
            if (Parameter.UseTemporary && !Directory.Exists(Parameter.TemporaryDirectory))
            {
                Directory.CreateDirectory(Parameter.TemporaryDirectory);
            }

            // 変換実行
            ConvertFile(Parameter);

            // 一時パスのファイルを変換後ファイルパスへ移動
            if (Parameter.UseTemporary)
            {
                try
                {
                    TempFileToDestFile();
                }
                catch
                {
                    File.Delete(Parameter.TemporaryFilePath);
                    throw;
                }
            }
        }

        /// <summary>
        /// 変換実行が有効かどうかを取得します。
        /// </summary>
        /// <returns>true:有効, false:無効</returns>
        protected virtual bool CanConvert()
        {
            if (Parameter == null)
            {
                ErrorMessage = ExceptionResources.GetString("InvalidConvertParameter");
                return false;
            }
            if ((Parameter.SourceFilePath == null || Parameter.SourceFilePath == "") ||
                    (Parameter.DestinationFilePath == null || Parameter.DestinationFilePath == ""))
            {
                ErrorMessage = ExceptionResources.GetString("InvalidConvertParameter");
                return false;
            }
            if (!File.Exists(Parameter.SourceFilePath))
            {
                ErrorMessage = string.Format(ExceptionResources.GetString("NotExistsFilePath"), Parameter.SourceFilePath);
                return false;
            }
            if (!Directory.Exists(Path.GetDirectoryName(Parameter.DestinationFilePath)))
            {
                ErrorMessage = string.Format(ExceptionResources.GetString("NotExistsFilePath"), Parameter.DestinationFilePath);
                return false;
            }

            if (Parameter.UseTemporary)
            {
                if (Parameter.DestinationFilePath == Parameter.TemporaryFilePath)
                {
                    ErrorMessage = ExceptionResources.GetString("SameTempPathAndDestFilePath");
                    return false;
                }
            }

            if (!Parameter.Overwrite && File.Exists(Parameter.DestinationFilePath))
            {
                ErrorMessage = ExceptionResources.GetString("ExistsDestFilePath");
                return false;
            }

            return true;
        }

        /// <summary>
        /// ファイルの変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        protected abstract void ConvertFile(FileConvertParameter parameter);

        private string SetTemporaryDirectory()
        {
            if (!Parameter.UseTemporary)
            {
                return null;
            }

            // 一時ディレクトリが未指定時はWindows標準の一時ディレクトリを使用
            if (Parameter.TemporaryDirectory == null || Parameter.TemporaryDirectory == "")
            {
                return Path.GetTempPath();
            }

            return Parameter.TemporaryDirectory;
        }

        /// <summary>
        /// 一時ファイルパスを決定する。
        /// </summary>
        /// <returns>一時ファイルパス。</returns>
        private string CreateTemporaryFilePath()
        {
            if (!Parameter.UseTemporary)
            {
                return null;
            }

            return Path.Combine(Parameter.TemporaryDirectory, Path.GetRandomFileName());
        }

        /// <summary>
        /// 一時ファイルパスを変換後ファイルパスへ移動させる。
        /// </summary>
        private void TempFileToDestFile()
        {
            var fileExists = File.Exists(Parameter.DestinationFilePath);

            // 上書き設定しておらず、変換後ファイルパスにファイルがある場合
            if (!Parameter.Overwrite && fileExists)
            {
                throw new ArgumentException(ExceptionResources.GetString("ExistsDestFilePath"));
            }

            // 変換後ファイルパスへ保存
            if (Parameter.Overwrite && fileExists)
            {
                File.Delete(Parameter.DestinationFilePath);
            }
            var directory = Path.GetDirectoryName(Parameter.DestinationFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.Move(Parameter.TemporaryFilePath, Parameter.DestinationFilePath);
        }
    }
}
