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
        private Type currentView;
        private OrdersReceivedView ordersReceivedView;
        private InventoryView inventoryView;

        public Type Options { get { return typeof(OptionsView); } }
        public Type SetBrowser { get { return typeof (SetBrowserDialog); } }

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

            file.Children.Add(exportToWantedList);

            options.Children.Add(preferences);
            applicationMenu.Add(file);
            applicationMenu.Add(options);
        }

        void RequestSwitch(UserControl control)
        {
            currentView = control.GetType();
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
                           CanExecuteMethod = () => currentView == typeof (InventoryView),
                           DefaultCommand = ((InventoryViewModel)InventoryView.DataContext).ExportInventoryToWantedListCommand
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
