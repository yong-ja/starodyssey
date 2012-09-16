using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using AvengersUtd.BrickLab.Controls;
using AvengersUtd.BrickLab.DataAccess;
using AvengersUtd.BrickLab.Model;
using AvengersUtd.BrickLab.Model.XmlWrappers;
using AvengersUtd.BrickLab.Settings;
using HtmlAgilityPack;
using System.Web;
using AvengersUtd.BrickLab.Logging;

namespace AvengersUtd.BrickLab.ViewModel
{
    public class InventoryViewModel : ViewModelBase
    {
        private readonly Inventory inventory;
        private ObservableCollection<ItemViewModel> partList;

        public ICommand ExportInventoryToWantedListCommand { get
        {
            return new DelegateCommand<string>(ExportToWantedList, delegate { return !BrickClient.IsBusy; });
        } }

        public InventoryViewModel()
        {
            inventory = new Inventory();
        }

        public ObservableCollection<ItemViewModel> PartList
        {
            get { return partList; }
            private set
            {
                if (partList == value)
                    return;

                partList = value;
                RaisePropertyChanged("PartList");
            }
        }

        private IEnumerable<ItemViewModel> selectedItems;
        public IEnumerable<ItemViewModel> SelectedItems
        {
            get { return selectedItems; }
            internal set
            {
                if (selectedItems == value)
                    return;
                selectedItems = value;
                RaisePropertyChanged("SelectedItems");
            }
        }

        public void DownloadSet(string setId)
        {
            Contract.Requires(!string.IsNullOrEmpty(setId));
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate
                             {
                                 inventory.DownloadSetInventory(setId);
                                 PartList = new ObservableCollection<ItemViewModel>(from p in inventory.GetParts()
                                                                                    select new ItemViewModel(p));
                             };
            Mouse.OverrideCursor = Cursors.Wait;
            worker.RunWorkerCompleted += (sender, e) => Dispatcher.BeginInvoke(new Action(delegate
                                                                               { Mouse.OverrideCursor = Cursors.Arrow; }));
            worker.RunWorkerAsync();
        }

        public void ExportToWantedList(string wantedListId)
        {
            Contract.Requires(!string.IsNullOrEmpty(wantedListId));

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate
                             {
                                 bool loggedIn = BrickClient.Authenticate();
                                 if (!loggedIn)
                                     return;

                                 XmlWantedListInventory xmlInventory = new XmlWantedListInventory(inventory, wantedListId);

                                 string xmlFile = string.Concat("xmlFile=",
                                                                HttpUtility.UrlEncode(XmlManager.Serialize(xmlInventory),
                                                                                      Encoding.UTF8));

                                 byte[] data = Encoding.UTF8.GetBytes(xmlFile);

                                 HttpWebResponse response = BrickClient.PerformRequest(BrickClient.Page.UploadToWantedListVerify,
                                                                                       null, data);
                                 HtmlDocument htmlDocument = new HtmlDocument();
                                 StreamReader sr = new StreamReader(response.GetResponseStream());
                                 string responseHtml = sr.ReadToEnd();
                                 htmlDocument.LoadHtml(responseHtml);
                                 HtmlNode root = htmlDocument.DocumentNode;
                                 HtmlNode node = (from HtmlNode n in root.SelectNodes("//input")
                                                  where n.Attributes["type"].Value == "HIDDEN"
                                                  select n).FirstOrDefault();

                                 if (node != null)
                                     LogEvent.Network.Log("Wanted List Upload: Verify ok");
                                 else
                                 {
                                     LogEvent.Network.Log("Wanted List Upload: Verify failed.");
                                     return;
                                 }

                                 string xmlId = string.Format("{0}={1}", node.Attributes["name"].Value,
                                                              node.Attributes["value"].Value);

                                 HttpWebResponse finalResponse =
                                     BrickClient.PerformRequest(BrickClient.Page.UploadToWantedListFinal, null,
                                                                Encoding.UTF8.GetBytes(xmlId));
                                 if (finalResponse != null)
                                     LogEvent.Network.Log("Wanted List succesfully uploaded.");

                                 response.Close();
                                 finalResponse.Close();
                             };
            Mouse.OverrideCursor = Cursors.Wait;
            worker.RunWorkerCompleted += (sender, e) => Dispatcher.BeginInvoke(new Action(delegate
                                                                                          { Mouse.OverrideCursor = Cursors.Arrow;
                                                                                          }));
            worker.RunWorkerAsync();
        }

        internal void SetPrice(PriceInfo priceInfo)
        {
            List<ItemViewModel> items = SelectedItems.ToList();
            if (items.Count > 0)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += delegate
                                 {
                                     bool loggedIn = BrickClient.Authenticate();
                                     if (!loggedIn)
                                         return;

                                     foreach (ItemViewModel item in SelectedItems)
                                     {
                                         item.Price = Inventory.GetPriceGuideInfo(item.ItemNr, item.ColorId, item.Condition,
                                                                                  priceInfo.PriceInfoType);
                                     }
                                 };
                Mouse.OverrideCursor = Cursors.Wait;
                worker.RunWorkerCompleted += (sender, e) => Dispatcher.BeginInvoke(new Action(delegate
                                                                                              {
                                                                                                  Mouse.OverrideCursor =
                                                                                                      Cursors.Arrow;
                                                                                              }));
                worker.RunWorkerAsync();
            }
        }

        public ICommand SetPriceCommand
        {
            get { return new DelegateCommand<PriceInfo>(SetPrice, delegate { return (SelectedItems != null && SelectedItems.Count() > 0); }); }
        }

        public ICommand SelectionChangedCommand
        {
            get
            {
                return new DelegateCommand<IList>(
                delegate(IList items)
                {

                    SelectedItems = items.Cast<ItemViewModel>();
                }, null); }
        }

    }
}
