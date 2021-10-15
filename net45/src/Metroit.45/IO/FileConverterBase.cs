using System;
using System.Collections.Generic;
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
        /// 変換をかけた結果に対して更に変換するものを取得または設定します。
        /// </summary>
        public List<FileConverterBase> ReactiveConvert { get; set; } = new List<FileConverterBase>();

        /// <summary>
        /// 反応変換を実施した親のコンバーターを取得します。
        /// </summary>
        public FileConverterBase Parent { get; private set; } = null;

        /// <summary>
        /// 新しい FileConverterBase インスタンスを生成します。
        /// </summary>
        public FileConverterBase() { }

        /// <summary>
        /// 新しい FileConverterBase インスタンスを生成します。
        /// </summary>
        /// <param name="parameter">FileConvertParameter オブジェクト。</param>
        public FileConverterBase(FileConvertParameter parameter) : base()
        {
            Parameter = parameter;
        }

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
            if (parameter.OriginalFileName == null)
            {
                parameter.OriginalFileName = parameter.SourceConvertFileName;
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
                File.Copy(parameter.SourceFileName, parameter.SourceConvertFileName, true);
            }

            try
            {
                // 変換実行
                ConvertFile(parameter);

                // 反応変換の実行
                if (ReactiveConvert != null)
                {
                    foreach (var reactive in ReactiveConvert)
                    {
                        reactive.Parent = this;
                        reactive.Parameter.OriginalFileName = parameter.OriginalFileName;
                        switch (reactive.Parameter.ReactiveTarget)
                        {
                            case ReactiveFileTarget.Original:
                                reactive.Parameter.SourceFileName = parameter.OriginalFileName;
                                break;
                            case ReactiveFileTarget.RecentOriginal:
                                reactive.Parameter.SourceFileName = parameter.SourceConvertFileName;
                                break;
                            case ReactiveFileTarget.RecentConvert:
                                reactive.Parameter.SourceFileName = parameter.DestConvertFileName;
                                break;
                        }
                        reactive.Convert();
                    }
                }

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
                    if (parameter.DestThrough)
                    {
                        File.Delete(parameter.DestConvertFileName);
                        var tempDirectory = Path.GetDirectoryName(parameter.DestConvertFileName);
                        if (Directory.GetFiles(tempDirectory).Length == 0)
                        {
                            Directory.Delete(tempDirectory);
                        }
                    }
                    else
                    {
                        TempFileToDestFile(parameter);
                    }
                }
            }
            catch
            {
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
                    File.Delete(parameter.DestConvertFileName);

                    var tempDirectory = Path.GetDirectoryName(parameter.DestConvertFileName);
                    if (Directory.GetFiles(tempDirectory).Length == 0)
                    {
                        Directory.Delete(tempDirectory);
                    }
                }
                throw;
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
                    (!parameter.DestThrough && string.IsNullOrEmpty(parameter.DestFileName)))
            {
                ErrorMessage = ExceptionResources.GetString("InvalidConvertParameter");
                return false;
            }
            if (!File.Exists(parameter.SourceFileName))
            {
                ErrorMessage = string.Format(ExceptionResources.GetString("NotExistsFilePath"), parameter.SourceFileName);
                return false;
            }

            // 変換をスルーするのに、一時ディレクトリを利用していない場合はNG
            if (parameter.DestThrough && !parameter.UseDestTemporary)
            {
                ErrorMessage = ExceptionResources.GetString("InvalidConvertParameter");
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
