namespace Metroit.ChangeTracking.Generic
{
    /// <summary>
    /// オブジェクト内にあるプロパティおよびフィールドの変更追跡を提供します。<br/>
    /// 変更追跡が行われるのは下記をすべて満たすプロパティまたはフィールドです。<br/>
    ///   - <see cref="Metroit.Annotations.NoTrackingAttribute"/> が設定されていないプロパティまたはフィールド<br/>
    ///   - <see cref="NoTrackings"/> で指定されていないプロパティまたはフィールド<br/>
    /// プロパティは get アクセサーが必要です。
    /// </summary>
    /// <typeparam name="T">変更追跡を行うクラス。</typeparam>
    public class PropertyChangeTracker<T> : PropertyChangeTracker where T : class
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="instance">変更追跡を行うオブジェクト。</param>
        public PropertyChangeTracker(T instance) : base(instance) { }
    }
}
