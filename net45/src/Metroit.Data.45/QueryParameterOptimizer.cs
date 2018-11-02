using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Metroit.Data
{
    /// <summary>
    /// クエリパラメータの最適化を提供します。
    /// </summary>
    public class QueryParameterOptimizer
    {
        private const string parameterPattern = @"--.*|/\*.*?\*/|'.*?'|(:\w+)|(@\w+)";
        private const string singleLineCommentPattern = @"--.*";

        private readonly Regex paraeterRegex;
        private readonly Regex multiLineRegex;

        /// <summary>
        /// MetDbParameterParser クラスの新しいインスタンスを初期化します。
        /// </summary>
        public QueryParameterOptimizer()
        {
            paraeterRegex = new Regex(parameterPattern, RegexOptions.Compiled | RegexOptions.Singleline);
            multiLineRegex = new Regex(singleLineCommentPattern, RegexOptions.Compiled);
        }

        /// <summary>
        /// バインド変数タイプに応じて、クエリのパラメータ文字を最適化します。
        /// </summary>
        /// <param name="query">クエリ文字列。</param>
        /// <param name="bindVariableType">MetDbBindVariableType 列挙体。</param>
        /// <returns>最適化後のクエリ</returns>
        public string GetOptimizedText(string query, QueryBindVariableType bindVariableType)
        {
            string result;
            switch (bindVariableType)
            {
                case QueryBindVariableType.AtmarkWithParam:
                    result = optimize(query, "@", true);
                    break;
                case QueryBindVariableType.ColonWithParam:
                    result = optimize(query, ":", true);
                    break;
                case QueryBindVariableType.Question:
                    result = optimize(query, "?", false);
                    break;
                default:
                    result = query;
                    break;
            }
            return result;
        }

        /// <summary>
        /// クエリをパターン検索して、パラメータ部を最適化する。
        /// </summary>
        /// <param name="query">クエリ文字列。</param>
        /// <param name="sign">バインド変数タイプに応じた記号。</param>
        /// <param name="isPrefix">記号を接頭辞として扱うかどうか。</param>
        /// <returns>クエリ</returns>
        private string optimize(string query, string sign, bool isPrefix)
        {
            var text = new StringBuilder(query);
            var startIndex = 0;
            while (true)
            {
                var match = paraeterRegex.Match(text.ToString(), startIndex);
                if (!match.Success)
                {
                    break;
                }

                // 単一行コメントの場合、複数行正規表現で次の探査位置を求める
                if (match.Value.Length >= 2 && match.Value.Substring(0, 2) == "--")
                {
                    var match2 = multiLineRegex.Match(text.ToString(), startIndex);
                    startIndex = match2.Index + match2.Length;
                    continue;
                }

                // 文字列または複数行コメントの場合、次の探査位置を求める
                if (match.Value.IndexOf("'") >= 0 || (match.Value.Length >= 2 && match.Value.Substring(0, 2) == "/*"))
                {
                    startIndex = match.Index + match.Length;
                    continue;
                }

                // パラメータの記号化・プリフィックスを最適化
                string parameterName = sign;
                if (isPrefix)
                {
                    parameterName = sign + match.Value.Substring(1);
                }
                text.Replace(match.Value, parameterName, match.Index, match.Length);
                startIndex = match.Index + parameterName.Length;
            }

            return text.ToString();
        }
    }
}
