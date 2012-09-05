using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using AvengersUtd.BrickLab.Controls;
using AvengersUtd.BrickLab.DataAccess;
using AvengersUtd.BrickLab.Logging;
using AvengersUtd.BrickLab.Settings;

namespace AvengersUtd.BrickLab.ViewModel
{
    public class OrdersReceivedViewModel : ViewModelBase, IDisposable
    {
        private bool disposed;
        private readonly OrderRepository orderRepository;
        private ObservableCollection<OrderViewModel> orders;
        private OrderViewModel selectedOrder;
        private readonly Dispatcher dispatcher;

        public OrdersReceivedViewModel()
        {
            orderRepository = new OrderRepository();
            orders = new ObservableCollection<OrderViewModel>();
            dispatcher = Dispatcher.CurrentDispatcher;

            orderRepository.OrderAdded +=
                (sender, e) => dispatcher.BeginInvoke(new Action(() => OnOrderAddedToRepository(sender, e)));
            orderRepository.OrderChanged += (sender, e) =>
                                            dispatcher.BeginInvoke(new Action(() => OnOrderChangedInRepository(sender, e)),
                                                                   DispatcherPriority.Normal);
        }

        public OrdersReceivedViewModel(string filename)
        {
            LoadOrdersFromFile(filename);
        }

        private void OnOrderChangedInRepository(object sender, OrderChangedEventArgs e)
        {
            LogEvent.OrderChanged.Log(e.OldOrder.Id);
            orders.Remove(orders.First(o => o.Id == e.OldOrder.Id));
            orders.Add(new OrderViewModel(e.NewOrder));

        }



        private void OnOrderAddedToRepository(object sender, OrderAddedEventArgs e)
        {
            OrderViewModel viewModel = new OrderViewModel(e.NewOrder);
            OrderViewModel ovm = orders.FirstOrDefault(o => o.Id == viewModel.Id);
            if (ovm != null)
            {
                if (!ovm.OrderModelObject.Equals(viewModel.OrderModelObject))
                {
                    orders.Remove(ovm);
                    orders.Add(viewModel);
                }
            }
            else
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
            set
            {
                selectedOrder = value;
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
            get
            {
                return new DelegateCommand<OrderViewModel>(
                    param => Dispatcher.CurrentDispatcher.BeginInvoke(new Action(param.Update), DispatcherPriority.Background),
                    null); // Due to the lack of RowEditEnded
            }
        }

        public ICommand SynchOrders
        {
            get { return new DelegateCommand<OrdersReceivedViewModel>(param => param.DownloadOrders(), null); }
        }

        public void DownloadOrders()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
                             {
                                 orderRepository.DownloadOrders();
                             };
            Mouse.OverrideCursor = Cursors.Wait;
            worker.RunWorkerCompleted += (sender, e) => Mouse.OverrideCursor = Cursors.Arrow;
            worker.RunWorkerAsync();
        }

        private void Orders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                orderRepository.SaveToDisk();
            }
            disposed = true;
        }

        ~OrdersReceivedViewModel()
        {
            Dispose(false);
        }
    }
}
