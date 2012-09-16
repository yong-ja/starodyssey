using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using AvengersUtd.BrickLab.Logging;
using AvengersUtd.BrickLab.Model;
using HtmlAgilityPack;
using AvengersUtd.BrickLab.DataAccess.Csv;
using Condition = AvengersUtd.BrickLab.Model.Condition;

namespace AvengersUtd.BrickLab.DataAccess
{
    public class Inventory
    {
        private readonly List<Item> partList;
        private ImageCache imageCache;

        public Inventory()
        {
            partList = new List<Item>();
            imageCache = new ImageCache();
        }

        public List<Item> GetParts()
        {
            return new List<Item>(partList);
        }

        public void DownloadSetInventory(string setNumber)
        {
            string queryString = string.Format("itemType=S&viewType=4&itemTypeInv=S&itemNo={0}&downloadType=T", setNumber);
            byte[] data = Encoding.UTF8.GetBytes(queryString);
            LogEvent.Network.Log(string.Format("Downloading inventory for set [{0}]", setNumber));
            HttpWebResponse responseHtml = BrickClient.PerformRequest(BrickClient.Page.InventoryDownload,
                new[] {"?a=a"}, data);
            if (responseHtml == null || responseHtml.StatusCode != HttpStatusCode.OK)
                return;

            LogEvent.System.Log("Parsing inventory...");
            partList.AddRange(ParseTabInventory(responseHtml.GetResponseStream()));

            LogEvent.Network.Log(string.Format("Downloading image list for set [{0}]", setNumber));
            Dictionary<string, Uri> imageList = DownloadImageList(setNumber);
            foreach (KeyValuePair<string, Uri> kvp in imageList)
            {
                string itemCode = kvp.Key;
                string[] args = itemCode.Split('.');
                string itemNr = args[0];
                int colorCode = Int32.Parse(args[1]);

                foreach (Item item in partList.FindAll(item => string.Equals(itemNr, item.ItemNr) && item.ColorId==colorCode))
                    item.ImageUri = kvp.Value;
            }
            //var images = from p in partList select p.ImageUri;
            //imageCache.DownloadImages(images);
            //LogEvent.Network.Log(tabInventory);
        }

        static IEnumerable<Item> ParseTabInventory(Stream stream)
        {
            List<Item> partList = new List<Item>();
            using (CsvReader csv = new CsvReader(new StreamReader(stream), true, '\t'))
            {
                while (csv.ReadNextRecord())
                {
                    string itemNr = csv["Item No"];
                    string itemDesc = csv["Item Name"];
                    string itemQty = csv["Qty"];
                    string colorId = csv["Color Id"];
                    string colorCode = csv["Color ID"];
                    ItemType itemType;
                    if (csv["Type"] == "P") itemType = ItemType.Part;
                    else itemType = csv["Type"] == "M" ? ItemType.MiniFigure : ItemType.Unknown;

                    Uri imageUri = new Uri(string.Format("http://img.bricklink.com/{0}/{1}.{2}",
                                                    itemType == ItemType.Part ? string.Format("P/{0}", colorCode) : "M",
                                                    itemNr, "jpg"));

                    partList.Add(new Item
                    {
                        ItemType = itemType,
                        ItemNr = itemNr,
                        Description = itemDesc,
                        Quantity = Int32.Parse(itemQty),
                        ColorId = Int32.Parse(colorId),
                        ImageUri = imageUri,
                        Condition = Condition.New
                    });
                }
            }

            return partList;
        }

        internal static float GetPriceGuideInfo(string itemId, int colorId, Condition condition, PriceInfoType priceInfo)
        {
            string[] args = new[] {itemId, colorId.ToString()};
            string responseHtml = BrickClient.NavigateTo(BrickClient.Page.PriceGuide, true, args);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseHtml);
            HtmlNode root = htmlDoc.DocumentNode;
            var rows = root.SelectNodes("/table[@class='fv'][1]/tr/*");
            List<HtmlNode> headerCells = rows.Skip(1).First().SelectNodes("td").ToList();
            int newIndex = headerCells.FindIndex(c => c.InnerText.Equals("New"));
            int oldIndex = headerCells.FindIndex(c => c.InnerText.Equals("Old"));
            HtmlNode priceRow = rows.Skip(2).First();
            var priceTableRows = priceRow.SelectNodes("/table/tr/*");
            var priceTableCell = condition == Condition.New ? priceTableRows[newIndex] : priceTableRows[oldIndex];
            var priceTableInnerRows = priceTableCell.SelectNodes("/table[@class='fv'/tr/*");

            string searchString;
            switch (priceInfo)
            {
                case PriceInfoType.Min:
                    searchString = "Min";
                    break;
                case PriceInfoType.Max:
                    searchString = "Max";
                    break;

                default:
                case PriceInfoType.Average:
                    searchString = "Avg";
                    break;

                case PriceInfoType.QuantityAverage:
                    searchString = "Qty Avg";
                    break;
            }

            foreach (HtmlNode row in priceTableInnerRows)
            {
                if (!row.ChildNodes[0].InnerText.StartsWith(searchString)) continue;

                string price = row.ChildNodes[1].InnerText.Split(' ')[1];
                return float.Parse(price);
            }
            
            return float.NaN;
        }


        public Dictionary<string, Uri> DownloadImageList(string setNumber)
        {
            LogEvent.Network.Write("pretry");
            try
            {
                LogEvent.Network.Write("In dw Image List");
                HtmlWeb htmlWeb = new HtmlWeb();
                string downloadUrl = String.Format("http://www.bricklink.com/catalogItemInv.asp?S={0}", setNumber);

                // Creates an HtmlDocument object from an URL
                LogEvent.Network.Write("creating web document");
                HtmlDocument document = htmlWeb.Load(downloadUrl);
                HtmlNode root = document.DocumentNode;
                            // Targets a specific node
                LogEvent.Network.Write("looking for node");
                var tableNodes = from table in root.SelectNodes("//table[@class='ta']") select table;
                HtmlNode[] nodes = tableNodes.ToArray();
                LogEvent.Network.Log(string.Format("Inventory for set [{0}] downloaded.", setNumber));
                return ParseTable(nodes[0]);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException + "\n\n" +
                ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                LogEvent.System.Log(string.Format("{0}\n{1}\nStackTrace:\n{2}", ex.GetType(),
                    ex.Message, ex.StackTrace));
                return null;
            }


            

        }

        private static Dictionary<string, Uri> ParseTable(HtmlNode table)
        {
            var rows = table.SelectNodes("tr");

            List<HtmlNode> headerCells = rows.First().SelectNodes("td").ToList();

            int indexImage = headerCells.FindIndex(c => HtmlEntity.DeEntitize(c.InnerText).Trim().Equals("Image"));
            //int indexQty = headerCells.FindIndex(c => c.InnerText.Equals("Qty"));
            int indexItemNr = headerCells.FindIndex(c => HtmlEntity.DeEntitize(c.InnerText).Trim().StartsWith("Item"));
            //int indexDescription = headerCells.FindIndex(c => c.InnerText.EndsWith("Description"));
            Dictionary<string, Uri> imageList = new Dictionary<string, Uri>();
            foreach (HtmlNode row in rows.Skip(2))
            {
                var cells = from cell in row.SelectNodes("td") select cell;
                HtmlNode[] cellArray = cells.ToArray();
                if (cellArray.Length < 3)
                    continue;
                string imageUrl = cellArray[indexImage].Descendants("img").First().Attributes["src"].Value;
                //int quantity = Int32.Parse(HtmlEntity.DeEntitize(cellArray[indexQty].FirstChild.InnerText).Trim());
                string itemNr = HtmlEntity.DeEntitize(cellArray[indexItemNr].InnerText).Trim();
                if (itemNr.Contains("Inv"))
                    itemNr = itemNr.Split(' ')[0];
                //string description = HtmlEntity.DeEntitize(cellArray[indexDescription].InnerText);
                //Item item = new Item
                //            {
                //                ImageUri = new Uri(imageUrl),
                //                ItemNr = itemNr,
                //                Quantity = quantity,
                //                Description = String.Empty
                //            };
                //parts.Add(item);

                Regex regex = new Regex(@"/P/(?'colorCode'\d+)/");
                Match match = regex.Match(imageUrl);
                string colorCode = string.Empty;
                if (match.Success)
                    colorCode = match.Groups["colorCode"].Value;

                string itemCode = string.Format("{0}.{1}", itemNr, string.IsNullOrEmpty(colorCode) ? "0" : colorCode);
                if (!imageList.ContainsKey(itemCode))
                    imageList.Add(itemCode, new Uri(imageUrl));
            }
            return imageList;
            //return parts.ToArray();
        }

        
        
    }
}
