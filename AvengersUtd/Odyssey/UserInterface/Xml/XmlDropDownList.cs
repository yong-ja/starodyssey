using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Text;

namespace AvengersUtd.Odyssey.UserInterface.Xml
{
    public class XmlDropDownList : XmlBaseControl
    {
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        [Category("Appearance")]
        public string[] Items { get; set; }
        protected override void WriteCustomCSCode(StringBuilder sb)
        {
            string items = Items.Aggregate(string.Empty, (current, item) => current + (item + ','));

            sb.AppendFormat("\t\tItems = new []{{{0}}},", items);
        }

        public XmlDropDownList(DropDownList dropDownList)
            : base(dropDownList)
        {

            Items = (from TextLiteral label in dropDownList.Controls select label.Content).ToArray();
        }
    }
}
