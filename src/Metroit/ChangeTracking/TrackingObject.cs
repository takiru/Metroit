using Metroit.Annotations;
using Metroit.ChangeTracking.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Metroit.ChangeTracking
{
    /// <summary>
    /// 変更追跡が可能なオブジェクトを提供します。
    /// </summary>
    /// <typeparam name="T">変更追跡を行うクラス。</typeparam>
    public class TrackingObject<T> : IPropertyChangeTrackerProvider<TrackingObject<T>>, INotifyPropertyChanged where T : class
    {
        private PropertyChangeTracker<TrackingObject<T>> _changeTracker;

        /// <summary>
        /// プロパティの値が変更されたときに発生します。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 変更追跡を取得します。
        /// </summary>
        [NoTracking]
        public PropertyChangeTracker<TrackingObject<T>> ChangeTracker => _changeTracker;

        [NoTracking]
        PropertyChangeTracker IPropertyChangeTrackerProvider.ChangeTracker => ChangeTracker;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public TrackingObject()
        {
            _changeTracker = new PropertyChangeTracker<TrackingObject<T>>(this);
            PropertyChanged += TrackingObject_PropertyChanged;
        }

        /// <summary>
        /// 変更通知が行われたプロパティまたはフィールドを追跡する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackingObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _changeTracker.TrackingProperty(e.PropertyName);
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
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 値変更の通知を行います。
        /// </summary>
        /// <param name="propertyName">プロパティ名。</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
