using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// デザイン情報のリソース取得を提供します。
    /// </summary>
    internal sealed class DesignResources
    {
        private static DesignResources loader;
        private ResourceManager resources;

        internal DesignResources()
        {
            this.resources = new ResourceManager("Metroit.Windows.Forms.Properties.DesignResources", this.GetType().Assembly);
        }

        private static DesignResources GetLoader()
        {
            Interlocked.CompareExchange<DesignResources>(ref loader, new DesignResources(), null);
            return loader;
        }

        private static CultureInfo Culture
        {
            get
            {
                return CultureInfo.DefaultThreadCurrentUICulture;
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
            DesignResources loader = GetLoader();
            return loader.resources.GetString(name + "." + Culture.TwoLetterISOLanguageName) ?? loader.resources.GetString(name);
        }

        public static object GetObject(string name)
        {
            DesignResources loader = GetLoader();
            return loader.resources.GetObject(name + "." + Culture.TwoLetterISOLanguageName) ?? loader.resources.GetString(name);
        }
    }
}
