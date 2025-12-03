using Metroit.Annotations;

namespace Metroit.ChangeTracking.Generic
{
    /// <summary>
    /// オブジェクト内にあるプロパティおよびフィールドの変更追跡を提供します。<br/>
    /// <see cref="NoTrackingAttribute"/>または<see cref="PropertyChangeTracker.NoTrackings"/>が設定されたプロパティまたはフィールドは変更追跡をしません。<br/>
    /// 追跡を必要とするプロパティまたはフィールドは get アクセサーが必要です。
    /// </summary>
    /// <typeparam name="T">変更追跡を行うクラス。</typeparam>
    public class PropertyChangeTracker<T> : PropertyChangeTracker where T : class
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="instance">変更追跡を行うオブジェクト。</param>
        public PropertyChangeTracker(T instance) : base(instance) { }

        public PropertyChangeTracker() : base()
        {

        }
    }
}
