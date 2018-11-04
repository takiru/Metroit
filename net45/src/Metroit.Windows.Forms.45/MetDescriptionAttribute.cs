using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// Metroit で用意された説明文を提供します。
    /// </summary>
    internal sealed class MetDescriptionAttribute : DescriptionAttribute
    {
        /// <summary>
        /// MetDescriptionAttribute クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MetDescriptionAttribute() { }

        /// <summary>
        /// MetDescriptionAttribute クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="description">説明文。</param>
        public MetDescriptionAttribute(string description) : base(description) { }

        /// <summary>
        /// 指定された説明文キーワードからローカライズされた説明文を取得します。
        /// </summary>
        public override string Description => DesignResources.GetString(DescriptionValue);
    }
}
