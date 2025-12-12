using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Metroit.Extensions
{
    /// <summary>
    /// String クラスの拡張メソッドを提供します。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 区切り文字判断から単語を区切り、区切り文字を挿入します。
        /// </summary>
        /// <param name="value">文字列</param>
        /// <param name="separateChar">区切り文字</param>
        /// <param name="type">区切り文字判断</param>
        /// <returns>区切り文字を挿入した文字列</returns>
        public static string InsertSeparator(this string value, string separateChar, SeparateJudgeType type = SeparateJudgeType.UpperChar)
        {
            var sb = new StringBuilder();
            var i = 0;
            foreach (var c in value)
            {
                if (i > 0)
                {
                    if ((type == SeparateJudgeType.UpperChar && char.IsUpper(c)) ||
                        (type == SeparateJudgeType.LowerChar && char.IsLower(c)))
                    {
                        sb.Append(separateChar);
                    }
                }
                sb.Append(c);
                i++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// <para>囲み文字内の文字列を取得します。</para>
        /// <para>囲み文字の開始、終了がひもづかない場合は取得できません。</para>
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="startBracket">囲み文字の開始文字。</param>
        /// <param name="endBracket">囲み文字の終了文字。</param>
        /// <param name="includeBracket">結果に囲み文字を含めるかどうか。</param>
        /// <returns>囲み文字内の文字列。取得できない場合は空。</returns>
        public static string GetEnclosedText(this string value, string startBracket, string endBracket, bool includeBracket = false)
        {
            var started = false;
            var openCount = 0;
            var startIndex = -1;

            // 指定ブラケットに一致するかどうか
            var sb = new StringBuilder();
            Func<int, string, bool> IsMatchBracket = delegate (int index, string targetBracket)
            {
                sb.Clear();
                for (var i = 0; i < targetBracket.Length; i++)
                {
                    sb.Append(value[index + i]);
                }
                var bracket = sb.ToString();

                if (bracket == targetBracket)
                {
                    return true;
                }
                return false;
            };

            // 開始ブラケット処理
            Action<int> startBracketAction = delegate (int index)
            {
                // 文字長を超えるならブラケット処理なし
                if (value.Length - 1 < index + startBracket.Length - 1)
                {
                    return;
                }

                // ブラケットに合致しなければ処理なし
                if (!IsMatchBracket(index, startBracket))
                {
                    return;
                }

                openCount++;
                if (!started)
                {
                    startIndex = index;
                }
                started = true;
            };

            // 終了ブラケット処理
            Action<int> endBracketAction = delegate (int index)
            {
                // 文字長を超えるならブラケット処理なし
                if (value.Length - 1 < index + endBracket.Length - 1)
                {
                    return;
                }

                // ブラケットに合致しなければ処理なし
                if (!IsMatchBracket(index, endBracket))
                {
                    return;
                }
                openCount--;
            };

            for (var index = 0; index < value.Length; index++)
            {
                startBracketAction(index);
                endBracketAction(index);

                if (!started || openCount > 0)
                {
                    continue;
                }

                index += endBracket.Length - 1;
                if (!includeBracket)
                {
                    startIndex += startBracket.Length;
                    index -= endBracket.Length;
                }
                return value.Substring(startIndex, index + 1 - startIndex);
            }
            return value;
        }

        /// <summary>
        /// <para>囲み文字内の文字列を取得します。</para>
        /// <para>囲み文字の開始、終了がひもづかない場合は取得できません。</para>
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="includeBracket">結果に囲み文字を含めるかどうか。</param>
        /// <param name="startBracket">囲み文字の開始文字。</param>
        /// <param name="endBracket">囲み文字の終了文字。</param>
        /// <returns>囲み文字内の文字列。取得できない場合は空。</returns>
        public static string GetEnclosedText(this string value, bool includeBracket = false, char startBracket = '(', char endBracket = ')')
        {
            return GetEnclosedText(value, startBracket.ToString(), endBracket.ToString(), includeBracket);
        }

        /// <summary>
        /// 文字列の長さを取得します。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="fullWidthCharTwo">全角文字を2文字としてカウントするかどうか。</param>
        /// <returns>
        /// 文字列の長さを返却します。<br/>
        /// <paramref name="fullWidthCharTwo"/> が <see langword="true"/> のときは、全角文字を2文字としてカウントした文字列の長さを返却します。<br/>
        /// <see langword="false"/>のときは、<paramref name="value"/>の<see cref="string.Length"/>を返却します。
        /// </returns>
        public static int GetTextCount(this string value, bool fullWidthCharTwo)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            // 全角文字を2文字としてカウントしないなら素の文字列長を返却
            if (!fullWidthCharTwo)
            {
                return value.Length;
            }

            // StringInfoで書記素クラスタ（表示される1文字）単位で処理
            int count = 0;
            StringInfo stringInfo = new StringInfo(value);

            for (int i = 0; i < stringInfo.LengthInTextElements; i++)
            {
                string textElement = stringInfo.SubstringByTextElements(i, 1);

                if (IsFullWidth(textElement))
                {
                    count += 2;  // 全角1文字 = 2カウント
                }
                else
                {
                    // 半角の場合、実際のchar数をカウント
                    count += textElement.Length;
                }
            }

            return count;
        }

        /// <summary>
        /// 1文字が全角文字かどうかを判定します。
        /// </summary>
        /// <param name="value">1文字の文字列。</param>
        /// <returns>
        /// 全角文字の場合は <see langword="true"/>, それ以外の場合は <see langword="false"/> を返却します。<br/>
        /// <paramref name="value"/> が 1文字でないときは <see langword="false"/> を返却します。<br/>
        /// 視覚的に2文字以上で構成される文字列をパラメーターとしたときは<see langword="false"/>を返却します。
        /// </returns>
        public static bool IsFullWidth(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            // StringInfoで1文字（書記素クラスタ）かどうかを確認
            StringInfo stringInfo = new StringInfo(value);
            if (stringInfo.LengthInTextElements != 1)
            {
                return false;  // 複数文字は判定しない
            }

            // 半角カナを含むかチェック
            if (value.Any(c => c >= 0xFF61 && c <= 0xFF9F))
            {
                return false;  // 半角カナ = 半角
            }

            // ゼロ幅結合子が含まれている場合は絵文字結合なので全角
            if (value.Contains('\u200D'))
            {
                return true;
            }

            // 制御文字や結合文字を除いた最初の実文字を取得して判定
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];

                // 制御文字や結合文字をスキップ
                if (c == 0x200D ||  // ゼロ幅結合子
                    (c >= 0xFE00 && c <= 0xFE0F) ||  // 異体字セレクタ
                    (c >= 0x0300 && c <= 0x036F))    // 結合発音記号
                {
                    continue;
                }

                // サロゲートペアの場合
                if (char.IsHighSurrogate(c) && i + 1 < value.Length && char.IsLowSurrogate(value[i + 1]))
                {
                    // サロゲートペアは基本的に全角
                    return true;
                }

                // 通常の文字（BMP内）
                return IsFullWidthBMPChar(c);
            }

            return false;
        }

        /// <summary>
        /// BMP内の文字が全角文字かどうかを判定します。
        /// </summary>
        /// <param name="c">文字。</param>
        /// <returns>全角文字の場合は <see langword="true"/>, それ以外の場合は <see langword="false"/> を返却します。</returns>
        private static bool IsFullWidthBMPChar(char c)
        {
            // ASCII範囲（0x00-0x7F）は半角
            if (c <= 0x7F)
            {
                return false;
            }

            // 半角カタカナの範囲を除外（FF61-FF9F）
            if (c >= 0xFF61 && c <= 0xFF9F)
            {
                return false;
            }

            // それ以外はすべて全角扱い
            return true;
        }
    }
}
