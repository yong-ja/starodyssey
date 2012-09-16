using System.IO;
using System.Windows;
using System.Windows.Controls;
using AvengersUtd.BrickLab.ViewModel;
using MenuItem = AvengersUtd.BrickLab.Controls.MenuItem;

namespace AvengersUtd.BrickLab.View
{
    /// <summary>
    /// Interaction logic for OrdersReceived.xaml
    /// </summary>
    public partial class OrdersReceivedView : UserControl, IView
    {
        private OrdersReceivedViewModel vm;

        public ViewModelBase ViewModel
        {
            get { return vm; }
        }

        public ViewType ViewType { get { return ViewType.OrdersReceived; } }

        public OrdersReceivedView()
        {
            vm = new OrdersReceivedViewModel();
            DataContext = vm;
            InitializeComponent(); 
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
            vm.LoadOrdersFromFile(Path.Combine(Global.CurrentDir, Global.OrdersReceivedFile));
            Dispatcher.ShutdownStarted += (s, args) => vm.Dispose();
        }


        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            vm.Dispose();
        }

    }
}
