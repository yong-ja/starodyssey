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
            Preferences preferences = new Preferences{
                UserId = tUsername.Text,
                Password = string.IsNullOrEmpty(encryptedPassword) ? Global.CurrentPreferences.Password : encryptedPassword,
                Currency = (Currency)cbCurrency.SelectedItem,
                ProxyAddress = tbProxyServer.Text,
                ProxyPort = int.Parse(tbProxyPort.Text)
            };
            XmlManager.Serialize(preferences, Global.SettingsFile );
            Global.CurrentPreferences = preferences;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //tUsername.Text = preferencesData.UserId;
            preferencesData = Global.CurrentPreferences;
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
