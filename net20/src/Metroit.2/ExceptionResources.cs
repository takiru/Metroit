using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading;

namespace Metroit
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
            this.resources = new ResourceManager("Metroit.Properties.ExceptionResources", this.GetType().Assembly);
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
                return CultureInfo.CurrentCulture;
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

        public static object GetObject(string name)
        {
            ExceptionResources loader = GetLoader();
            return loader.resources.GetObject(name + "." + Culture.TwoLetterISOLanguageName) ?? loader.resources.GetString(name);
        }
    }
}
