using System;
using System.Threading.Tasks;
using System.ComponentModel;

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
        public Action<T, ConvertCompleteEventArgs> Complete { get; set; } = null;

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <returns>変換結果。</returns>
        public ConvertResultType Convert()
        {
            return Convert(Parameter);
        }

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        /// <returns>変換結果。</returns>
        public ConvertResultType Convert(T parameter)
        {
            Parameter = parameter;

            var cea = new CancelEventArgs();
            cea.Cancel = false;
            Prepare?.Invoke(parameter, cea);
            if (cea.Cancel)
            {
                var r = new ConvertResultType();
                r = ConvertResultType.Cancelled;
                var ccea = new ConvertCompleteEventArgs();
                ccea.Result = ConvertResultType.Cancelled;
                Complete?.Invoke(parameter, ccea);
                return r;
            }

            try
            {
                var result = new ConvertCompleteEventArgs();
                DoConvert(parameter);
                result.Result = ConvertResultType.Succeed;
                result.Error = null;
                Complete?.Invoke(parameter, result);
                return result.Result;

            }
            catch (Exception e)
            {
                var result = new ConvertCompleteEventArgs();
                result.Result = ConvertResultType.Failed;
                result.Error = e;
                Complete?.Invoke(parameter, result);
                return result.Result;
            }
        }

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <returns>ConvertResult のタスク。</returns>
        public async Task<ConvertResultType> ConvertAsync()
        {
            var task = await Task.Run(() =>
            {
                return Convert(Parameter);
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
                 return Convert(parameter);
             });

            return task;
        }

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター</param>
        protected abstract void DoConvert(T parameter);
    }
}
