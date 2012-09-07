using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvengersUtd.BrickLab.Model.XmlWrappers
{
    public class XmlWantedListItem
    {
        private ItemType itemType;
        [XmlIgnore]
        public ItemType ItemType
        {
            get { return itemType; }
            set {
                itemType = value;
                StringItemType = Converter.ItemTypeFromEnum(itemType);
            }
        }

        [XmlElement("ITEMTYPE")]
        public string StringItemType { get; set; }

        [XmlElement("ITEMID")]
        public string ItemNr { get; set; }
        [XmlElement("COLOR")]
        public int ColorId { get; set; }

        private Condition condition;
        [XmlIgnore]
        public Condition Condition
        {
            get { return condition; }
            set { condition = value;
                StringCondition = Converter.ConditionFromEnum(condition);
            }
        }

        [XmlElement("CONDITION")]        
        public string StringCondition { get; set; }
        [XmlElement("MINQTY")]
        public int Quantity { get; set; }
        //public float Price { get; set; }
        [XmlElement("NOTIFY")]
        public string Notify { get; set; }
        
        [XmlElement("WANTEDLISTID")]
        public string WantedListId { get; set; }

        public XmlWantedListItem()
        {
            Notify = "N";
        }

        public XmlWantedListItem(Item item) :this()
        {
            ItemType = item.ItemType;
            Condition = item.Condition;
            ItemNr = item.ItemNr;
            ColorId = item.ColorId;
            Quantity = item.Quantity;
        }

        
    }
}
