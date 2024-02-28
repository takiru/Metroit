using System;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// コントロールに無効な値が設定されたときの発生する例外です。
    /// </summary>
    public class DeniedTextException : Exception
    {
        /// <summary>
        /// DeniedTextException クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外メッセージ。</param>
        /// <param name="text">例外が発生する元となったテキスト。</param>
        public DeniedTextException(string message, string text = null)
            : base(message)
        {
            InvalidText = text;
        }

        /// <summary>
        /// DeniedTextException クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外メッセージ。</param>
        /// <param name="innerException">内部例外オブジェクト。</param>
        /// <param name="text">例外が発生する元となったテキスト。</param>
        public DeniedTextException(string message, Exception innerException, string text = null)
            : base(message, innerException)
        {
            InvalidText = text;
        }

        /// <summary>
        /// 例外が発生する元となったテキストを取得します。
        /// </summary>
        public string InvalidText { get; private set; }
    }
}
