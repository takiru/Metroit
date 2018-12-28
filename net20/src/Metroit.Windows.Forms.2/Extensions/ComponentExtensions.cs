using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Metroit.Windows.Forms.Extensions
{
    /// <summary>
    /// Component クラスの拡張メソッドを提供します。
    /// </summary>
    public static class ComponentExtensions
    {
        private static readonly string processName = Process.GetCurrentProcess().ProcessName.ToUpper();

        /// <summary>
        /// <para>Component が現在デザイン モードかどうかを示す値を取得します。</para>
        /// <para>System.Windows.Forms.Control.DesignMode プロパティで制御されない継承コンポーネントの状態まで把握します。</para>
        /// </summary>
        /// <returns>true:デザインモード中、false:実行中</returns>
        public static bool IsDesignMode(Component component)
        {
            if (System.ComponentModel.LicenseManager.UsageMode
                    == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return true;
            }

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
    }
}
