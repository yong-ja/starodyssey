using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AvengersUtd.BrickLab.DataAccess;

namespace AvengersUtd.BrickLab.Model.XmlWrappers
{
        [XmlRoot("INVENTORY")]
    public class XmlWantedListInventory
    {
        private List<XmlWantedListItem> xmlInventory;
            private string wantedListId;

            [XmlIgnore]
        public string WantedListId
            {
                get { return wantedListId; }
                set { wantedListId = value.Trim(); }
            }


            [XmlElement("ITEM")]
        public List<XmlWantedListItem> XmlInventory
        {
            get { return xmlInventory; }
            set { 
                xmlInventory = value;
                foreach (XmlWantedListItem xmlItem in xmlInventory)
                    xmlItem.WantedListId = WantedListId;
            }
        }

        public XmlWantedListInventory()
        {
            XmlInventory = new List<XmlWantedListItem>();
        }

        public XmlWantedListInventory(Inventory inventory, string wantedListId) : this()
        {
            WantedListId = wantedListId;

            foreach (XmlWantedListItem xmlItem in 
                inventory.GetParts().Select(item => new XmlWantedListItem(item) { WantedListId = wantedListId }))
            {
                XmlInventory.Add(xmlItem);
            }
        }
    }
}
