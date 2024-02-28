using System.ComponentModel;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// キャンセルできるイベントと値変更時の過程のデータを提供します。
    /// </summary>
    public class TextChangeValidationEventArgs : CancelEventArgs
    {
        private string input = "";
        private string before = "";
        private string after = "";

        /// <summary>
        /// 入力された値です。
        /// </summary>
        public string Input
        {
            get { return this.input; }
            internal set { this.input = value; }
        }

        /// <summary>
        /// 入力値を許可する前の値です。
        /// </summary>
        public string Before
        {
            get { return this.before; }
            internal set { this.before = value; }
        }

        /// <summary>
        /// 入力値を許可した時の値です。
        /// </summary>
        public string After
        {
            get { return this.after; }
            internal set { this.after = value; }
        }
    }
}
