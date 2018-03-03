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
            foreach (var c in value) {
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
    }
}
