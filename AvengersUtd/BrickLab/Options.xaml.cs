using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AvengersUtd.BrickLab.Data;
using AvengersUtd.BrickLab.Settings;
using System.IO;

namespace AvengersUtd.BrickLab
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            string encryptedPwd = DataProtector.EncryptData(tPassword.Password);
            Preferences preferences = new Preferences{
                UserId = tUsername.Text,
                Password = encryptedPwd
            };
            XmlManager.Serialize(preferences, Global.SettingsFile );
            Global.CurrentPreferences = preferences;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Preferences preferencesData = Global.CurrentPreferences;

            tUsername.Text = preferencesData.UserId;

            if (!string.IsNullOrEmpty(preferencesData.Password))
                tPassword.Password = DataProtector.DecryptData(preferencesData.Password);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
