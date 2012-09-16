using System.IO;
using System.Net;
using System.Windows;
using AvengersUtd.BrickLab.Data;
using AvengersUtd.BrickLab.Model;
using AvengersUtd.BrickLab.Settings;

namespace AvengersUtd.BrickLab.View
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class OptionsView : Window
    {
        private string encryptedPassword;
        private Preferences preferencesData;
        public OptionsView()
        {

            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            //string encryptedPwd = DataProtector.EncryptData(tPassword.Password);
            Currency currency = cbCurrency.SelectedItem == null ? Currency.Unknown : (Currency) cbCurrency.SelectedItem;
            
            Preferences preferences = new Preferences{
                UserId = tUsername.Text,
                Password = string.IsNullOrEmpty(encryptedPassword) ? Global.CurrentPreferences.Password : encryptedPassword,
                Currency = currency,
                ProxyAddress = tbProxyServer.Text,
                ProxyPort = int.Parse(tbProxyPort.Text)
            };

            XmlManager.Serialize(preferences, Path.Combine(Global.CurrentDir, Global.SettingsFile));
            Global.CurrentPreferences = preferences;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //tUsername.Text = preferencesData.UserId;
            preferencesData = Global.CurrentPreferences;
            if (preferencesData == null)
                return;
            this.DataContext = preferencesData;
            tPassword.Password = string.IsNullOrEmpty(preferencesData.Password) ? string.Empty : "12345678";
            tPassword.PasswordChanged += (s, args) => encryptedPassword = DataProtector.EncryptData(tPassword.Password);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
