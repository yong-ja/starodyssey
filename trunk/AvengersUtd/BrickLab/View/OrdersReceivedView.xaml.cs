﻿using System;
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
using AvengersUtd.BrickLab.Data;
using AvengersUtd.BrickLab.Logging;
using AvengersUtd.BrickLab.Model;
using AvengersUtd.BrickLab.ViewModel;

namespace AvengersUtd.BrickLab.Controls
{
    /// <summary>
    /// Interaction logic for OrdersReceived.xaml
    /// </summary>
    public partial class OrdersReceivedView : UserControl
    {
        private OrdersReceivedViewModel vm;
        public OrdersReceivedView()
        {
            InitializeComponent(); 
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            vm = (OrdersReceivedViewModel)OrdersGrid.DataContext;
            vm.LoadOrdersFromFile(Global.OrdersReceivedFile);
            MainWindowViewModel mwVm = (MainWindowViewModel)DataContext;
            MenuItem actions = new MenuItem("Actions");
            MenuItem synchOrders = new MenuItem("Synchronize orders")
                                   {
                                       Command = vm.SynchOrders,
                                       CommandParameter = vm
                                   };

            actions.Children.Add(synchOrders);
            mwVm.MenuItems.Add(actions);
            Dispatcher.ShutdownStarted += (s, args) => vm.Dispose();
        }


        private void OrdersGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            LogEvent.Network.Write("!");
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            OrdersReceivedViewModel vm = (OrdersReceivedViewModel)OrdersGrid.DataContext;
            vm.Dispose();
        }

    }
}