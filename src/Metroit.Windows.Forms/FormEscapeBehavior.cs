using System.ComponentModel;
using System.Text;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// ESCキーが押された時の振る舞いを扱います。
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FormEscapeBehavior
    {
        /// <summary>
        /// このフォームまたはそれぞれのコントロールにILeaveRollback インターフェースを実装している時、コントロールの情報をロールバックします。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [MetDescription("ControlRollback")]
        public bool ControlRollback { get; set; } = false;

        /// <summary>
        /// コントロールを非アクティブにします。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [MetDescription("ControlLeave")]
        public bool ControlLeave { get; set; } = false;

        /// <summary>
        /// フォームを閉じます。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [MetDescription("ControlFormClose")]
        public bool FormClose { get; set; } = false;

        /// <summary>
        /// 有効になっている要素を文字列で返却します。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (ControlRollback)
            {
                sb.Append("ControlRollback,");
            }
            if (ControlLeave)
            {
                sb.Append("ControlLeave,");
            }
            if (FormClose)
            {
                sb.Append("FormClose,");
            }
            if (sb.Length == 0)
            {
                sb.Append("(None)");
            }
            else
            {
                sb.Length -= 1;
            }
            return sb.ToString();
        }
    }
}
