using System;

namespace Metroit.IO
{
    /// <summary>
    /// 変換処理完成時のイベントのデータを提供します。
    /// </summary>
    public class ConvertCompleteEventArgs
    {
        /// <summary>
        /// タスク内で発生した例外情報を取得します。
        /// </summary>
        public Exception Error { get; internal set; }

        /// <summary>
        /// 変換結果を取得します。
        /// </summary>
        public ConvertResultType Result { get; internal set; }
    }
}
