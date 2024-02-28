using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Metroit.IO
{
    /// <summary>
    /// データの変換を表します。
    /// </summary>
    public abstract class ConverterBase<T> where T : IConvertParameter, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public T Parameter { get; set; } = new T();

        /// <summary>
        /// 変換準備を行います。
        /// </summary>
        public Action<T, CancelEventArgs> Prepare { get; set; } = null;

        /// <summary>
        /// 変換処理が完了したときに発生します。
        /// </summary>
        public Action<T, ConvertCompleteEventArgs> Completed { get; set; } = null;

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <returns>変換結果。</returns>
        public ConvertResultType Convert()
        {
            return Convert(Parameter, CancellationToken.None);
        }

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        /// <returns>変換結果。</returns>
        public ConvertResultType Convert(T parameter)
        {
            return Convert(parameter, CancellationToken.None);
        }

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <returns>ConvertResult のタスク。</returns>
        public async Task<ConvertResultType> ConvertAsync()
        {
            var task = await Task.Run(() =>
            {
                return Convert(Parameter, CancellationToken.None);
            });

            return task;
        }

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <param name="token">キャンセルトークン。</param>
        /// <returns>ConvertResult のタスク。</returns>
        public async Task<ConvertResultType> ConvertAsync(CancellationToken token)
        {
            var task = await Task.Run(() =>
            {
                return Convert(Parameter, token);
            });

            return task;
        }

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        /// <returns>ConvertResult のタスク。</returns>
        public async Task<ConvertResultType> ConvertAsync(T parameter)
        {
            var task = await Task.Run(() =>
             {
                 return Convert(parameter, CancellationToken.None);
             });

            return task;
        }

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        /// <param name="token">キャンセルトークン。</param>
        /// <returns>ConvertResult のタスク。</returns>
        public async Task<ConvertResultType> ConvertAsync(T parameter, CancellationToken token)
        {
            var task = await Task.Run(() =>
            {
                return Convert(parameter, token);
            });

            return task;
        }

        /// <summary>
        /// 変換を行う。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        /// <param name="token">キャンセルトークン。</param>
        /// <returns>変換結果。</returns>
        private ConvertResultType Convert(T parameter, CancellationToken token)
        {
            Parameter = parameter;

            var cea = new CancelEventArgs();
            cea.Cancel = false;
            Prepare?.Invoke(parameter, cea);
            if (cea.Cancel)
            {
                var ccea = new ConvertCompleteEventArgs();
                ccea.Result = ConvertResultType.Cancelled;
                Completed?.Invoke(parameter, ccea);
                return ConvertResultType.Cancelled;
            }

            try
            {
                token.ThrowIfCancellationRequested();

                var result = DoConvert(parameter, token);
                Completed?.Invoke(parameter, result);
                return result.Result;

            }
            catch (OperationCanceledException ex)
            {
                var result = new ConvertCompleteEventArgs();
                result.Result = ConvertResultType.Cancelled;
                result.Error = ex;
                Completed?.Invoke(parameter, result);
                return result.Result;

            }
            catch (Exception ex)
            {
                var result = new ConvertCompleteEventArgs();
                result.Result = ConvertResultType.Failed;
                result.Error = ex;
                Completed?.Invoke(parameter, result);
                return result.Result;
            }
        }

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター</param>
        /// <param name="token"></param>
        protected abstract ConvertCompleteEventArgs DoConvert(T parameter, CancellationToken token);
    }
}
