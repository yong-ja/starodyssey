using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using AvengersUtd.BrickLab.Controls;
using AvengersUtd.BrickLab.Data;
using System.Linq;
using AvengersUtd.BrickLab.Settings;
using AvengersUtd.BrickLab.ViewModel;
using MenuItem = AvengersUtd.BrickLab.Controls.MenuItem;

namespace AvengersUtd.BrickLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        private readonly ObservableCollection<MenuItem> applicationMenu;


        public MainWindow()
        {
            InitializeComponent();
            //applicationMenu = new ObservableCollection<MenuItem>();
            //applicationMenu.CollectionChanged += (sender, e) => OnPropertyChanged(new DependencyPropertyChangedEventArgs("MenuItems"));
            //CreateMenuItems();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);

        }

        

        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                return applicationMenu;
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Global.Init();
            Title += Global.Version;

            //InventoryGrid.ItemsSource = bc.DownloadSetInventory("8028-1");
            string response;
            //bool result = BrickClient.PerformLogin();
            //if (result)
            //    OrderManager.ParseHtml();
            //OrderManager.GetOrders().First(o => o.Id == 2991284).UpdateShipping(1.51f);
        }

        


        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Options options = new Options();
            options.ShowDialog(); 
        }

        private void OrdersReceived_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DockPanel.Children.RemoveAt(DockPanel.Children.Count - 1);
            DockPanel.Children.Add(new OrdersReceived());
        }

        public void SwitchTo(UserControl control)
        {
            Container.Content = control;
            foreach (ImageButton imgButton in SidePanel.Children)
                if (imgButton.CommandParameter != control)
                    imgButton.IsChecked = false;

        }

    }
}
