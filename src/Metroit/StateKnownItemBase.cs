using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Metroit
{
    /// <summary>
    /// 値の変更状態を有するアイテムを提供します。
    /// </summary>
    public abstract class StateKnownItemBase : INotifyPropertyChanged
    {
        private ItemState state = ItemState.NotModified;

        /// <summary>
        /// アイテムの状態を取得します。
        /// </summary>
        public ItemState State
        {
            get => state;
            private set
            {
                SetProperty(ref state, value);
            }
        }

        private bool isDataInitializing = false;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public StateKnownItemBase()
        {
            isDataInitializing = true;
            SetInitialData();
            isDataInitializing = false;

            NewItem();
        }

        /// <summary>
        /// 初期データを設定します。
        /// </summary>
        protected virtual void SetInitialData() { }

        /// <summary>
        /// アイテムを新規データ扱いにします。
        /// </summary>
        public void NewItem()
        {
            State = ItemState.New;
        }

        /// <summary>
        /// アイテムを新規編集データ扱いにします。
        /// </summary>
        public void NewModifiedItem()
        {
            State = ItemState.NewModified;
        }

        /// <summary>
        /// アイテムを修正データ扱いにします。
        /// </summary>
        public void ModifedItem()
        {
            State = ItemState.Modified;
        }

        /// <summary>
        /// アイテムを未修正データ扱いにします。
        /// </summary>
        public void NotModifedItem()
        {
            State = ItemState.NotModified;
        }

        /// <summary>
        /// 変更の通知が行われた時に発生します。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 値の設定を行い、変更を通知します。
        /// </summary>
        /// <typeparam name="T">設定するプロパティ情報</typeparam>
        /// <param name="field">値を設定する変数。</param>
        /// <param name="value">値。</param>
        /// <param name="propertyName">プロパティ名。</param>
        /// <returns>true:値の設定を行った, false:値の設定を行わなかった。</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            // 初期データ設定時は処理しない
            if (isDataInitializing)
            {
                return false;
            }

            // 値に変更がない場合は処理しない
            if (Equals(field, value))
            {
                return false;
            }

            field = value;
            NotifyChangedValue(propertyName, value);

            // ステート以外が変更された場合はステートを変更する
            if (string.Compare(propertyName, nameof(State), true) != 0)
            {
                switch (State)
                {
                    case ItemState.New:
                        NewModifiedItem();
                        break;

                    case ItemState.NotModified:
                        ModifedItem();
                        break;
                }
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }

        /// <summary>
        /// 変更された値を通知します。
        /// </summary>
        /// <typeparam name="T">設定するプロパティ情報</typeparam>
        /// <param name="propertyName">プロパティ名。</param>
        /// <param name="value">値。</param>
        protected virtual void NotifyChangedValue<T>(string propertyName, T value) { }
    }
}
