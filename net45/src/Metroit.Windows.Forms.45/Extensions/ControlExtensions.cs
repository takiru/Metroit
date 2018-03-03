using System;
using System.Collections.Generic;
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
        /// <para>Control が現在デザイン モードかどうかを示す値を取得します。</para>
        /// <para>System.Windows.Forms.Control.DesignMode プロパティで制御されない継承コンポーネントの状態まで把握します。</para>
        /// </summary>
        /// <returns>true:デザインモード中、false:実行中</returns>
        public static bool IsDesignMode(this Control control)
        {
            if (System.ComponentModel.LicenseManager.UsageMode
                    == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return true;
            }

            var processName = Process.GetCurrentProcess().ProcessName.ToUpper();
            if (processName == "DEVENV" || processName == "VCSEXPRESS")
            {
                return true;
            }

            if (AppDomain.CurrentDomain.FriendlyName == "DefaultDomain")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 次のコントロールをアクティブにします。
        /// この命令は、TABキーを押下したものと同様の動作をします。
        /// </summary>
        /// <param name="control">対象Control オブジェクト。</param>
        /// <param name="forward">タブオーダー内を前方に移動する場合はtrue。後方に移動する場合はfalse。</param>
        /// <remarks>TextBox の AcceptsTab が true の時、タブ文字が入力されます。</remarks>
        public static void MoveNextControl(this Control control, bool forward = true)
        {
            if (forward)
            {
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
    }
}
