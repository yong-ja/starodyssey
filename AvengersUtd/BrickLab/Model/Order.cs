using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using AvengersUtd.BrickLab.Data;
using AvengersUtd.BrickLab.ViewModel;

namespace AvengersUtd.BrickLab.Model
{
    public enum OrderStatus
    {
        Pending = 1,
        Processing = 6,
        Ready = 2,
        Paid = 3,
        Packed = 5,
        Shipped = 7,
        Received = 8,
        Completed = 9
    }

    public class Order
    {
        private float shipping;
        private float insurance;
        private float additionalCharge;
        private float extraCredit;

        [XmlAttribute]
        public int Id { get; set; }
        [XmlIgnore]
        public DateTime Date { get; internal set;}

        [XmlAttribute("Date")]
        public string DateTimeString
        {
            get
            {
                return Date.ToString("MMM d, yyyy");
            }
            set
            {
                DateTime date;
                DateTime.TryParse(value, out date);
                Date = date;
            }
        }

        [XmlAttribute("Buyer")]
        public string BuyerUserId { get; set; }
        [XmlAttribute]
        public int Items { get;  set; }
        [XmlAttribute]
        public int Lots { get;  set; }



        [XmlAttribute]
        public float Shipping { get; set; }


        [XmlAttribute]
        public float Insurance { get; set; }



        [XmlAttribute]
        public float AdditionalCharge { get; set; }

        [XmlAttribute]
        public float CouponCredit { get;  set; }

        [XmlAttribute]
        public float ExtraCredit { get; set; }

        [XmlAttribute]
        public float OrderTotal { get;  set; }
        [XmlAttribute]
        public OrderStatus Status { get;  set; }
        [XmlAttribute]
        public bool IsComplete { get; set; }

        [XmlIgnore]
        public float GrandTotal
        {
            get { return OrderTotal + Shipping + Insurance + AdditionalCharge - (CouponCredit + ExtraCredit); }
        }

        public void UpdateShipping(float newValue)
        {
            //string thisUpdate = GetUpdateString();
            //var orders = OrderManager.GetOrders();
            //foreach (Order o in orders.Skip(1).Take(19))
            //    thisUpdate += o.NoUpdateString();
        //    byte[] data = Encoding.UTF8.GetBytes(thisUpdate);

        //    HttpWebResponse response = BrickClient.PerformRequest(BrickClient.Page.OrdersReceived, "?a=a&pg=1&orderFiled=N&srtAsc=DESC", data);
        //    DebugWindow dWindow = new DebugWindow();
        //    StreamReader reader = new StreamReader(response.GetResponseStream());
        //    dWindow.HtmlSource = reader.ReadToEnd();
        //    dWindow.Show();
        //
        }



        string NoUpdateString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("nH{0}={1}&", Id, Shipping > 0 ? Shipping.ToString() : string.Empty);
            sb.AppendFormat("oH{0}={1}&", Id, Shipping > 0 ? Shipping.ToString() : string.Empty);
            sb.AppendFormat("nI{0}={1}&", Id, Insurance > 0 ? Insurance.ToString() : string.Empty);
            sb.AppendFormat("oI{0}={1}&", Id, Insurance >0 ? Insurance.ToString() : string.Empty);
            sb.AppendFormat("nD{0}={1}&", Id, AdditionalCharge >0 ? AdditionalCharge.ToString() : string.Empty);
            sb.AppendFormat("oD{0}={1}&", Id, AdditionalCharge >0 ? AdditionalCharge.ToString() : string.Empty);
            sb.AppendFormat("nC{0}={1}&", Id, ExtraCredit >0 ? ExtraCredit.ToString() : string.Empty);
            sb.AppendFormat("oC{0}={1}&", Id, ExtraCredit >0 ? ExtraCredit.ToString() : string.Empty);
            sb.AppendFormat("nS{0}={1}&", Id, (int)Status);
            sb.AppendFormat("oS{0}={1}&", Id, (int)Status);
            sb.AppendFormat("oI={0}&", Id);

            string queryString = sb.ToString();

            return queryString;

        }

        #region Equality members
        public bool Equals(Order other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id && other.Date.Equals(Date) && Equals(other.BuyerUserId, BuyerUserId) && other.Items == Items && other.Insurance.Equals(Insurance) && other.Lots == Lots && other.Shipping.Equals(Shipping) && other.AdditionalCharge.Equals(AdditionalCharge) && other.CouponCredit.Equals(CouponCredit) && other.ExtraCredit.Equals(ExtraCredit) && other.OrderTotal.Equals(OrderTotal) && Equals(other.Status, Status) && other.IsComplete.Equals(IsComplete);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Order)) return false;
            return Equals((Order)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Id;
                result = (result * 397) ^ Date.GetHashCode();
                result = (result * 397) ^ (BuyerUserId != null ? BuyerUserId.GetHashCode() : 0);
                result = (result * 397) ^ Items;
                result = (result * 397) ^ Insurance.GetHashCode();
                result = (result * 397) ^ Lots;
                result = (result * 397) ^ Shipping.GetHashCode();
                result = (result * 397) ^ AdditionalCharge.GetHashCode();
                result = (result * 397) ^ CouponCredit.GetHashCode();
                result = (result * 397) ^ ExtraCredit.GetHashCode();
                result = (result * 397) ^ OrderTotal.GetHashCode();
                result = (result * 397) ^ Status.GetHashCode();
                result = (result * 397) ^ IsComplete.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(Order left, Order right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Order left, Order right)
        {
            return !Equals(left, right);
        } 
        #endregion
    }
}
