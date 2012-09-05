using System.Windows;
using AvengersUtd.BrickLab.Data;
using AvengersUtd.BrickLab.Settings;

namespace AvengersUtd.BrickLab.View
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class OptionsView : Window
    {
        public OptionsView()
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
