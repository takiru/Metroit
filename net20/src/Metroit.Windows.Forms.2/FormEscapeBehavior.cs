namespace Metroit.Windows.Forms
{
    /// <summary>
    /// ESCキーが押された時の振る舞いの定義です。
    /// </summary>
    public enum FormEscapeBehavior
    {
        /// <summary>
        /// 何もしないことを示します。
        /// </summary>
        None,
        /// <summary>
        /// コントロールがアクティブの時に非アクティブにすることを示します。
        /// </summary>
        ControlLeave,
        /// <summary>
        /// フォームを閉じることを示します。
        /// </summary>
        FormClose,
        /// <summary>
        /// 1回目の押下で非アクティブにし、2回目の押下でフォームを閉じることを示します。
        /// </summary>
        Both
    }
}
