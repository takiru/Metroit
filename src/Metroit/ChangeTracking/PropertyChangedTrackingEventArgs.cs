using System;

namespace Metroit.ChangeTracking
{
    /// <summary>
    /// 変更追跡が行われたプロパティのイベントのデータを提供します。
    /// </summary>
    public class PropertyChangedTrackingEventArgs : EventArgs
    {
        /// <summary>
        /// 変更追跡が行われたプロパティ名を取得します。
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// 変更追跡が行われたプロパティの値を取得します。
        /// </summary>
        public object ChangedValue { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="propertyName">変更追跡が行われたプロパティ。</param>
        /// <param name="changedValue">変更追跡が行われたプロパティの値。</param>
        public PropertyChangedTrackingEventArgs(string propertyName, object changedValue)
        {
            PropertyName = propertyName;
            ChangedValue = changedValue;
        }
    }
}
