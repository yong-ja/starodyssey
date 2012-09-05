using System.Windows;

namespace AvengersUtd.BrickLab.View
{
    /// <summary>
    /// Interaction logic for SetBrowserView.xaml
    /// </summary>
    public partial class SetBrowserDialog : Window
    {
        public SetBrowserDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
