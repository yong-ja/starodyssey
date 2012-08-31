using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AvengersUtd.BrickLab.Settings;
using AvengersUtd.BrickLab.ViewModel;

namespace AvengersUtd.BrickLab
{
    public static class Global
    {
        public const string SettingsFile = "Preferences.data";
        public const string OrdersReceivedFile = "orders.xml";
        public static Preferences CurrentPreferences { get; internal set; }

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


        public static void Init()
        {
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFile)))
                CurrentPreferences = XmlManager.Deserialize<Preferences>(SettingsFile);
            
        }
    }
}
