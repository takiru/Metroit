using System;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 許可する文字を定義します。
    /// </summary>
    [Serializable]
    [Flags]
    public enum AcceptsCharType
    {
        /// <summary>
        /// 全ての文字を示します。
        /// </summary>
        All = 1,
        /// <summary>
        /// 半角数字を示します。
        /// </summary>
        HalfNumeric = 2,
        /// <summary>
        /// 全角数字を示します。
        /// </summary>
        FullNumeric = 4,
        /// <summary>
        /// 半角英字を示します。
        /// </summary>
        HalfAlpha = 16,
        /// <summary>
        /// 全角英字を示します。
        /// </summary>
        FullAlpha = 32,
        /// <summary>
        /// 半角記号を示します。
        /// </summary>
        HalfSign = 64,
        /// <summary>
        /// 全角記号を示します。
        /// </summary>
        FullSign = 128,
        /// <summary>
        /// カスタムを示します。
        /// </summary>
        Custom = 256
    }
}
