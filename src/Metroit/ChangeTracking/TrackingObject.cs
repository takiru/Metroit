using Metroit.Annotations;
using Metroit.ChangeTracking.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Metroit.ChangeTracking
{
    /// <summary>
    /// 変更追跡が可能なオブジェクトを提供します。
    /// </summary>
    /// <typeparam name="T1">変更追跡対象オブジェクト。</typeparam>
    /// <typeparam name="T2">トラッカー。</typeparam>
    public class TrackingObject<T1, T2> : IPropertyChangeTrackerProvider<T1>, INotifyPropertyChanged where T1 : class where T2 : PropertyChangeTracker<T1>, new()
    {
        private readonly T2 _changeTracker;

        /// <summary>
        /// プロパティの値が変更されたときに発生します。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 変更追跡を取得します。
        /// </summary>
        [NoTracking]
        public PropertyChangeTracker<T1> ChangeTracker => _changeTracker;

        [NoTracking]
        PropertyChangeTracker IPropertyChangeTrackerProvider.ChangeTracker => ChangeTracker;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public TrackingObject()
        {
            _changeTracker = new T2();
            _changeTracker.SetInstance(this);
        }

        /// <summary>
        /// プロパティの値を設定し、変更があった場合に通知を行います。
        /// </summary>
        /// <typeparam name="U">プロパティの型。</typeparam>
        /// <param name="field">更新フィールド。</param>
        /// <param name="value">更新値。</param>
        /// <param name="propertyName">プロパティ名。</param>
        /// <returns>プロパティの値が変更された場合は true, それ以外は false を返却します。</returns>
        protected bool SetProperty<U>(ref U field, U value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(field, value))
            {
                return false;
            }

            field = value;
            _changeTracker.TrackingProperty(propertyName);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            return true;
        }

        /// <summary>
        /// 値変更の通知を行います。
        /// </summary>
        /// <param name="e">プロパティ名。</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
