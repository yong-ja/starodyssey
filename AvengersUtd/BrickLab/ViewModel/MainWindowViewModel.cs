using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AvengersUtd.BrickLab.Controls;
using AvengersUtd.BrickLab.Model;
using AvengersUtd.BrickLab.View;
using Condition = AvengersUtd.BrickLab.Model.Condition;
using MenuItem = AvengersUtd.BrickLab.Controls.MenuItem;

namespace AvengersUtd.BrickLab.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MainWindow window;
        private OrdersReceivedView ordersReceivedView;
        private InventoryView inventoryView;
        private AboutView aboutView;

        public Type Options { get { return typeof(OptionsView); } }
       
        public MainWindowViewModel()
        {
            window = (MainWindow)Application.Current.MainWindow;
            inventoryView = new View.InventoryView();
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

        public string Version { get { return Global.Version; } }

        public OrdersReceivedView OrdersReceivedView
        {
            get { return ordersReceivedView ?? (ordersReceivedView = new OrdersReceivedView()); }
        }

        public InventoryView InventoryView
        {
            get { return inventoryView ?? (inventoryView = new InventoryView()); }
        }

        public AboutView AboutView
        {
            get { return aboutView ?? (aboutView = new AboutView()); }
        }

        private void CreateMenuItems()
        {
            applicationMenu = new ObservableCollection<MenuItem>();
            MenuItem file = new MenuItem("File");
            MenuItem exportToWantedList = new MenuItem("Export to Wanted List")
                                          {
                                              Command = ShowInputDialogCommand,
                                              CommandParameter = ExportToWantedListVM
                                          };

            MenuItem options = new MenuItem("Options");
            MenuItem preferences = new MenuItem("Preferences")
            {
                Command = ShowDialogCommand,
                CommandParameter = Options
            };

            var orVm = ((OrdersReceivedViewModel) OrdersReceivedView.ViewModel);
            MenuItem actions = new MenuItem("Actions");
            MenuItem synchOrders = new MenuItem("Synchronize orders")
            {
                Command = orVm.SynchOrders,
                CommandParameter = orVm
            };

            var iVm = ((InventoryViewModel) InventoryView.ViewModel);
            MenuItem edit = new MenuItem("Edit");
            MenuItem setPrice = new MenuItem("Set price to...");
            MenuItem last6Min = new MenuItem("Last 6 months minimum price")
                                {
                                    Command = iVm.SetPriceCommand,
                                    CommandParameter = new PriceInfo(Condition.New, PriceInfoType.Min)
                                };
            MenuItem last6Avg= new MenuItem("Last 6 months average price")
            {
                Command = iVm.SetPriceCommand,
                CommandParameter = new PriceInfo(Condition.New, PriceInfoType.Average)
            };
            

            file.Children.Add(exportToWantedList);
            actions.Children.Add(synchOrders);
            options.Children.Add(preferences);
            edit.Children.Add(setPrice);
            setPrice.Children.Add(last6Min);
            setPrice.Children.Add(last6Avg);
            applicationMenu.Add(file);
            applicationMenu.Add(edit);
            applicationMenu.Add(actions);
            applicationMenu.Add(options);
        }

        void RequestSwitch(UserControl control)
        {
            Global.CurrentView = ((IView) control).ViewType;
            window.SwitchTo(control);
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
                        dialog.ShowDialog();
                    }, null);
            }
        }

        public DelegateCommand<InputDialogViewModel> ShowInputDialogCommand
        {
            get
            {
                return new DelegateCommand<InputDialogViewModel>(
                    delegate(InputDialogViewModel param)
                    {
                        Window dialog = ((Window) Activator.CreateInstance(param.InputDialogType));
                        dialog.DataContext = param;
                        dialog.ShowDialog();
                    },
                    param => param.CanExecuteMethod());
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
            get { return new DelegateCommand<UserControl>(
                RequestSwitch, 
                delegate { return !BrickClient.IsBusy; }); }
        }

        public ICommand ShowInventoryView
        {
            get
            {
                return new DelegateCommand<string>(delegate(string setId)
                                                   {
                                                       InventoryView.SetId = setId;
                                                       RequestSwitch(InventoryView);
                                                   }, 
                                                   (s) => !string.IsNullOrEmpty(s));
            }
        }

        
        #endregion

        #region InputDialogViewModels
        InputDialogViewModel ExportToWantedListVM
        {
            get
            {
                return new InputDialogViewModel
                       {
                           Label = "Wanted List Id",
                           Description = "Please Enter a Wanted List Id",
                           CanExecuteMethod = () => Global.CurrentView == ViewType.InventoryView,
                           DefaultCommand = ((InventoryViewModel)InventoryView.ViewModel).ExportInventoryToWantedListCommand
                       };
            }
        }

        public InputDialogViewModel ChooseSetVM
        {
            get
            {
                return new InputDialogViewModel
                {
                    Label = "Set",
                    Description = "Please enter a set number, e.g.: 1234-1",
                    DefaultCommand = ShowInventoryView,
                    CanExecuteMethod = () => true
                };
            }
        }

            #endregion


    }
}
