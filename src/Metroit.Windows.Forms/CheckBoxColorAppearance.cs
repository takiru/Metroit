using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// チェックボックスの外観の色を提供します。
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CheckBoxColorAppearance
    {
        private readonly CheckBox Owner;
        private readonly string Key;
        private readonly IReadOnlyList<PropertyDefaultController> DefaultControllers;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="owner">オーナーとなるチェックボックス。</param>
        /// <param name="key">親プロパティのキー値。</param>
        /// <param name="defaultControllers">既定値コントローラーのコレクション。</param>
        internal CheckBoxColorAppearance(CheckBox owner, string key, IReadOnlyList<PropertyDefaultController> defaultControllers)
        {
            Owner = owner;
            Key = key;
            DefaultControllers = defaultControllers;
        }

        private Color _borderColor = Color.Empty;

        /// <summary>
        /// ボーダー色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("CheckBoxColorAppearanceBorderColor")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    Owner.Invalidate();
                }
            }
        }

        /// <summary>
        /// ボーダー色の変更があるかどうかを取得する。
        /// </summary>
        /// <returns>変更がある場合は true, それ以外は false を返却する。</returns>
        private bool ShouldSerializeBorderColor()
        {
            return DefaultControllers
                .Where(x => x.Key == $"{Key}.{nameof(BorderColor)}")
                .FirstOrDefault()?
                .ShouldSerialize?.Invoke() ?? true;
        }

        /// <summary>
        /// ボーダー色を既定値にリセットする。
        /// </summary>
        private void ResetBorderColor()
        {
            DefaultControllers
                .Where(x => x.Key == $"{Key}.{nameof(BorderColor)}")
                .FirstOrDefault()?
                .Reset?.Invoke();
        }

        private Color _backColor = Color.Empty;

        /// <summary>
        /// 背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("CheckBoxColorAppearanceBackColor")]
        public Color BackColor
        {
            get => _backColor;
            set
            {
                if (_backColor != value)
                {
                    _backColor = value;
                    Owner.Invalidate();
                }
            }
        }

        /// <summary>
        /// 背景色の変更があるかどうかを取得する。
        /// </summary>
        /// <returns>変更がある場合は true, それ以外は false を返却する。</returns>
        private bool ShouldSerializeBackColor()
        {
            return DefaultControllers
                .Where(x => x.Key == $"{Key}.{nameof(BackColor)}")
                .FirstOrDefault()?
                .ShouldSerialize?.Invoke() ?? true;
        }

        /// <summary>
        /// 背景色を既定値にリセットする。
        /// </summary>
        private void ResetBackColor()
        {
            DefaultControllers
                .Where(x => x.Key == $"{Key}.{nameof(BackColor)}")
                .FirstOrDefault()?
                .Reset?.Invoke();
        }

        /// <summary>
        /// オブジェクトの文字列を返却します。
        /// </summary>
        /// <returns>オブジェクトの文字列。</returns>
        public override string ToString()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return string.Empty;
            }

            return base.ToString();
        }
    }
}
