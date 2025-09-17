using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metroit.Text
{
    /// <summary>
    /// ASCII文字のコンバーターを提供します。
    /// </summary>
    public class AsciiConverter
    {
        /// <summary>
        /// マップ情報。
        /// </summary>
        private static readonly Dictionary<char, char> Map = new Dictionary<char, char>();

        /// <summary>
        /// 半角バックスラッシュ。
        /// </summary>
        private static readonly char HalfBackslashChar = (char)0x5c;

        /// <summary>
        /// 全角バックスラッシュ。
        /// </summary>
        private static readonly char FullBackslashChar = (char)0xFF3C;

        /// <summary>
        /// 全角￥。
        /// </summary>
        private static readonly char FullYenChar = (char)0xFFE5;

        /// <summary>
        /// 静的コンストラクタによってマップ情報を構成する。
        /// </summary>
        static AsciiConverter()
        {
            // 半角スペース
            Map[(char)0x3000] = (char)0x20;

            // 半角スペース, \ 以外のASCII文字
            for (int i = 0x21; i <= 0x7E; i++)
            {
                Map[(char)(i + 0xFEE0)] = (char)i;
            }
        }

        /// <summary>
        /// <paramref name="text"/> に含まれる半角ASCII文字を全角文字に変換します。
        /// </summary>
        /// <param name="text">変換する文字列。</param>
        /// <param name="backslashToYen">バックスラッシュを全角￥に変換するかどうか。</param>
        /// <returns>半角ASCII文字を全角文字に変換した文字列。</returns>
        public static string ToFullWidth(string text, bool backslashToYen = false)
        {
            RemapBackslash(backslashToYen);

            var result = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                if (Map.ContainsValue(text[i]))
                {
                    // NOTE: バックスラッシュには2種類の全角文字(＼, ￥)があり得て、￥ の方が文字コードが後続に位置するため、Last() で取得する。
                    result.Append(Map
                        .Where(x => x.Value == text[i])
                        .Select(x => x.Key)
                        .Last());
                    continue;
                }

                result.Append(text[i]);
            }

            return result.ToString();
        }

        /// <summary>
        /// <paramref name="text"/> に含まれる全角ASCII文字を半角文字に変換します。
        /// </summary>
        /// <param name="text">変換する文字列。</param>
        /// <param name="yenToBackslash">全角￥をバックスラッシュに変換するかどうか。</param>
        /// <returns>全角ASCII文字を半角文字に変換した文字列。</returns>
        public static string ToHalfWidth(string text, bool yenToBackslash = false)
        {
            RemapBackslash(yenToBackslash);

            var result = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                if (Map.ContainsKey(text[i]))
                {
                    result.Append(Map[text[i]]);
                    continue;
                }

                result.Append(text[i]);
            }

            return result.ToString();
        }

        /// <summary>
        /// バックスラッシュと半角\、全角￥のマップ情報を再マップする。
        /// </summary>
        /// <param name="mapFullYen">全角￥をマップするかどうか。</param>
        private static void RemapBackslash(bool mapFullYen)
        {
            Map.Remove(FullYenChar);
            Map.Remove(FullBackslashChar);

            if (mapFullYen)
            {
                Map.Add(FullYenChar, HalfBackslashChar);
            }
            Map.Add(FullBackslashChar, HalfBackslashChar);
        }
    }
}