using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Media;
using AvengersUtd.BrickLab.Logging;
using AvengersUtd.BrickLab.Model;
using HtmlAgilityPack;
using AvengersUtd.BrickLab.DataAccess.Csv;

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
            HttpWebResponse responseHtml = BrickClient.PerformRequest(BrickClient.Page.InventoryDownload, "?a=a", data);
            if (responseHtml == null || responseHtml.StatusCode != HttpStatusCode.OK)
                return;

            partList.AddRange(ParseTabInventory(responseHtml.GetResponseStream()));

            Dictionary<string, Uri> imageList = DownloadImageList(setNumber);
            foreach (KeyValuePair<string, Uri> kvp in imageList)
            {
                string itemNr = kvp.Key;
                foreach (Item item in partList.FindAll(item => string.Equals(itemNr, item.ItemNr)))
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


        public Dictionary<string, Uri> DownloadImageList(string setNumber)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            string downloadUrl = String.Format("http://www.bricklink.com/catalogItemInv.asp?S={0}", setNumber);

            // Creates an HtmlDocument object from an URL
            HtmlDocument document = htmlWeb.Load(downloadUrl);
            HtmlNode root = document.DocumentNode;

            // Targets a specific node
            var tableNodes = from table in root.SelectNodes("//table[@class='ta']") select table;
            HtmlNode[] nodes = tableNodes.ToArray();
            return ParseTable(nodes[0]);
        }

        private static Dictionary<string, Uri> ParseTable(HtmlNode table)
        {
            var rows = table.SelectNodes("tr");
            List<Item> parts = new List<Item>();

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
                if (!imageList.ContainsKey(itemNr))
                    imageList.Add(itemNr, new Uri(imageUrl));
            }
            return imageList;
            //return parts.ToArray();
        }

        
        
    }
}
