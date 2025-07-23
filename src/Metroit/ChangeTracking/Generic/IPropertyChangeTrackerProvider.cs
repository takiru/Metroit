namespace Metroit.ChangeTracking.Generic
{
    /// <summary>
    /// 変更追跡の取得インターフェースします。
    /// </summary>
    /// <typeparam name="T">変更追跡を行うクラス。</typeparam>
    public interface IPropertyChangeTrackerProvider<T> : IPropertyChangeTrackerProvider where T : class
    {
        /// <summary>
        /// 変更追跡を取得します。
        /// </summary>
        new PropertyChangeTracker<T> ChangeTracker { get; }
    }
}
