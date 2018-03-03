using Metroit.Windows.Forms;
using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Metroit.Windows.Forms.ComponentModel
{
    /// <summary>
    /// 許可文字を設定するためのエディタを提供します。
    /// </summary>
    internal class AcceptsCharEditor : UITypeEditor
    {
        /// <summary>
        /// 表示タイプ設定処理を取得します。
        /// </summary>
        /// <param name="context">ITypeDescriptorContext オブジェクト。</param>
        /// <returns>表示タイプ。</returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            // ドロップダウン表示フォーム
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// 許可文字を設定するためのユーザーコントロールを表示します。
        /// </summary>
        /// <param name="context">ITypeDescriptorContext オブジェクト。</param>
        /// <param name="provider">IServiceProvider オブジェクト。</param>
        /// <param name="value">object オブジェクト。</param>
        /// <returns>プロパティ値。</returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
                IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = null;

            // MSDN UI 型エディターの実装 に基づいた記述
            if (provider != null)
            {
                editorService = provider.GetService(
                        typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }

            // MSDN UI 型エディターの実装 に基づいた記述
            if (editorService == null)
            {
                return value;
            }

            using (AcceptsCharControl acc = new AcceptsCharControl(
                    (AcceptsCharType)value))
            {
                editorService.DropDownControl(acc);
                if (acc.ApprovalCharacters == 0)
                {
                    acc.ApprovalCharacters |= AcceptsCharType.All;
                }
                return acc.ApprovalCharacters;
            }
        }
    }
}
