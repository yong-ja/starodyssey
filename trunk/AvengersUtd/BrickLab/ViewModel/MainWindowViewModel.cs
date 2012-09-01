using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AvengersUtd.BrickLab.Controls;
using MenuItem = AvengersUtd.BrickLab.Controls.MenuItem;

namespace AvengersUtd.BrickLab.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MainWindow window;
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


        private OrdersReceived ordersReceived;

        public OrdersReceived OrdersReceived
        {
            get
            {
                if (ordersReceived == null)
                    ordersReceived = new OrdersReceived();
                return ordersReceived;
            }
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
        private DelegateCommand<Type> ShowDialogCommand
        {
            get { return new DelegateCommand<Type>(param => ((Window) Activator.CreateInstance(param)).ShowDialog(), null); }
        }

        public DelegateCommand<Window> ShowWindowCommand
        {
            get { return new DelegateCommand<Window>(param => param.Show(), null); }
        }

        public DelegateCommand<UserControl> ShowUserControlCommand
        {
            get { return new DelegateCommand<UserControl>(window.SwitchTo, null); }
        }

        public Type Options { get { return typeof(Options); } }
        #endregion

        
    }
}
