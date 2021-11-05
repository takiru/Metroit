using System;
using System.Collections.Generic;
using System.Text;
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
        bool IsRollbacked { get; }

        /// <summary>
        /// ロールバックを実施します。
        /// </summary>
        void Rollback();
    }
}
