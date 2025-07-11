namespace Metroit.Windows.Forms
{
    /// <summary>
    /// アプリケーションミューテックスの動作を定義します。
    /// </summary>
    public enum ApplicationMutexBehavior
    {
        /// <summary>
        /// アプリケーションを終了することを示します。
        /// </summary>
        Shutdown,

        /// <summary>
        /// アプリケーションを前面に表示することを示します。
        /// </summary>
        BringToFront
    }
}
