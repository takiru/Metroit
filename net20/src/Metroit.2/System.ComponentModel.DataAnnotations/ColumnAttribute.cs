using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel.DataAnnotations.Schema
{
    /// <summary>
    /// プロパティのマップ先のデータベース列を表します。
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ColumnAttribute : Attribute
    {
        /// <summary>
        /// プロパティのマップ先列の名前を取得します。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 取得またはプロパティのマップ先列の 0 から始まる順序を設定します。
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 取得またはプロパティのマップ先列のデータベース プロバイダー固有のデータ型を設定します。
        /// </summary>
        public string TypeName { get; set; }
    }
}
