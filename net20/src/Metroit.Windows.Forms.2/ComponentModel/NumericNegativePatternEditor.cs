using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Metroit.Windows.Forms.ComponentModel
{
    /// <summary>
    /// マイナス数値の記号表現を設定するためのエディタを提供します。
    /// </summary>
    internal class NumericNegativePatternEditor : UITypeEditor
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
        /// マイナス数値の記号表現を設定するためのユーザーコントロールを表示します。
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

            using (NumericNegativePatternControl nnpc = new NumericNegativePatternControl(
                    (NumericNegativePattern)value))
            {
                var curentNumberPattern = nnpc.NumericNegativePattern.NumberNegativePattern;
                var currentCurrencyPattern = nnpc.NumericNegativePattern.CurrencyNegativePattern;
                var currentPercentPattern = nnpc.NumericNegativePattern.PercentNegativePattern;

                editorService.DropDownControl(nnpc);

                // 呼び出し前と値が変更なければそのまま返却
                if (nnpc.NumericNegativePattern.NumberNegativePattern == curentNumberPattern &&
                        nnpc.NumericNegativePattern.CurrencyNegativePattern == currentCurrencyPattern &&
                        nnpc.NumericNegativePattern.PercentNegativePattern == currentPercentPattern)
                {
                    return nnpc.NumericNegativePattern;
                }

                // 呼び出し前と値の変更があった場合、新規オブジェクトとして返却
                var result = new NumericNegativePattern();
                result.NumberNegativePattern = nnpc.NumericNegativePattern.NumberNegativePattern;
                result.CurrencyNegativePattern = nnpc.NumericNegativePattern.CurrencyNegativePattern;
                result.PercentNegativePattern = nnpc.NumericNegativePattern.PercentNegativePattern;
                return result;
            }
        }
    }
}
