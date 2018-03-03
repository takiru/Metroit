using System;
using System.Collections.Generic;

namespace Metroit.Collections.Generic
{
    /// <summary>
    /// 管理上限を有する Dictionary を提供します。
    /// </summary>
    /// <typeparam name="TKey">キーオブジェクト。</typeparam>
    /// <typeparam name="TValue">管理を行うオブジェクト。</typeparam>
    public class LimitedDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private TValue lastAccessInstance = default(TValue);

        /// <summary>
        /// UpperLimitDictionary クラスの新しいインスタンスを初期化します。
        /// </summary>
        public LimitedDictionary() : base() { }

        /// <summary>
        /// UpperLimitDictionary クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="maxSize">管理する最大数。0を指定すると無制限になります。</param>
        public LimitedDictionary(int maxSize = 0) : base()
        {
            if (maxSize < 0)
            {
                throw new ArgumentOutOfRangeException("MaxSize", ExceptionResources.GetString("ObjectMaxSizeOutOfRange"));
            }

            MaxSize = maxSize;
        }

        /// <summary>
        /// 管理可能な最大インスタンス数を取得します。
        /// </summary>
        public int MaxSize { get; private set; }

        /// <summary>
        /// 直前で操作した値を取得します。
        /// </summary>
        /// <returns>直前で操作した値。</returns>
        public TValue LastAccessItem => lastAccessInstance;

        /// <summary>指定されたキーに関連付けられている値を取得または設定します。</summary>
        /// <param name="key">取得または設定する値のキー。</param>
        /// <returns>指定されたキーに関連付けられている値。</returns>
        public new TValue this[TKey key]
        {
            get
            {
                lastAccessInstance = base[key];
                return base[key];
            }
        }

        /// <summary>指定したキーと値をディクショナリに追加します。</summary>
        /// <param name="key">追加する要素のキー。</param>
        /// <param name="value">追加する要素の値。</param>
        /// <exception cref="T:System.ArgumentException">管理上限に達したため、追加できません。</exception>
        public new void Add(TKey key, TValue value)
        {
            if (!CanAdd())
            {
                throw new ArgumentException("MaxSize", ExceptionResources.GetString("ObjectMaxReached"));
            }

            base.Add(key, value);
            lastAccessInstance = value;
        }

        /// <summary>
        /// 追加が可能かどうかを取得します。
        /// </summary>
        /// <returns>true:追加可能, false:追加不可。</returns>
        public bool CanAdd()
        {
            // 管理数の制限
            if (MaxSize > 0 && Count == MaxSize)
            {
                return false;
            }

            return true;
        }
    }
}
