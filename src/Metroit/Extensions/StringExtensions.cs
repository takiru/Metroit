using System;
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
    }
}
