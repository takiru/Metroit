namespace Metroit.ChangeTracking
{
    /// <summary>
    /// プロパティの変更を追跡するエントリを表します。
    /// </summary>
    public class PropertyChangeEntry
    {
        /// <summary>
        /// プロパティ名を取得します。
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// オリジナルの値を取得します。
        /// </summary>
        public object OriginalValue { get; }

        /// <summary>
        /// 現在の値を取得します。
        /// </summary>
        public object CurrentValue { get; private set; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="propertyName">プロパティ名。</param>
        /// <param name="originalValue">オリジナルの値。</param>
        public PropertyChangeEntry(string propertyName, object originalValue)
        {
            PropertyName = propertyName;
            OriginalValue = originalValue;
            CurrentValue = OriginalValue;
        }

        /// <summary>
        /// 変更された値を設定します。
        /// </summary>
        /// <param name="changedValue">変更された値。</param>
        internal void ChangeValue(object changedValue)
        {
            CurrentValue = changedValue;
        }

        /// <summary>
        /// 変更されたかどうかを取得します。
        /// </summary>
        public bool Changed => !object.Equals(OriginalValue, CurrentValue);
    }
}
