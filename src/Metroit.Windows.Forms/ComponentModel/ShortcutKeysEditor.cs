using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Metroit.Windows.Forms.ComponentModel
{
    /// <summary>
    /// 許可文字を設定するためのエディタを提供します。
    /// </summary>
    public class ShortcutKeysEditor : UITypeEditor
    {
        private ShortcutKeysControl shortcutKeysControl = null;

        /// <summary>
        /// 表示タイプ設定処理を取得します。
        /// </summary>
        /// <param name="context">ITypeDescriptorContext オブジェクト。</param>
        /// <returns>表示タイプ。</returns>
        public override UITypeEditorEditStyle GetEditStyle(
          ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// 許可文字を設定するためのユーザーコントロールを表示します。
        /// </summary>
        /// <param name="context">ITypeDescriptorContext オブジェクト。</param>
        /// <param name="provider">IServiceProvider オブジェクト。</param>
        /// <param name="value">object オブジェクト。</param>
        /// <returns>プロパティ値。</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider == null)
            {
                return value;
            }

            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service == null)
            {
                return value;
            }

            // デザイナ上で一度もコントロールが開かれていない時に初期化する
            if (shortcutKeysControl == null)
            {
                shortcutKeysControl = new ShortcutKeysControl(this);
            }

            shortcutKeysControl.Prepare(service, value);
            service.DropDownControl(shortcutKeysControl);
            value = shortcutKeysControl.Value;
            shortcutKeysControl.Terminate();

            return value;
        }
    }
}
