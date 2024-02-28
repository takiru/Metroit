using System.ComponentModel;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// キャンセルできるイベントと値変更時の過程のデータを提供します。
    /// </summary>
    public class ValueChangeValidatingEventArgs : CancelEventArgs
    {
        private decimal? input = null;
        private decimal? before = null;
        private decimal? after = null;

        /// <summary>
        /// 入力された値です。
        /// </summary>
        public decimal? Input
        {
            get { return this.input; }
            internal set { this.input = value; }
        }

        /// <summary>
        /// 入力値を許可する前の値です。
        /// </summary>
        public decimal? Before
        {
            get { return this.before; }
            internal set { this.before = value; }
        }

        /// <summary>
        /// 入力値を許可した時の値です。
        /// </summary>
        public decimal? After
        {
            get { return this.after; }
            internal set { this.after = value; }
        }
    }
}
