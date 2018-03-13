using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// コントロールのロールバックを提供します。
    /// </summary>
    public interface IControlRollback
    {
        /// <summary>
        /// ロールバック済みかどうかを取得します。
        /// </summary>
        /// <param name="sender">指示したオブジェクト。</param>
        /// <param name="control">コントロール。</param>
        /// <returns></returns>
        bool IsRollbacked(object sender, Control control);

        /// <summary>
        /// ロールバックを実施します。
        /// </summary>
        /// <param name="sender">指示したオブジェクト。</param>
        /// <param name="control">コントロール。</param>
        void Rollback(object sender, Control control);
    }
}
