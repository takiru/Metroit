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

        private UncheckedColorAppearance _mouseOverAppearance;

        /// <summary>
        /// マウスカーソルが領域内に入ったときの外観を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("CheckBoxAppearanceMouseOver")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public UncheckedColorAppearance MouseOver
        {
            get
            {
                if (_mouseOverAppearance == null)
                {
                    _mouseOverAppearance = new UncheckedColorAppearance(Owner, $"{Key}.{nameof(MouseOver)}", DefaultControllers);
                }
                return _mouseOverAppearance;
            }
        }

        private UncheckedColorAppearance _mouseDownAppearance;

        /// <summary>
        /// マウスがクリックされたときの外観を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [MetCategory("MetAppearance")]
        [MetDescription("CheckBoxAppearanceMouseDown")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public UncheckedColorAppearance MouseDown
        {
            get
            {
                if (_mouseDownAppearance == null)
                {
                    _mouseDownAppearance = new UncheckedColorAppearance(Owner, $"{Key}.{nameof(MouseDown)}", DefaultControllers);
                }
                return _mouseDownAppearance;
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
