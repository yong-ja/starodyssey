using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AvengersUtd.BrickLab.Controls;
using AvengersUtd.BrickLab.View;
using MenuItem = AvengersUtd.BrickLab.Controls.MenuItem;

namespace AvengersUtd.BrickLab.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MainWindow window;
        private OrdersReceivedView ordersReceivedView;
        private InventoryView inventoryView;

        public Type Options { get { return typeof(OptionsView); } }
        public Type SetBrowser { get { return typeof (SetBrowserDialog); } }

        public MainWindowViewModel()
        {
            window = (MainWindow)Application.Current.MainWindow;
            CreateMenuItems();
        }

        private ObservableCollection<MenuItem> applicationMenu;

        public ObservableCollection<MenuItem> MenuItems
        {
            get { return applicationMenu; }
            set
            {
                if (applicationMenu == value)
                    return;
                applicationMenu = value;
                RaisePropertyChanged("MenuItem");
            }
        }



        public OrdersReceivedView OrdersReceivedView
        {
            get { return ordersReceivedView ?? (ordersReceivedView = new OrdersReceivedView()); }
        }

        public InventoryView InventoryView
        {
            get { return inventoryView ?? (inventoryView = new InventoryView()); }
        }

        private void CreateMenuItems()
        {
            applicationMenu = new ObservableCollection<MenuItem>();
            MenuItem file = new MenuItem("File");

            MenuItem options = new MenuItem("Options");
            MenuItem preferences = new MenuItem("Preferences")
            {
                Command = ShowDialogCommand,
                CommandParameter = Options
            };

            options.Children.Add(preferences);
            applicationMenu.Add(file);
            applicationMenu.Add(options);
        }

        #region Commands
        public DelegateCommand<Type> ShowDialogCommand
        {
            get
            {
                return new DelegateCommand<Type>(
                    delegate(Type param)
                    {
                        Window dialog = ((Window) Activator.CreateInstance(param));
                        dialog.DataContext = this;
                        dialog.Show();
                    }, null);
            }
        }

        public ICommand CloseDialogCommand
        {
            get { return new DelegateCommand<Window>(param => param.Close(), param => param.IsVisible); }
        }

        public DelegateCommand<Window> ShowWindowCommand
        {
            get { return new DelegateCommand<Window>(param => param.Show(), null); }
        }

        public DelegateCommand<UserControl> ShowUserControlCommand
        {
            get { return new DelegateCommand<UserControl>(window.SwitchTo, null); }
        }

        public ICommand ShowInventoryView
        {
            get
            {
                return new DelegateCommand<string>(delegate(string setId)
                                                   {
                                                       InventoryView.SetId = setId;
                                                       window.SwitchTo(InventoryView);
                                                   }, null);
            }
        }

        
        #endregion

        
    }
}
