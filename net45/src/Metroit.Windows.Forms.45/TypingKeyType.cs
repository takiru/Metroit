using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 押されたキー種類を定義します。
    /// </summary>
    internal enum TypingKeyType
    {
        /// <summary>
        /// 通常キー。
        /// </summary>
        DefaultKeys,
        /// <summary>
        /// Deleteキー。
        /// </summary>
        DeleteKey,
        /// <summary>
        /// Ctrl+X, SelectionLength > 0 時のShift+Delete による切り取り。
        /// </summary>
        Cut
    }
}
