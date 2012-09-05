using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AvengersUtd.BrickLab.Model;

namespace AvengersUtd.BrickLab.ViewModel
{
    public class OrderViewModel : ViewModelBase, IEditableObject
    {
        

        private readonly Order order;
        private bool inEdit;
        private OrderViewModel backupCopy;

        public OrderViewModel()
        {
            order = new Order();
        }

        public OrderViewModel(Order order)
        {
            this.order = order;
        }

        public int Id
        {
            get
            {
                return order.Id;
            }
            set
            {
                if (order.Id == value)
                    return;
                order.Id = value;
                RaisePropertyChanged("Id");
            }
        }

        public DateTime Date
        {
            get { return order.Date; }
            set
            {
                if (order.Date == value)
                    return;
                order.Date = value;
                RaisePropertyChanged("Date");
            }
        }

        public string BuyerUserId
        {
            get { return order.BuyerUserId; }
            set
            {
                if (order.BuyerUserId == value)
                    return;
                order.BuyerUserId = value;
                RaisePropertyChanged("BuyerUserId");
            }
        }

        public float Shipping
        {
            get { return order.Shipping; }
            set
            {
                if (Math.Abs(order.Shipping - value) < Global.Epsilon)
                    return;
                order.Shipping = value;
                RaisePropertyChanged("Shipping");
                RaisePropertyChanged("OrderGrandTotal");
            }
        }

        public float Insurance
        {
            get { return order.Insurance; }
            set
            {
                if (Math.Abs(order.Insurance - value) < Global.Epsilon)
                    return;
                order.Insurance = value;
                RaisePropertyChanged("Insurance");
                RaisePropertyChanged("OrderGrandTotal");
            }
        }

        public float AdditionalCharge
        {
            get { return order.AdditionalCharge; }
            set
            {
                if (Math.Abs(order.AdditionalCharge - value) < Global.Epsilon)
                    return;
                order.AdditionalCharge = value;
                RaisePropertyChanged("AdditionalCharge");
                RaisePropertyChanged("GrandTotal");
            }
        }

        public float CouponCredit
        {
            get { return order.CouponCredit; }
            set
            {
                if (Math.Abs(order.CouponCredit - value) < Global.Epsilon)
                    return;
                order.CouponCredit = value;
                RaisePropertyChanged("CouponCredit");
            }
        }

        public float ExtraCredit
        {
            get { return order.ExtraCredit; }
            set
            {
                if (Math.Abs(order.ExtraCredit - value) < Global.Epsilon)
                    return;
                order.ExtraCredit = value;
                RaisePropertyChanged("ExtraCredit");
                RaisePropertyChanged("GrandTotal");
            }
        }

        public float OrderTotal
        {
            get { return order.OrderTotal; }
            set
            {
                if (Math.Abs(order.OrderTotal - value) < Global.Epsilon)
                    return;
                order.OrderTotal = value;
                RaisePropertyChanged("OrderTotal");
            }
        }

        public float GrandTotal
        {
            get { return order.GrandTotal; }
        }

        public OrderStatus Status
        { get { return order.Status; }
            set
            {
                if (order.Status == value)
                    return;
                order.Status = value;
                RaisePropertyChanged("Status");
            }
        }

        public bool IsComplete
        {
            get { return order.IsComplete; }
            set
            {
                if (order.IsComplete == value)
                    return;
                order.IsComplete = value;
                RaisePropertyChanged("IsComplete");
            }
        }

        public int Items
        {
            get { return order.Items; }
            set
            {
                if (order.Items == value)
                    return;
                order.Items = value;
                RaisePropertyChanged("Items");
            }
        }

        public int Lots
        {
            get { return order.Lots; }
            set
            {
                if (order.Lots == value)
                    return;
                order.Lots = value;
                RaisePropertyChanged("Lots");
            }
        }

        public bool IsDirty { get; set; }

        internal Order OrderModelObject
        {
            get { return order; }
        }

        private OrderViewModel GetCopy()
        {
            OrderViewModel ovm = new OrderViewModel
                                 {
                                     Id = Id,
                                     BuyerUserId = BuyerUserId,
                                     Date = Date,
                                     Items = Items,
                                     Lots = Lots,
                                     Shipping = Shipping,
                                     Insurance = Insurance,
                                     AdditionalCharge = AdditionalCharge,
                                     CouponCredit = CouponCredit,
                                     ExtraCredit = ExtraCredit,
                                     OrderTotal = OrderTotal,
                                     Status = Status,
                                     IsComplete = IsComplete
                                 };
            return ovm;

        }
        
        public static string CreateUpdateString(OrderViewModel newItem, OrderViewModel oldItem)
        {
            Contract.Requires(oldItem != null);
            Contract.Requires(newItem != null);
            StringBuilder sb = new StringBuilder();
            int id = oldItem.Id;
            sb.AppendFormat("nH{0}={1}&", id, newItem.Shipping > 0 ? newItem.Shipping.ToString() : string.Empty);
            sb.AppendFormat("oH{0}={1}&", id, oldItem.Shipping > 0 ? oldItem.Shipping.ToString() : string.Empty);
            sb.AppendFormat("nI{0}={1}&", id, newItem.Insurance >0 ? newItem.Insurance.ToString() : string.Empty);
            sb.AppendFormat("oI{0}={1}&", id, oldItem.Insurance >0 ? oldItem.Insurance.ToString() : string.Empty);
            sb.AppendFormat("nD{0}={1}&", id, newItem.AdditionalCharge > 0 ? newItem.AdditionalCharge.ToString() : string.Empty);
            sb.AppendFormat("oD{0}={1}&", id, oldItem.AdditionalCharge > 0 ? oldItem.AdditionalCharge.ToString() : string.Empty);
            sb.AppendFormat("nC{0}={1}&", id, newItem.ExtraCredit >0 ? newItem.ExtraCredit.ToString() : string.Empty);
            sb.AppendFormat("oC{0}={1}&", id, oldItem.ExtraCredit >0 ? oldItem.ExtraCredit.ToString() : string.Empty);
            sb.AppendFormat("nS{0}={1}&", id, (int)newItem.Status);
            sb.AppendFormat("oS{0}={1}&", id, (int)oldItem.Status);
            sb.AppendFormat("oI={0}&", id);
            return sb.ToString();

        }

        public void Update()
        {
            if (!CanUpdate())
                return;
            //Contract.Requires(backupCopy != null);
            if (BrickClient.NeedsLogin())
                BrickClient.PerformLogin();
            string queryString = CreateUpdateString(this, backupCopy);
            byte[] data = Encoding.UTF8.GetBytes(queryString);
            HttpWebResponse response = BrickClient.PerformRequest(BrickClient.Page.OrdersReceived, "?a=a&pg=1&orderFiled=N&srtAsc=DESC", data);
            DebugWindow dWindow = new DebugWindow();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            dWindow.HtmlSource = reader.ReadToEnd();
            dWindow.Show();
        }

        public bool CanUpdate()
        {
            
            return !IsComplete && IsDirty;
        }

        #region IEditableObject
        public void BeginEdit()
        {
            if (inEdit) return;
            inEdit = true;
            backupCopy = GetCopy();
        }

        public void CancelEdit()
        {
            if (!inEdit) return;
            inEdit = false;

            Shipping = backupCopy.Shipping;
            Insurance = backupCopy.Insurance;
            AdditionalCharge = backupCopy.AdditionalCharge;
            ExtraCredit = backupCopy.ExtraCredit;
            Status = backupCopy.Status;
            OrderTotal = backupCopy.OrderTotal;
            IsDirty = false;
        }

        public void EndEdit()
        {
            if (!inEdit) return;
            IsDirty = !order.Equals(backupCopy.order);
            inEdit = false;
        }
        #endregion


    }
}
