namespace Metroit.Contracts
{
    /// <summary>
    /// ダイアログのレスポンスデータのインターフェースを提供します。
    /// </summary>
    /// <typeparam name="T">レスポンスデータの型。</typeparam>
    public interface IDialogResponse<T>
    {
        /// <summary>
        /// レスポンスデータを取得または設定します。
        /// </summary>
        T Response { get; set; }
    }
}
