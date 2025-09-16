using System;
using System.ComponentModel;
using System.Diagnostics;

#if (NET462_OR_GREATER)
using System.Deployment.Application;
#endif

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
        public static bool IsDesignMode(this Component component)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return true;
            }

            if (processName == "DEVENV" ||
                processName == "VCSEXPRESS" ||
                processName == "DESIGNTOOLSSERVER")
            {
                return true;
            }

            if (component.Site?.DesignMode == true)
            {
                return true;
            }

#if (NET462_OR_GREATER)
            // ClickOnceでの実行はDefaultDomainのため、ClickOnce時は検証しない
            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                if (AppDomain.CurrentDomain.FriendlyName == "DefaultDomain")
                {
                    return true;
                }
            }
#endif
            return false;
        }
    }
}
