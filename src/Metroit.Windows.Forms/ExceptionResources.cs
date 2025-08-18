using System.Globalization;
using System.Resources;
using System.Threading;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 例外メッセージのリソース取得を提供します。
    /// </summary>
    internal sealed class ExceptionResources
    {
        private static ExceptionResources loader;
        private ResourceManager resources;

        internal ExceptionResources()
        {
            this.resources = new ResourceManager("Metroit.Windows.Forms.Properties.ExceptionResources", this.GetType().Assembly);
        }

        private static ExceptionResources GetLoader()
        {
            Interlocked.CompareExchange<ExceptionResources>(ref loader, new ExceptionResources(), null);
            return loader;
        }

        private static CultureInfo Culture
        {
            get
            {
                return CultureInfo.DefaultThreadCurrentUICulture ?? CultureInfo.CurrentCulture;
            }
        }

        public static ResourceManager Resources
        {
            get
            {
                return GetLoader().resources;
            }
        }

        public static string GetString(string name)
        {
            ExceptionResources loader = GetLoader();
            return loader.resources.GetString(name + "." + Culture.TwoLetterISOLanguageName) ?? loader.resources.GetString(name);
        }

        public static string GetString(string name, params object[] param)
        {
            ExceptionResources loader = GetLoader();
            return string.Format(loader.resources.GetString(name + "." + Culture.TwoLetterISOLanguageName) ?? loader.resources.GetString(name), param);
        }

        public static object GetObject(string name)
        {
            ExceptionResources loader = GetLoader();
            return loader.resources.GetObject(name + "." + Culture.TwoLetterISOLanguageName) ?? loader.resources.GetString(name);
        }

    }
}
