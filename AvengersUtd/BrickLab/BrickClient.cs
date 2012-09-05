using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
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
            OrdersPlaced,
            InventoryDownload
        }

        private const string BricklinkAddress = "http://www.bricklink.com";
        private static CookieContainer cookies;
        private static readonly string UserAgent = "BrickLab" + Global.Version;
        private static BackgroundWorker worker;

        public static bool IsBusy { get; private set; }

        static BrickClient()
        {
            cookies = new CookieContainer();
        }

        public static bool NeedsLogin()
        {
            return (DateTime.Now - Global.LastLogin).TotalMinutes > 15;
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
            request.Timeout = 5000;
            if (Global.CurrentPreferences.HasProxy)
                request.Proxy = new WebProxy(Global.CurrentPreferences.ProxyAddress, Global.CurrentPreferences.ProxyPort);
            request.CookieContainer = cookies;

            request.ContentLength = data.Length;
            try
            {
                IsBusy = true;
                Stream newStream = request.GetRequestStream();
                // Send the data.
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                response = (HttpWebResponse) request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                MessageBox.Show(Application.Current.MainWindow,
                    "Could not reach host. Check your internet connection. Exception message:\n\n" +
                                ex.Message,
                                "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            IsBusy = false;


            return response;
            
        }


        public static bool PerformLogin()
        {
            Preferences prefs = Global.CurrentPreferences;
            string postData = "frmUsername=" + prefs.UserId;
            postData += ("&frmPassword=" + DataProtector.DecryptData(prefs.Password));
            byte[] data = Encoding.UTF8.GetBytes(postData);

            HttpWebResponse response = PerformRequest(Page.MyBricklink, string.Empty, data);

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

                if (response.ResponseUri.LocalPath.Contains("oops"))
                {
                    LogEvent.Network.Log("Login failed");
                    return false;
                }
                cookies.Add(response.Cookies);

            
            StreamReader reader = new StreamReader(response.GetResponseStream());
            Global.LastLogin = DateTime.Now;
            LogEvent.Network.Log("Login successful");
            response.Close();
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
            if (Global.CurrentPreferences.HasProxy)
                request.Proxy = new WebProxy(Global.CurrentPreferences.ProxyAddress, Global.CurrentPreferences.ProxyPort);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            response.Close();
            return sr.ReadToEnd();
        }

        public static bool RemoteFileExists(Uri uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Timeout = 1000; //set the timeout to 5 seconds to keep the user from waiting too long for the page to load
                request.Method = "HEAD"; //Get only the header information -- no need to download any content

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                int statusCode = (int)response.StatusCode;
                if (statusCode >= 100 && statusCode < 400) //Good requests
                {
                    return true;
                }
                else if (statusCode >= 500 && statusCode <= 510) //Server Errors
                {
                    LogEvent.Network.Log(String.Format("The remote server has thrown an internal error. Url is not valid: {0}", uri.AbsolutePath));
                    return false;
                }
                response.Close();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError) //400 errors
                {
                    return false;
                }
                else
                {
                    LogEvent.Network.Log(String.Format("Unhandled status [{0}] returned for url: {1}", ex.Status, uri.AbsolutePath), ex);
                }
            }
            catch (Exception ex)
            {
                LogEvent.Network.Log(String.Format("Could not test url {0}.", uri.AbsolutePath), ex);
            }
            return false;
        }

        static string GetUrl(Page page)
        {
            string address = "http://www.bricklink.com/";
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

                case Page.InventoryDownload:
                    address += "catalogDownload.asp";
                    break;
            }

            return address;
        }
    }
}
