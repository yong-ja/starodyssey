using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AvengersUtd.BrickLab;
using AvengersUtd.BrickLab.Data;
using AvengersUtd.BrickLab.Logging;
using AvengersUtd.BrickLab.Settings;
using HtmlAgilityPack;

namespace AvengersUtd
{
    public static class BrickClient
    {
        public enum Page
        {
            MyBricklink,
            OrdersReceived,
            OrdersPlaced
        }

        private const string BricklinkAddress = "http://www.bricklink.com";
        private static CookieContainer cookies;
        private static readonly string UserAgent = "BrickLab" + Global.Version;
        private static BackgroundWorker worker;

        static BrickClient()
        {
            cookies = new CookieContainer();
        }

        public static bool NeedsLogin()
        {
            return (DateTime.Now - Global.LastLogin).TotalMinutes > 15;
        }

        public static Part[] DownloadSetInventory(string setNumber)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            string downloadURL = string.Format("http://www.bricklink.com/catalogItemInv.asp?S={0}", setNumber);
 
            // Creates an HtmlDocument object from an URL
            HtmlDocument document = htmlWeb.Load(downloadURL);
            HtmlNode root = document.DocumentNode;

            // Targets a specific node
            var tableNodes = from table in root.SelectNodes("//table[@class='ta']") select table;
            HtmlNode[] nodes = tableNodes.ToArray();

            return ParseTable(nodes[0]);
        }

        static Part[] ParseTable(HtmlNode table)
        {
            var rows = table.SelectNodes("tr");
            List<Part> parts = new List<Part>();
            foreach (HtmlNode row in rows.Skip(2))
            {
                var cells = from cell in row.SelectNodes("td") select cell;
                HtmlNode[] cellArray = cells.ToArray();
                if (cellArray.Length < 3)
                    continue;
                string imageUrl = cellArray[0].Descendants("img").First().Attributes["src"].Value;
                int quantity = int.Parse(HtmlEntity.DeEntitize(cellArray[1].FirstChild.InnerText).Trim());
                string itemNr = cellArray[2].InnerText;
                Part part = new Part(imageUrl, itemNr, quantity, string.Empty);
                parts.Add(part);
            }

            return parts.ToArray();
        }

        public static HttpWebResponse PerformRequest(Page page, string args, byte[] data)
        {
            // Prepare web request...
            HttpWebRequest request =
                (HttpWebRequest) WebRequest.Create(GetUrl(page) + args);
            HttpWebResponse response;

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = UserAgent;
            request.CookieContainer = cookies;

            request.ContentLength = data.Length;
            Stream newStream = request.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            return (HttpWebResponse) request.GetResponse();
        }

        public static bool PerformLogin()
        {
            Preferences prefs = Global.CurrentPreferences;
            string postData = "frmUsername=" + prefs.UserId;
            postData += ("&frmPassword=" + DataProtector.DecryptData(prefs.Password));
            byte[] data = Encoding.UTF8.GetBytes(postData);

            HttpWebResponse response;

            try
            {
                response = PerformRequest(Page.MyBricklink, string.Empty, data);
                if (response.ResponseUri.LocalPath.Contains("oops"))
                {
                    LogEvent.Network.Log("Login failed");
                    return false;
                }
                cookies.Add(response.Cookies);
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }
            
            StreamReader reader = new StreamReader(response.GetResponseStream());
            Global.LastLogin = DateTime.Now;
            LogEvent.Network.Log("Login successful");
            //DebugWindow dWindow = new DebugWindow {HtmlSource = reader.ReadToEnd()};
            //dWindow.Show();

            return true;
        }

        public static string NavigateTo(Page page, string args = null)
        {
            if (NeedsLogin())
            {
                if (!PerformLogin())
                    return string.Empty;
            }

            HttpWebRequest request =
              (HttpWebRequest)WebRequest.Create(GetUrl(page) + args);

            request.Method = "Get";
            request.CookieContainer = cookies;
            request.UserAgent = UserAgent;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            return sr.ReadToEnd();
        }

        static string GetUrl(Page page)
        {
            string address = "http://www.bricklink.com//";
            switch (page)
            {

                default:
                case Page.MyBricklink:
                    address += "my.asp";
                    break;

                case Page.OrdersReceived:
                    address += "orderReceived.asp";
                    break;

                case Page.OrdersPlaced:
                    address += "orderPlaced.asp";
                    break;
            }

            return address;
        }
    }
}
