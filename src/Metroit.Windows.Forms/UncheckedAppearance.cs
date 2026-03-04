using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// チェックボックスの外観を提供します。
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class UncheckedAppearance
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
        internal UncheckedAppearance(CheckBox owner, string key, IReadOnlyList<PropertyDefaultController> defaultControllers)
        {
            Owner = owner;
            Key = key;
            DefaultControllers = defaultControllers;
        }

        private UncheckedColorAppearance _defaultAppearance;

        /// <summary>
        /// 既定の外観を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("CheckBoxAppearanceDefault")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public UncheckedColorAppearance Default
        {
            get
            {
                if (_defaultAppearance == null)
                {
                    _defaultAppearance = new UncheckedColorAppearance(Owner, $"{Key}.{nameof(Default)}", DefaultControllers);
                }
                return _defaultAppearance;
            }
        }

        private UncheckedColorAppearance _focusedAppearance;

        /// <summary>
        /// フォーカスを有しているか、マウスカーソルが領域内に入ったときの外観を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("CheckBoxAppearanceFocused")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public UncheckedColorAppearance Focused
        {
            get
            {
                if (_focusedAppearance == null)
                {
                    _focusedAppearance = new UncheckedColorAppearance(Owner, $"{Key}.{nameof(Focused)}", DefaultControllers);
                }
                return _focusedAppearance;
            }
        }

        private UncheckedColorAppearance _pressedAppearance;

        /// <summary>
        /// マスペースキーが押されたか、ウスがクリックされたときの外観を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("CheckBoxAppearancePressed")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public UncheckedColorAppearance Pressed
        {
            get
            {
                if (_pressedAppearance == null)
                {
                    _pressedAppearance = new UncheckedColorAppearance(Owner, $"{Key}.{nameof(Pressed)}", DefaultControllers);
                }
                return _pressedAppearance;
            }
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
