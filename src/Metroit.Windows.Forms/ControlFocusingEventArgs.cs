using System.ComponentModel;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// コントロールがフォーカスされる直前のデータを提供します。
    /// </summary>
    public class ControlFocusingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// 選択したアイテムを取得します。
        /// </summary>
        public object Current { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="current">フォーカスを所持しているコントロール。</param>
        public ControlFocusingEventArgs(Control current) : base(false)
        {
            Current = current;
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="current">フォーカスを所持しているコントロール。</param>
        /// <param name="cancel">キャンセルするかどうか。</param>
        public ControlFocusingEventArgs(Control current, bool cancel) : this(current)
        {
            Cancel = cancel;
        }
    }
}
