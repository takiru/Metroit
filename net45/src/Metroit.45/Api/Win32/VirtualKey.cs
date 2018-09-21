using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroit.Api.Win32
{
    /// <summary>
    /// 仮想キーコードを提供します。
    /// </summary>
    public static class VirtualKey
    {
        /// <summary>
        /// 上キーを定義します。
        /// </summary>
        public const int VK_UP = 0x26;

        /// <summary>
        /// 下キーを定義します。
        /// </summary>
        public const int VK_DOWN = 0x28;

        /// <summary>
        /// ESCキーを定義します。
        /// </summary>
        public const int VK_ESCAPE = 0x1B;

        /// <summary>
        /// Enterキーを定義します。
        /// </summary>
        public const int VK_RETURN = 0x0D;

        /// <summary>
        /// BackSpaceキーを定義します。
        /// </summary>
        public const int VK_BACK = 0x08;

        /// <summary>
        /// Deleteキーを定義します。
        /// </summary>
        public const int VK_DELETE = 0x2E;
    }
}
