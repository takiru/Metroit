namespace Metroit.Text
{
    /// <summary>
    /// テキストのコンバーターを提供します。
    /// </summary>
    public static class TextConverter
    {
        /// <summary>
        /// <paramref name="text"/> に含まれる半角文字列を全角文字列に変換します。
        /// </summary>
        /// <param name="text">変換する文字列。</param>
        /// <param name="asciiConvert"><paramref name="text"/> に含まれる半角ASCII文字を全角文字に変換するかどうか。</param>
        /// <param name="katakanaConvert"><paramref name="text"/> に含まれる半角カタカナを全角カタカナに変換するかどうか。</param>
        /// <param name="backslashToYen">バックスラッシュを全角￥に変換するかどうか。</param>
        /// <returns>半角文字列を全角文字列に変換した文字列。</returns>
        public static string ToFullWidth(string text, bool asciiConvert, bool katakanaConvert, bool backslashToYen = false)
        {
            var covertedAsciiText = text;
            if (asciiConvert)
            {
                covertedAsciiText = AsciiConverter.ToFullWidth(text, backslashToYen);
            }

            var convertedKatakanaText = covertedAsciiText;
            if (katakanaConvert)
            {
                convertedKatakanaText = KatakanaConverter.ToFullWidth(covertedAsciiText);
            }

            return convertedKatakanaText;
        }

        /// <summary>
        /// <paramref name="text"/> に含まれる全角文字列を半角文字列に変換します。
        /// </summary>
        /// <param name="text">変換する文字列。</param>
        /// <param name="asciiConvert"><paramref name="text"/> に含まれる半角ASCII文字を全角文字に変換するかどうか。</param>
        /// <param name="katakanaConvert"><paramref name="text"/> に含まれる半角カタカナを全角カタカナに変換するかどうか。</param>
        /// <param name="yenToBackslash">全角￥をバックスラッシュに変換するかどうか。</param>
        /// <returns>全角文字列を半角文字列に変換した文字列。</returns>
        public static string ToHalfWidth(string text, bool asciiConvert, bool katakanaConvert, bool yenToBackslash = false)
        {
            var covertedAsciiText = text;
            if (asciiConvert)
            {
                covertedAsciiText = AsciiConverter.ToHalfWidth(text, yenToBackslash);
            }

            var convertedKatakanaText = covertedAsciiText;
            if (katakanaConvert)
            {
                convertedKatakanaText = KatakanaConverter.ToHalfWidth(covertedAsciiText);
            }

            return convertedKatakanaText;
        }
    }
}