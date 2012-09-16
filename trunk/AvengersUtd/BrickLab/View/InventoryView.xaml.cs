using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvengersUtd.BrickLab.Controls;
using AvengersUtd.BrickLab.Logging;
using AvengersUtd.BrickLab.Model;
using AvengersUtd.BrickLab.ViewModel;

namespace AvengersUtd.BrickLab.View
{
    /// <summary>
    /// Interaction logic for InventoryView.xaml
    /// </summary>
    public partial class InventoryView : UserControl, IView
    {
        private InventoryViewModel iVm;

        public static readonly DependencyProperty SetIdProperty =
            DependencyProperty.Register("SetId", typeof (string), typeof (InventoryView), new UIPropertyMetadata(null));

        public string SetId
        {
            get { return (string) GetValue(SetIdProperty); }
            set { SetValue(SetIdProperty, value); }
        }

        public ViewType ViewType { get { return ViewType.OrdersReceived; } }

        public ViewModelBase ViewModel
        {
            get { return iVm; }
        }



        public InventoryView()
        {
            iVm = new InventoryViewModel();
            DataContext = iVm;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            iVm.DownloadSet(SetId);
        }

        private void InventoryGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LogEvent.System.Log("Test");
        }





    }
}
