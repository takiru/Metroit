using System;
using System.Collections.Generic;
using System.Text;

namespace Metroit.Api.Win32
{
    /// <summary>
    /// BitBlt に利用するラスター操作コード。
    /// </summary>
    public static class RasterOperation
    {
        /// <summary>
        /// ソースの長方形を宛先の長方形に直接コピーします。
        /// </summary>
        public const int SRCCOPY = 0x00CC0020;

        /// <summary>
        /// ブールOR演算子を使用して、ソース長方形と宛先長方形の色を組み合わせます。
        /// </summary>
        public const int SRCPAINT = 0x00EE0086;

        /// <summary>
        /// ブールAND演算子を使用して、ソース長方形と宛先長方形の色を組み合わせます。
        /// </summary>
        public const int SRCAND = 0x008800C6;

        /// <summary>
        /// ブールXOR演算子を使用して、ソース長方形と宛先長方形の色を組み合わせます。
        /// </summary>
        public const int SRCINVERT = 0x00660046;

        /// <summary>
        /// ブールAND演算子を使用して、宛先長方形の反転された色をソース長方形の色と組み合わせます。
        /// </summary>
        public const int SRCERASE = 0x00440328;

        /// <summary>
        /// 反転したソース長方形を宛先にコピーします。
        /// </summary>
        public const int NOTSRCCOPY = 0x00330008;

        /// <summary>
        /// ブールOR演算子を使用して、ソース長方形と宛先長方形の色を組み合わせてから、結果の色を反転します。
        /// </summary>
        public const int NOTSRCERASE = 0x001100A6;

        /// <summary>
        /// ブールAND演算子を使用して、hdcDestで 現在選択されているブラシとソース長方形の色をマージします。
        /// </summary>
        public const int MERGECOPY = 0x00C000CA;

        /// <summary>
        /// ブールOR演算子を使用して、反転したソース長方形の色をデスティネーション長方形の色とマージします。
        /// </summary>
        public const int MERGEPAINT = 0x00BB0226;

        /// <summary>
        /// hdcDestで 現在選択されているブラシを宛先ビットマップにコピーします。
        /// </summary>
        public const int PATCOPY = 0x00F00021;

        /// <summary>
        /// hdcDestで 現在選択されているブラシの色を、ブールOR演算子を使用して反転されたソース長方形の色と組み合わせます。この操作の結果は、ブールOR演算子を使用して、宛先の長方形の色と組み合わされます。
        /// </summary>
        public const int PATPAINT = 0x00FB0A09;

        /// <summary>
        /// ブールXOR演算子を使用して 、hdcDestで現在選択されているブラシの色を、宛先の長方形の色と組み合わせます。
        /// </summary>
        public const int PATINVERT = 0x005A0049;

        /// <summary>
        /// 宛先の長方形を反転します。
        /// </summary>
        public const int DSTINVERT = 0x00550009;

        /// <summary>
        /// 物理パレットのインデックス0に関連付けられた色を使用して、宛先の長方形を塗りつぶします。（この色は、デフォルトの物理パレットでは黒です。）
        /// </summary>
        public const int BLACKNESS = 0x00000042;

        /// <summary>
        /// 物理パレットのインデックス1に関連付けられた色を使用して、宛先の長方形を塗りつぶします。（この色は、デフォルトの物理パレットでは白です。）
        /// </summary>
        public const int WHITENESS = 0x00FF0062;

        /// <summary>
        /// 結果の画像でウィンドウの上にレイヤー化されているウィンドウが含まれます。デフォルトでは、画像にはウィンドウのみが含まれています。これは通常、デバイスコンテキストの印刷には使用できないことに注意してください。
        /// </summary>
        public const int CAPTUREBLT = 0x40000000;
    }
}
