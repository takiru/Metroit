using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;

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
        /// 変換処理が完了した時に発生します。
        /// </summary>
        public Action<FileConvertParameter> ConvertSucceed { get; set; } = null;

        /// <summary>
        /// 変換処理が失敗した時に発生します。
        /// </summary>
        public Action<FileConvertParameter, Exception> ConvertFailed { get; set; } = null;

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        /// <param name="token">キャンセルトークン。</param>
        protected override sealed ConvertCompleteEventArgs DoConvert(FileConvertParameter parameter, CancellationToken token)
        {
            return ExecuteConvert(this);
        }

        /// <summary>
        /// 変換を行う。
        /// </summary>
        /// <param name="converter">変換コンバーター。</param>
        /// <param name="isReactive">反応したものかどうか。</param>
        /// <returns>ConvertCompleteEventArgs オブジェクト。</returns>
        private ConvertCompleteEventArgs ExecuteConvert(FileConverterBase converter, bool isReactive = false)
        {
            if (isReactive)
            {
                var cea = new CancelEventArgs();
                cea.Cancel = false;
                converter.Prepare?.Invoke(converter.Parameter, cea);
                if (cea.Cancel)
                {
                    var ccea = new ConvertCompleteEventArgs();
                    ccea.Result = ConvertResultType.Cancelled;
                    return ccea;
                }
            }

            if (!CanConvert(converter.Parameter))
            {
                var exception = new ArgumentException(ErrorMessage);
                converter.ConvertFailed?.Invoke(converter.Parameter, exception);
                throw exception;
            }

            // 変換元の一時パスを決定
            converter.Parameter.SourceConvertFileName = converter.Parameter.SourceFileName;
            if (converter.Parameter.UseSourceTemporary)
            {
                converter.Parameter.SourceConvertFileName = CreateTempFileName(converter.Parameter.SourceTempDirectory, converter.Parameter.SourceFileName);
            }
            if (converter.Parameter.OriginalFileName == null)
            {
                converter.Parameter.OriginalFileName = converter.Parameter.SourceConvertFileName;
            }

            // 変換先の一時パスを決定
            converter.Parameter.DestConvertFileName = converter.Parameter.DestFileName;
            if (converter.Parameter.UseDestTemporary)
            {
                converter.Parameter.DestConvertFileName = CreateTempFileName(converter.Parameter.DestTempDirectory, converter.Parameter.DestFileName);
            }

            // 変換元ファイルを一時パスへコピー
            if (converter.Parameter.UseSourceTemporary)
            {
                File.Copy(converter.Parameter.SourceFileName, converter.Parameter.SourceConvertFileName, true);
            }

            try
            {
                var result = new ConvertCompleteEventArgs();
                result.Result = ConvertResultType.Succeed;

                // 変換実行
                converter.ConvertFile(converter.Parameter);
                converter.CopyDestFile(converter.Parameter);
                converter.ConvertSucceed?.Invoke(converter.Parameter);

                // 反応変換の実行
                if (converter.ReactiveConvert != null)
                {
                    foreach (var reactive in converter.ReactiveConvert)
                    {
                        reactive.Parent = converter;
                        reactive.Parameter.OriginalFileName = converter.Parameter.OriginalFileName;
                        switch (reactive.Parameter.ReactiveTarget)
                        {
                            case ReactiveFileTarget.Original:
                                reactive.Parameter.SourceFileName = converter.Parameter.OriginalFileName;
                                break;
                            case ReactiveFileTarget.RecentOriginal:
                                reactive.Parameter.SourceFileName = converter.Parameter.SourceConvertFileName;
                                break;
                            case ReactiveFileTarget.RecentConvert:
                                reactive.Parameter.SourceFileName = converter.Parameter.DestConvertFileName;
                                break;
                            case ReactiveFileTarget.RecentDest:
                                reactive.Parameter.SourceFileName = converter.Parameter.DestFileName;
                                break;
                        }
                        try
                        {
                            var reactiveResult = ExecuteConvert(reactive, true);

                            // 異常が発生した場合は変換を強制終了する
                            if (reactiveResult.Result == ConvertResultType.Failed)
                            {
                                result = reactiveResult;
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            // 異常が発生した場合は変換を強制終了する
                            result.Result = ConvertResultType.Failed;
                            result.Error = ex;
                            break;
                        }
                    }
                }

                converter.DeleteTemporary(converter.Parameter);
                return result;
            }
            catch (Exception ex)
            {
                converter.DeleteTemporary(converter.Parameter);
                converter.ConvertFailed?.Invoke(converter.Parameter, ex);
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
        /// <param name="parameter">変換パラメーター。</param>
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

        /// <summary>
        /// 変換先の一時ディレクトリを利用している場合、変換したファイルを変換先へコピーする。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        private void CopyDestFile(FileConvertParameter parameter)
        {
            // 変換先として一時ディレクトリを利用していない場合は何もしない
            if (!parameter.UseDestTemporary)
            {
                return;
            }

            // 変換したファイルをスルーする場合は何もしない
            if (parameter.DestThrough)
            {
                return;
            }

            CopySucceedDestFile(parameter);
        }

        /// <summary>
        /// 変換先の一時ディレクトリを利用している場合、変換したファイルを変換先へコピーします。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        protected virtual void CopySucceedDestFile(FileConvertParameter parameter)
        {
            var fileExists = File.Exists(parameter.DestFileName);

            // 上書きでない時、変換後に変換先ファイルパスに同名のファイルがある場合
            if (!parameter.Overwrite && fileExists)
            {
                throw new ArgumentException(ExceptionResources.GetString("ExistsDestFilePath"));
            }

            // 変換後ファイルパスへコピー
            var directory = Path.GetDirectoryName(parameter.DestFileName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.Copy(parameter.DestConvertFileName, parameter.DestFileName);
        }

        /// <summary>
        /// 変換元、変換先の一時ディレクトリを利用している場合、一時ファイルを削除する。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        private void DeleteTemporary(FileConvertParameter parameter)
        {
            // 変換元として一時ディレクトリを利用していない場合は何もしない
            if (parameter.UseSourceTemporary)
            {
                DeleteFile(parameter.SourceConvertFileName);
            }

            // 変換元として一時ディレクトリを利用していない場合は何もしない
            if (parameter.UseDestTemporary)
            {
                DeleteFile(parameter.DestConvertFileName);
            }
        }

        /// <summary>
        /// ファイルの削除を行う。
        /// </summary>
        /// <param name="fileName">ファイルパス。</param>
        private void DeleteFile(string fileName)
        {
            File.Delete(fileName);

            // ディレクトリが空になった場合はディレクトリも削除する
            var tempDirectory = Path.GetDirectoryName(fileName);
            if (Directory.GetFiles(tempDirectory).Length == 0)
            {
                Directory.Delete(tempDirectory);
            }
        }
    }
}
