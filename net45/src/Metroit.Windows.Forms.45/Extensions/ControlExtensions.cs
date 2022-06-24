﻿using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Windows.Forms;

namespace Metroit.Windows.Forms.Extensions
{
    /// <summary>
    /// Control クラスの拡張メソッドを提供します。
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// 次のコントロールをアクティブにします。
        /// この命令は、TABキーを押下したものと同様の動作をします。
        /// </summary>
        /// <param name="control">対象Control オブジェクト。</param>
        /// <param name="forward">タブオーダー内を前方に移動する場合はtrue。後方に移動する場合はfalse。</param>
        /// <remarks></remarks>
        public static void MoveNextControl(this Control control, bool forward = true)
        {
            var textBox = GetTextBox(control);
            if (textBox != null && textBox.Multiline && textBox.AcceptsTab)
            {
                if (forward)
                {
                    SendKeys.Send("^{TAB}");
                }
                else
                {
                    SendKeys.Send("^+{TAB}");
                }
                return;
            }

            // その他コントロール
            if (forward)
            {
                // NOTE: テキストエリアで CtrL+V を行われた後に処理されると、 "^{TAB}" となり、Tab文字が入力されてしまうため、
                //       強制的に Ctrl を一度送信して回避する。
                if (Control.ModifierKeys == Keys.Control)
                {
                    SendKeys.Send("^");
                }
                SendKeys.Send("{TAB}");
            }
            else
            {
                SendKeys.Send("+{TAB}");
            }
        }

        /// <summary>
        /// コントロールに存在するすべてのコントロールをフォーカス遷移順に取得します。
        /// </summary>
        /// <param name="control">対象Control オブジェクト。</param>
        /// <param name="excludeInvisible">true:非表示項目を取得しない, false:非表示項目を取得する。</param>
        /// <param name="excludeDisable">true:無効項目を取得しない, false:無効項目を取得する。</param>
        /// <returns>コントロールリスト。</returns>
        public static List<Control> GetChildControls(this Control control, bool excludeInvisible = true, bool excludeDisable = true)
        {
            List<Control> allControls;
            GetTargetChildControls(control, control, excludeInvisible, excludeDisable, out allControls);

            return allControls;
        }

        /// <summary>
        /// 対象コントロール上のすべてのコントロールを取得する。
        /// </summary>
        /// <param name="targetControl">次のコントロールを取得する対象オブジェクト。</param>
        /// <param name="currentControl">手前のコントロールオブジェクト。</param>
        /// <param name="excludeInvisible">true:非表示項目を取得しない, false:非表示項目を取得する。</param>
        /// <param name="excludeDisable">true:無効項目を取得しない, false:無効項目を取得する。</param>
        /// <param name="allControls">取得したすべてのコントロールオブジェクト</param>
        /// <returns>コントロールリスト。</returns>
        private static Control GetTargetChildControls(Control targetControl, Control currentControl,
                bool excludeInvisible, bool excludeDisable, out List<Control> allControls)
        {
            allControls = new List<Control>();

            while (true)
            {
                var nextControl = targetControl.GetNextControl(currentControl, true);
                if (nextControl == null)
                {
                    return currentControl;
                }
                if (excludeInvisible && !nextControl.Visible)
                {
                    currentControl = nextControl;
                    continue;
                }
                if (excludeDisable && !nextControl.Enabled)
                {
                    currentControl = nextControl;
                    continue;
                }
                allControls.Add(nextControl);

                if (nextControl.Controls.Count > 0)
                {
                    var innerControls = new List<Control>();
                    nextControl = GetTargetChildControls(nextControl, nextControl,
                            excludeInvisible, excludeDisable, out innerControls);
                    if (innerControls.Count > 0)
                    {
                        allControls.AddRange(innerControls);
                    }
                }
                currentControl = nextControl;
            }
        }

        /// <summary>
        /// 対象コントロールから TextBox を見つけ出す。
        /// </summary>
        /// <param name="control">Control オブジェクト。</param>
        /// <returns>TextBox オブジェクト。</returns>
        private static TextBox GetTextBox(Control control)
        {
            var targetControl = control as TextBox;
            if (targetControl != null)
            {
                return targetControl;
            }

            var form = control as Form;
            if (form == null)
            {
                return null;
            }
            targetControl = form.ActiveControl as TextBox;
            if (targetControl != null)
            {
                return targetControl;
            }

            var splitContainer = form.ActiveControl as SplitContainer;
            targetControl = splitContainer?.ActiveControl as TextBox;
            if (targetControl != null)
            {
                return targetControl;
            }

            return null;
        }
    }
}
