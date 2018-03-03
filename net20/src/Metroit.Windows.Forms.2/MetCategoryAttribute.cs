using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// Metroit で用意された新たなカテゴリを提供します。
    /// </summary>
    internal sealed class MetCategoryAttribute : CategoryAttribute
    {
        /// <summary>
        /// MetCategoryAttribute クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MetCategoryAttribute() { }

        /// <summary>
        /// MetCategoryAttribute クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="category">カテゴリ名。</param>
        public MetCategoryAttribute(string category) : base(category) { }

        /// <summary>
        /// 指定されたカテゴリ名からローカライズされたカテゴリ表示文字を取得します。
        /// </summary>
        /// <param name="value">カテゴリ名。</param>
        /// <returns>ローカライズされたカテゴリ表示文字。</returns>
        protected override string GetLocalizedString(string value)
        {
            return DesignResources.GetString(value);
        }
    }
}
