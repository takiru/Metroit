namespace Metroit.Contracts
{
    /// <summary>
    /// ダイアログのリクエストデータのインターフェースを提供します。
    /// </summary>
    /// <typeparam name="T">リクエストデータの型。</typeparam>
    public interface IDialogRequest<T>
    {
        /// <summary>
        /// リクエストデータを取得または設定します。
        /// </summary>
        T Request { get; set; }
    }
}
