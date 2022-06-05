using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 描画を行う線の位置を定義します。
    /// </summary>
    public enum LinePosition
    {
        /// <summary>
        /// 線を描画しません。
        /// </summary>
        None,

        /// <summary>
        /// 上線を描画します。
        /// </summary>
        Overline,

        /// <summary>
        /// 下線を描画します。
        /// </summary>
        Underline
    }
}
