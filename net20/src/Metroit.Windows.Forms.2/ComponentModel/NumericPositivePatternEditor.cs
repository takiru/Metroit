using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Metroit.Windows.Forms.ComponentModel
{
    /// <summary>
    /// プラス数値の記号表現を設定するためのエディタを提供します。
    /// </summary>
    internal class NumericPositivePatternEditor : UITypeEditor
    {
        /// <summary>
        /// 表示タイプ設定処理を取得します。
        /// </summary>
        /// <param name="context">ITypeDescriptorContext オブジェクト。</param>
        /// <returns>表示タイプ。</returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            // モーダル表示フォーム
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// プラス数値の記号表現を設定するためのユーザーコントロールを表示します。
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

            using (NumericPositivePatternControl nppc = new NumericPositivePatternControl(
                    (NumericPositivePattern)value))
            {
                var currentCurrencyPattern = nppc.NumericPositivePattern.CurrencyPositivePattern;
                var currentPercentPattern = nppc.NumericPositivePattern.PercentPositivePattern;

                editorService.DropDownControl(nppc);

                // 呼び出し前と値が変更なければそのまま返却
                if (nppc.NumericPositivePattern.CurrencyPositivePattern == currentCurrencyPattern &&
                        nppc.NumericPositivePattern.PercentPositivePattern == currentPercentPattern)
                {
                    return nppc.NumericPositivePattern;
                }

                // 呼び出し前と値の変更があった場合、新規オブジェクトとして返却
                var result = new NumericPositivePattern();
                result.CurrencyPositivePattern = nppc.NumericPositivePattern.CurrencyPositivePattern;
                result.PercentPositivePattern = nppc.NumericPositivePattern.PercentPositivePattern;
                return result;
            }
        }
    }
}
