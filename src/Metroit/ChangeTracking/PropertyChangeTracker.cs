using Metroit.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Metroit.ChangeTracking
{
    /// <summary>
    /// オブジェクト内にあるプロパティおよびフィールドの変更追跡を提供します。<br/>
    /// 変更追跡が行われるのは下記をすべて満たすプロパティまたはフィールドです。<br/>
    ///   - <see cref="NoTrackingAttribute"/> が設定されていないプロパティまたはフィールド<br/>
    ///   - <see cref="NoTrackings"/> で指定されていないプロパティまたはフィールド<br/>
    /// プロパティは get アクセサーが必要です。
    /// </summary>
    public class PropertyChangeTracker
    {
        /// <summary>
        /// 変更追跡を行うオブジェクト。
        /// </summary>
        protected object Instance { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="instance">変更追跡を行うオブジェクト。</param>
        public PropertyChangeTracker(object instance)
        {
            Instance = instance;
        }

        /// <summary>
        /// 変更追跡を行わないプロパティまたはフィールドの名前のコレクションを取得または設定します。
        /// </summary>
        public string[] NoTrackings { get; set; } = { };

        /// <summary>
        /// 変更追跡を行うプロパティまたはフィールドのコレクション。
        /// </summary>
        private List<PropertyChangeEntry> _entries = new List<PropertyChangeEntry>();

        /// <summary>
        /// 変更追跡を行うプロパティまたはフィールドのコレクションを取得します。
        /// </summary>
        public IEnumerable<PropertyChangeEntry> Entries => _entries;

        /// <summary>
        /// 直前でトラッキングしたプロパティ名を取得します。
        /// </summary>
        public string LastTrackingProperty { get; private set; } = string.Empty;

        /// <summary>
        /// 変更追跡が行われているかどうかを取得します。
        /// </summary>
        public bool IsTracking { get; private set; } = false;

        /// <summary>
        /// 公開しているすべてのプロパティまたはフィールドの既定値と変更追跡をリセットします。
        /// </summary>
        public virtual void Reset()
        {
            _entries.Clear();

            var properties = Instance.GetType().GetProperties(BindingFlags.Instance |
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.GetField)
                .Where(x => IsTrackingProperty(x));

            foreach (var property in properties)
            {
                _entries.Add(new PropertyChangeEntry(property.Name, property.GetValue(Instance)));
            }
            IsTracking = true;
        }

        /// <summary>
        /// 変更追跡を行うプロパティまたはフィールドかどうかを取得する。
        /// </summary>
        /// <param name="pi">プロパティ情報。</param>
        /// <returns>変更追跡を行うプロパティまたはフィールドの場合は true, それ以外は false を返却する。</returns>
        private bool IsTrackingProperty(PropertyInfo pi)
        {
            if (pi.GetCustomAttribute(typeof(NoTrackingAttribute)) != null)
            {
                return false;
            }

            if (NoTrackings == null)
            {
                return true;
            }

            if (NoTrackings.Contains(pi.Name))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// プロパティまたはフィールドを追跡します。
        /// </summary>
        /// <param name="propertyName">変更通知が行われたプロパティ名。</param>
        public void TrackingProperty(string propertyName)
        {
            // 追跡対象となっていないプロパティは追跡しない
            if (!_entries.Any(x => x.PropertyName == propertyName))
            {
                return;
            }

            var defaultValue = _entries
                .Where(x => x.PropertyName == propertyName)
                .Select(x => x.OriginalValue)
                .Single();
            var changedValue = Instance.GetType().GetProperty(propertyName).GetValue(Instance);

            _entries
                .Where(x => x.PropertyName == propertyName)
                .Single()
                .ChangeValue(changedValue);

            LastTrackingProperty = propertyName;

            // 既定値に戻ってすべてのエントリが無変更となったとき、変更状態を初期化する
            if (Equals(changedValue, defaultValue))
            {
                if (_entries.Where(x => x.Changed).Count() == 0)
                {
                    IsSomethingValueChanged = false;
                }
                return;
            }

            IsSomethingValueChanged = true;
        }

        private bool _isSomethingValueChanged = false;

        /// <summary>
        /// プロパティまたはフィールドの値に変更があったかを取得します。
        /// </summary>
        public bool IsSomethingValueChanged
        {
            get => _isSomethingValueChanged;
            private set
            {
                if (_isSomethingValueChanged != value)
                {
                    _isSomethingValueChanged = value;
                    SomethingValueChanged?.Invoke(value);
                }
            }
        }

        /// <summary>
        /// 変更状態に変化があったときに走行します。
        /// 何らかの変更があった場合は true, すべてのプロパティおよびフィールドが既定値だった場合は false を提供します。
        /// </summary>
        public Action<bool> SomethingValueChanged = null;

        /// <summary>
        /// プロパティまたはフィールドが変更されたかどうかを取得します。
        /// </summary>
        /// <param name="propertyName">プロパティ名またはフィールド名。</param>
        /// <returns>プロパティまたはフィールドが変更されていた場合は true, それ以外は false を返却します。</returns>
        public bool ContainsChangedProperty(string propertyName)
        {
            return _entries
                .Where(x => x.PropertyName == propertyName && x.Changed)
                .Any();
        }

        /// <summary>
        /// 変更されたプロパティまたはフィールドのコレクションを取得します。
        /// </summary>
        /// <returns>変更されたプロパティまたはフィールドのコレクション。</returns>
        public IEnumerable<string> GetChangedProperties()
        {
            foreach (var changedValue in _entries.Where(x => x.Changed))
            {
                yield return changedValue.PropertyName;
            }
        }
    }
}
