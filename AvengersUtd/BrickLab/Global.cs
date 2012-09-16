using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using AvengersUtd.BrickLab.DataAccess;
using AvengersUtd.BrickLab.Settings;
using AvengersUtd.BrickLab.ViewModel;

namespace AvengersUtd.BrickLab
{
    public enum ViewType
    {
        None,
        OrdersReceived,
        InventoryView,
        About
    }

    public static class Global
    {
        public const double Epsilon = 0.001;
        public const string SettingsFile = "Preferences.data";
        public const string OrdersReceivedFile = "orders.xml";
        public const string CacheDir = "/Cache";
        private static Preferences preferences;

        public static Preferences CurrentPreferences
        {
            get { return preferences; }
            internal set
            {
                preferences = value;

                if (preferences.HasProxy)
                    WebRequest.DefaultWebProxy = new WebProxy(Global.CurrentPreferences.ProxyAddress,
                                                              Global.CurrentPreferences.ProxyPort);

            }
        }

        public static ViewType CurrentView { get; internal set; }

        public static DateTime LastLogin { get; internal set; }
        internal const int TimeOutMins = 15;

        public static string Version
        {
            get
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Revision);
            }
        }

        public static string CurrentDir { get; private set;}


        public static void Init()
        {
            string localAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BrickLab");
            if (!Directory.Exists(localAppData))
                Directory.CreateDirectory(localAppData);

            CurrentDir = localAppData;

            string settingsPath = Path.Combine(CurrentDir, SettingsFile);
            if (File.Exists(settingsPath))
                CurrentPreferences = XmlManager.Deserialize<Preferences>(settingsPath);
            else
                CurrentPreferences = new Preferences();


        }
    }
}
