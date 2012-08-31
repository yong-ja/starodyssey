using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AvengersUtd.BrickLab.Logging;
using AvengersUtd.BrickLab.Model;
using AvengersUtd.BrickLab.Settings;
using AvengersUtd.BrickLab.ViewModel;
using HtmlAgilityPack;

namespace AvengersUtd.BrickLab.DataAccess
{
    public class OrderRepository
    {
        private readonly List<Order> orders;
        private static int orderPages;
        private static int ordersPerPage;
        private static int ordersTotal;

        public event EventHandler<OrderAddedEventArgs> OrderAdded;

        public OrderRepository()
        {
            orders = new List<Order>();
        }

        protected void OnOrderAdded(OrderAddedEventArgs e)
        {
            if (OrderAdded != null)
                OrderAdded(this, e);
        }

        public List<Order> GetOrders()
        {
            return new List<Order>(orders);
        }


        public void LoadOrdersFromFile(string ordersReceivedFile)
        {
            Contract.Requires(File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ordersReceivedFile)));

            var xmlOrders = XmlManager.DeserializeCollection<Order>(Global.OrdersReceivedFile);

            foreach (Order o in xmlOrders)
                AddOrder(o);
        }

        public void AddOrder(Order order)
        {
            Contract.Requires(order != null);
            if (orders.Contains(order)) return;
            orders.Add(order);
            OnOrderAdded(new OrderAddedEventArgs(order));
        }

        public bool ContainsOrder(Order order)
        {
            Contract.Requires(order != null);
            return orders.Contains(order);
        }

        public void DownloadOrders()
        {
            string responseHtml = BrickClient.NavigateTo(BrickClient.Page.OrdersReceived);

            if (String.IsNullOrEmpty(responseHtml))
                throw new InvalidOperationException();

            // Creates an HtmlDocument object from an URL
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseHtml);

            Regex regex = new Regex(
                @"Total (?'total'\d+) Orders Received.  Page \d+ of (?'pages'\d+) \(Showing (?'perPage'\d+) Orders Per Page\)");
            string orderNodeText = document.DocumentNode.SelectNodes("//font[@face='Tahoma, Arial']")[0].InnerText.Split('\n')[0];

            Match match = regex.Match(orderNodeText);
            orderPages = Int32.Parse(match.Groups["pages"].Value);
            ordersTotal = Int32.Parse(match.Groups["total"].Value);
            ordersPerPage = Int32.Parse(match.Groups["perPage"].Value);
            

            for (int i = 0; i < orderPages; i++)
            {
                // Once the first page is complete, navigate to the other ones
                if (i > 0)
                {
                    responseHtml = BrickClient.NavigateTo(BrickClient.Page.OrdersReceived,
                                                          String.Format("?pg={0}&orderFiled=N&srtAsc=DESC", i + 1));
                    document.LoadHtml(responseHtml);
                }

                HtmlNode root = document.DocumentNode;
                var tableNodes = from table in root.SelectNodes("//table[@class='tableT1']") select table;
                HtmlNode[] nodes = tableNodes.ToArray();
                var newOrders = ParseTable(nodes[0]);
                foreach (Order o in newOrders)
                    AddOrder(o);
            }

            //DebugWindow dWindow = new DebugWindow { HtmlSource = responseHtml };
            //dWindow.Show();
            XmlManager.Serialize(orders, "orders.xml");
        }

        static IEnumerable<Order> ParseTable(HtmlNode table)
        {
            var rows = table.SelectNodes("tr");
            List<Order> parsedOrders = new List<Order>();
            Regex regex = new Regex(@"(?<parts>\d+)\s\((?<lots>\d+)\)");

            List<HtmlNode> headerCells = rows.First().SelectNodes("td").ToList();

            int indexId = headerCells.FindIndex(c => c.InnerText.Equals("ID"));
            int indexDate = headerCells.FindIndex(c => c.InnerText.Equals("Date"));
            int indexBuyer = headerCells.FindIndex(c => c.InnerText.Equals("Buyer"));
            int indexItems = headerCells.FindIndex(c => c.InnerText.StartsWith("Items"));
            int indexShip = headerCells.FindIndex(c => c.InnerText.Equals("Ship."));
            int indexAddCharge = headerCells.FindIndex(c => c.InnerText.StartsWith("Add."));
            int indexInsur = headerCells.FindIndex(c => c.InnerText.Equals("Insur."));
            int indexCredit = headerCells.FindIndex(c => c.InnerText.Equals("Credit"));
            int indexOrder = headerCells.FindIndex(c => c.InnerText.EndsWith("Total"));
            int indexStatus = headerCells.FindIndex(c => c.InnerText.EndsWith("Status"));

            int count = rows.Count;
            for (int i = 1; i < count - 1; i++)
            {
                HtmlNode row = rows[i];
                HtmlNode[] cells = (from cell in row.SelectNodes("td") select cell).ToArray();
                string orderId = cells[indexId].ChildNodes[0].InnerText;
                DateTime date = DateTime.Parse(cells[indexDate].InnerText);
                string userId = cells[indexBuyer].ChildNodes[0].InnerText;
                string items = HtmlEntity.DeEntitize(cells[indexItems].InnerText);
                Match match = regex.Match(items);

                int parts = Int32.Parse(match.Groups["parts"].Value); ;
                int lots = Int32.Parse(match.Groups["lots"].Value);

                float value;
                bool result = Single.TryParse(cells[indexShip].ChildNodes[0].Attributes["VALUE"].Value, out value);
                float shipping = result ? value : 0;

                result = Single.TryParse(cells[indexInsur].ChildNodes[0].Attributes["VALUE"].Value, out value);
                float insurance = result ? value : 0;

                HtmlNode cellCredit = cells[indexCredit];
                float couponCredit = 0;
                float extraCredit;
                if (cellCredit.ChildNodes.Count == 2)
                {
                    result = Single.TryParse(HtmlEntity.DeEntitize(cellCredit.ChildNodes[0].InnerText).Trim(), out value);
                    couponCredit = result ? value : 0;
                    result = Single.TryParse(cells[indexCredit].ChildNodes[1].Attributes["VALUE"].Value, out value);
                    extraCredit = result ? value : 0;
                }
                else
                {
                    result = Single.TryParse(cells[indexCredit].ChildNodes[0].Attributes["VALUE"].Value, out value);
                    extraCredit = result ? value : 0;
                }

                result = Single.TryParse(cells[indexAddCharge].ChildNodes[0].Attributes["VALUE"].Value, out value);
                float addCharge = result ? value : 0;

                result = Single.TryParse(cells[indexOrder].ChildNodes[0].InnerText, out value);
                float orderTotal = result ? value : 0;

                bool isComplete;
                OrderStatus orderStatus;
                if (cells[indexStatus].ChildNodes.Count == 2)
                {
                    // Is a "completed" order;
                    isComplete = true;
                    orderStatus = OrderStatus.Completed;
                }
                else
                {
                    string orderStatusValue =
                        cells[indexStatus].ChildNodes[0].ChildNodes.First(o => o.Attributes.Contains("selected")).
                        Attributes["Value"].Value;
                    int statusId = Int32.Parse(orderStatusValue);
                    orderStatus = (OrderStatus)statusId;
                    isComplete = false;
                }

                LogEvent.Network.Write(cells.Count().ToString());

                Order order = new Order
                {
                    Id = Int32.Parse(orderId),
                    Date = date,
                    BuyerUserId = userId,
                    Items = parts,
                    Lots = lots,
                    Shipping = shipping,
                    Insurance = insurance,
                    AdditionalCharge = addCharge,
                    CouponCredit = couponCredit,
                    ExtraCredit = extraCredit,
                    OrderTotal = orderTotal,
                    Status = orderStatus,
                    IsComplete = isComplete
                };
                parsedOrders.Add(order);
            }

            return parsedOrders;
        }
    }
}
