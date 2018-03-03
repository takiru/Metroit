using System;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Metroit.IO
{
    /// <summary>
    /// データの変換を表します。
    /// </summary>
    public abstract class ConverterBase
    {
        /// <summary>
        /// 実行が完了したイベントを処理するメソッドを表します。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        /// <param name="e">実行完了イベント。</param>
        public delegate void ConvertCompleteEventHandler(IConvertParameter parameter, ConvertCompleteEventArgs e);

        /// <summary>
        /// 変換準備のイベントを処理するメソッドを表します。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        /// <param name="e">キャンセルイベント。</param>
        public delegate void ConvertPrepareEventHandler(IConvertParameter parameter, CancelEventArgs e);

        /// <summary>
        /// 変換準備が行われたときに発生します。
        /// </summary>
        public event ConvertPrepareEventHandler Prepare;

        /// <summary>
        /// 変換処理が完了したときに発生します。
        /// </summary>
        public event ConvertCompleteEventHandler ConvertCompleted;

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        public ConvertResultType Convert(IConvertParameter parameter)
        {
            var cea = new CancelEventArgs();
            cea.Cancel = false;
            Prepare?.Invoke(parameter, cea);
            if (cea.Cancel)
            {
                var r = new ConvertResultType();
                r = ConvertResultType.Cancelled;
                var ccea = new ConvertCompleteEventArgs();
                ccea.Result = ConvertResultType.Cancelled;
                ConvertCompleted?.Invoke(parameter, ccea);
                return r;
            }

            try
            {
                return onConvert(parameter);

            }
            catch (Exception e)
            {
                var result = new ConvertCompleteEventArgs();
                result.Result = ConvertResultType.Failed;
                result.Error = e;
                ConvertCompleted?.Invoke(parameter, result);
                return result.Result;
            }
        }

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        /// <returns>ConvertResult のタスク。</returns>
        public async Task<ConvertResultType> ConvertAsync(IConvertParameter parameter)
        {
            var e = new CancelEventArgs();
            e.Cancel = false;
            Prepare?.Invoke(parameter, e);
            if (e.Cancel)
            {
                var r = new ConvertResultType();
                r = ConvertResultType.Cancelled;
                var cea = new ConvertCompleteEventArgs();
                cea.Result = ConvertResultType.Cancelled;
                ConvertCompleted?.Invoke(parameter, cea);
                return r;
            }

            var result = await onConvertTask(parameter);

            return result;
        }

        /// <summary>
        /// 変換を行います。
        /// </summary>
        /// <param name="parameter">変換パラメーター</param>
        protected abstract void DoConvert(IConvertParameter parameter);

        /// <summary>
        /// 非同期で変換を行うためのタスクメソッド。
        /// </summary>
        /// <param name="parameter">変換パラメーター。</param>
        /// <returns>ConvertResult のタスク。</returns>
        private Task<ConvertResultType> onConvertTask(IConvertParameter parameter)
        {
            var task = new Task<ConvertResultType>(() =>
            {
                try
                {
                    return onConvert(parameter);
                }
                catch (Exception e)
                {
                    var result = new ConvertCompleteEventArgs();
                    result.Result = ConvertResultType.Failed;
                    result.Error = e;
                    ConvertCompleted?.Invoke(parameter, result);
                    return result.Result;
                }
            });
            task.Start();
            return task;

        }

        /// <summary>
        /// 変換の呼び出しを実施する。
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private ConvertResultType onConvert(IConvertParameter parameter)
        {
            var result = new ConvertCompleteEventArgs();

            // 変換処理
            DoConvert(parameter);

            result.Result = ConvertResultType.Succeed;
            result.Error = null;
            ConvertCompleted?.Invoke(parameter, result);
            return result.Result;
        }
    }
}
