using System;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// プロパティの既定値制御コントローラーを提供します。
    /// </summary>
    internal class PropertyDefaultController
    {
        /// <summary>
        /// プロパティを決定づけるキー値を取得します。
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// プロパティ値が既定値から変更されたかどうかを取得するファンクションを取得します。
        /// </summary>
        public Func<bool> ShouldSerialize { get; }

        /// <summary>
        /// プロパティ値をリセットするアクションを取得します。
        /// </summary>
        public Action Reset { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="key">プロパティキー。</param>
        /// <param name="shouldSerialize">プロパティ値が既定値から変更されたかどうかを取得するファンクション。</param>
        /// <param name="reset">プロパティ値をリセットするアクション。</param>
        public PropertyDefaultController(string key, Func<bool> shouldSerialize, Action reset)
        {
            Key = key;
            ShouldSerialize = shouldSerialize;
            Reset = reset;
        }
    }
}
