using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows.Media;
using AvengersUtd.BrickLab.Model;

namespace AvengersUtd
{
    public class Item
    {
        public string ItemNr { get;  set; }
        public Uri ImageUri { get;  set; }
        public int Quantity { get;  set; }
        public float Price { get;  set; }
        public Condition Condition { get;  set;}
        public int ColorId { get; set; }
        public string Description { get;  set; }
        public ItemType ItemType { get; set; }
        public int WantedListId { get; set; }
    }
}
