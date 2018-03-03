using System;
using System.Collections.Generic;

namespace Metroit.Collections.Generic
{
    /// <summary>
    /// 管理上限を有する List を提供します。
    /// </summary>
    /// <typeparam name="TValue">管理を行うオブジェクト。</typeparam>
    public class LimitedList<TValue> : List<TValue>
    {
        private TValue lastAccessInstance = default(TValue);

        /// <summary>
        /// LimitedList クラスの新しいインスタンスを初期化します。
        /// </summary>
        public LimitedList() : base() { }

        /// <summary>
        /// LimitedList クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="maxSize">管理する最大数。0を指定すると無制限になります。</param>
        public LimitedList(int maxSize = 0) : base()
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

        /// <summary>指定されたインデックスの値を取得または設定します。</summary>
        /// <param name="index">インデックス。</param>
        /// <returns>指定されたインデックスの値。</returns>
        public new TValue this[int index]
        {
            get
            {
                lastAccessInstance = base[index];
                return base[index];
            }
        }

        /// <summary>指定した値をリストに追加します。</summary>
        /// <param name="value">追加する要素の値。</param>
        /// <exception cref="T:System.ArgumentException">管理上限に達したため、追加できません。</exception>
        public new void Add(TValue value)
        {
            if (!CanAdd())
            {
                throw new ArgumentException("MaxSize", ExceptionResources.GetString("ObjectMaxReached"));
            }

            base.Add(value);
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
