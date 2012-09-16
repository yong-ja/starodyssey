using System.Windows.Controls;
using System.Windows.Input;
using AvengersUtd.BrickLab.ViewModel;

namespace AvengersUtd.BrickLab.View
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : UserControl, IView
    {
        public AboutView()
        {
            InitializeComponent();
        }



        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(((TextBlock)sender).Text);
        }

        public ViewModelBase ViewModel
        {
            get { throw new System.NotImplementedException(); }
        }

        public ViewType ViewType
        {
            get { return ViewType.About; }
        }
    }
}
