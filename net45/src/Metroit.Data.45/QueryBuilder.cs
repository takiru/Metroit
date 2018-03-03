using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Metroit.Data
{
    /// <summary>
    /// クエリ文字列の編集を提供します。
    /// </summary>
    public class QueryBuilder
    {
        /// <summary>
        /// MetQueryBuilder クラスの新しいインスタンスを初期化します。
        /// </summary>
        public QueryBuilder() { }

        /// <summary>
        /// 新しい MetQueryBuilder のインスタンスを生成します。
        /// </summary>
        /// <param name="query">クエリ文字列。</param>
        public QueryBuilder(string query)
        {
            this.Query = query;
        }

        /// <summary>
        /// クエリ文字列の取得または設定します。
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// /* key */ で差し替える設定値の値を取得または設定します。
        /// </summary>
        public Dictionary<string, string> ReplaceQueries { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 末尾に追加するクエリ文字列を取得または設定します。
        /// </summary>
        public List<string> AddQueries { get; set; } = new List<string>();

        /// <summary>
        /// 先頭に挿入するクエリ文字列を取得または設定します。
        /// </summary>
        public List<string> InsertQueries { get; set; } = new List<string>();

        /// <summary>
        /// 設定したクエリ情報を展開し、クエリを生成します。
        /// </summary>
        /// <returns>クエリ文字列</returns>
        public string Build()
        {
            StringBuilder query = new StringBuilder();

            // 挿入クエリ
            foreach (var insert in this.InsertQueries)
            {
                query.Append(insert + " ");
            }

            query.Append(this.Query);

            // 追加クエリ
            foreach (var add in this.AddQueries)
            {
                query.Append(" " + add);
            }

            // 差し替えクエリ
            foreach (KeyValuePair<string, string> kvp in this.ReplaceQueries)
            {
                var matches = Regex.Matches(query.ToString(), @"/\*[\s]*" + kvp.Key + @"[\s]*\*/");
                foreach (Match match in matches)
                {
                    query.Replace(match.Value, kvp.Value);
                }
            }

            return query.ToString();
        }

        /// <summary>
        /// 差し替え設定した情報を展開し、クエリ文字列を生成します。
        /// </summary>
        /// <param name="query">クエリ文字列</param>
        /// <param name="replaceQueries">/* key */ で差し替える設定値</param>
        /// <returns>クエリ文字列</returns>
        public static string Build(string query, Dictionary<string, string> replaceQueries = null)
        {
            var queryBuilder = new QueryBuilder(query);
            if (replaceQueries != null)
            {
                queryBuilder.ReplaceQueries = replaceQueries;
            }

            return queryBuilder.Build();
        }
    }
}
