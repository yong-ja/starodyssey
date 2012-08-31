using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using AvengersUtd.BrickLab.Controls;
using AvengersUtd.BrickLab.DataAccess;
using AvengersUtd.BrickLab.Logging;
using AvengersUtd.BrickLab.Model;
using AvengersUtd.BrickLab.Settings;
using HtmlAgilityPack;

namespace AvengersUtd.BrickLab.ViewModel
{
    public class OrdersReceivedViewModel : ViewModelBase
    {
        private OrderRepository orderRepository;
        private ObservableCollection<OrderViewModel> orders;
        private OrderViewModel selectedOrder;

        public OrdersReceivedViewModel()
        {
            orderRepository = new OrderRepository();
            orders = new ObservableCollection<OrderViewModel>();

            orderRepository.OrderAdded += OnOrderAddedToRepository;
        } 
        
        public OrdersReceivedViewModel(string filename)
        {
            LoadOrdersFromFile(filename);
        }

        private void OnOrderAddedToRepository(object sender, OrderAddedEventArgs e)
        {
            OrderViewModel viewModel = new OrderViewModel(e.NewOrder);
            orders.Add(viewModel);
        }

        public void LoadOrdersFromFile(string filename)
        {
            orderRepository.LoadOrdersFromFile(filename);
            Orders = new ObservableCollection<OrderViewModel>(from o in orderRepository.GetOrders()
                                                              select new OrderViewModel(o));
            Orders.CollectionChanged += Orders_CollectionChanged;
        }
      
        public OrderViewModel SelectedOrder
        {
            get { return selectedOrder; }
            set { selectedOrder = value;
                RaisePropertyChanged("SelectedOrder");
            }
        }

        public ObservableCollection<OrderViewModel> Orders
        {
            get { return orders; }
            private set
            {
                if (orders == value)
                    return;

                orders = value;
                RaisePropertyChanged("Orders");
            }
        }

        public ICommand UpdateOrder
        {
            get { return new DelegateCommand<OrderViewModel>(param => param.Update(), param=>param.CanUpdate()); }
        }

        public ICommand SynchOrders
        {
            get { return new DelegateCommand<OrdersReceivedViewModel>(param => param.DownloadOrders(), null); }
        }

        public void DownloadOrders()
        {
            orderRepository.DownloadOrders();
        }

        void Orders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }


        
    }
}
