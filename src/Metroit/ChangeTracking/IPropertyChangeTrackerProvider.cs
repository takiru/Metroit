namespace Metroit.ChangeTracking
{
    /// <summary>
    /// 変更追跡の取得インターフェースします。
    /// </summary>
    public interface IPropertyChangeTrackerProvider
    {
        /// <summary>
        /// 変更追跡を取得します。
        /// </summary>
        PropertyChangeTracker ChangeTracker { get; }
    }
}
