using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Threading;
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
            InventoryDownload,
            UploadToWantedListVerify,
            UploadToWantedListFinal,
            PriceGuide,
            UpdateOrder
        }

        private static CookieContainer cookies;
        private static readonly string UserAgent = string.Format("BrickLab v{0}",Global.Version);
        private static BackgroundWorker worker;

        public static bool IsBusy { get; private set; }
        internal static Dispatcher Dispatcher { get; set; }

        static BrickClient()
        {
            cookies = new CookieContainer();
        }

        public static bool NeedsLogin()
        {
            return (DateTime.Now - Global.LastLogin).TotalMinutes > 15;
        }

        public static HttpWebResponse PerformRequest(Page page, string[] args, byte[] data)
        {
            return PerformRequest(GetUri(page, args), data);
        }

        public static HttpWebResponse PerformRequest(Uri uri, byte[] data=null)
        {
            // Prepare web request...
            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response;

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = UserAgent;
            request.Timeout = 5000;
            request.CookieContainer = cookies;

            LogEvent.Network.Log(string.Format("Performing request to {0}", uri));
            try
            {
                IsBusy = true;
                if (data != null)
                {
                    request.ContentLength = data.Length;
                    Stream newStream = request.GetRequestStream();
                    // Send the data.
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                }
                else
                    request.ContentLength = 0;

                response = (HttpWebResponse) request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                DisplayMessageBox(
                    "Could not reach host. Check your internet connection. Exception message:\n\n" +
                                ex.Message,
                                "Error",
                                MessageBoxImage.Error);
                LogEvent.Network.Log(ex.Message);
            }
            IsBusy = false;

            return response;
        }


        public static bool Authenticate()
        {
            return !NeedsLogin() || PerformLogin();

        }

        public static bool PerformLogin()
        {
            Preferences prefs = Global.CurrentPreferences;
            if (string.IsNullOrEmpty(prefs.UserId) || string.IsNullOrEmpty(prefs.Password))
            {
                MessageBox.Show("Please save your account information in the preferences first", "Warning", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return false;
            }

            string postData = "frmUsername=" + prefs.UserId;
            postData += ("&frmPassword=" + DataProtector.DecryptData(prefs.Password));
            byte[] data = Encoding.UTF8.GetBytes(postData);

            HttpWebResponse response = PerformRequest(Page.MyBricklink, null, data);

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

                if (response.ResponseUri.LocalPath.Contains("oops"))
                {
                    LogEvent.Network.Log("Login failed");
                    MessageBox.Show(Application.Current.MainWindow,
                    "Login failed. Please check your password","Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
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

        public static string NavigateTo(Page page, bool loginNeeded, string[] args = null)
        {
            if (loginNeeded)
            {
                bool loggedIn = Authenticate();
                if (!loggedIn)
                {
                    LogEvent.Network.Log("Login failed");
                    return string.Empty;
                }
            }

            HttpWebRequest request =
              (HttpWebRequest)WebRequest.Create(GetUri(page,args));

            request.Method = "GET";
            request.CookieContainer = cookies;
            request.UserAgent = UserAgent;
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse) request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                string responseHtml = sr.ReadToEnd();
                response.Close();
                return responseHtml;
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                MessageBox.Show(Application.Current.MainWindow,
                    "Could not reach host. Check your internet connection. Exception message:\n\n" +
                                ex.Message,
                                "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                LogEvent.Network.Log(ex.Message);
                return string.Empty;
            }
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

        static Uri GetUri(Page page, string[] args = null)
        {
            const string host = "http://www.bricklink.com/";
            string address;
            switch (page)
            {

                default:
                case Page.MyBricklink:
                    address = "my.asp";
                    break;

                case Page.UpdateOrder:
                    address = string.Format("orderReceived.asp?a=a&pg={0}&orderFiled=N&srtAsc=DESC", args[0]);
                    break;

                case Page.OrdersReceived:
                    address = string.Format("orderReceived.asp?pg={0}&orderFiled=N&srtAsc=DESC", args[0]);
                    break;

                case Page.OrdersPlaced:
                    address = "orderPlaced.asp";
                    break;

                case Page.InventoryDownload:
                    address = string.Format("catalogDownload.asp{0}", args[0]);
                    break;

                case Page.UploadToWantedListVerify:
                    address = "wantedXMLverify.asp";
                    break;

                case Page.UploadToWantedListFinal:
                    address = "wantedXMLfinal.asp";
                    break;

                case Page.PriceGuide:
                    address = string.Format("catalogPG.asp?P={0}&colorID={1}", args[0], args[1]);
                    break;
            }

            return new Uri(string.Format("{0}{1}", host, address));
        }

        static void DisplayMessageBox(string message, string caption, MessageBoxImage image)
        {
            Dispatcher.BeginInvoke(new Action(
                                       () =>
                                       MessageBox.Show(Application.Current.MainWindow, message, caption, MessageBoxButton.OK,
                                                       image)));
        }
    }
}
