using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Xml;

namespace AvengersUtd.Odysseus
{
    public partial class PropertyBox : Form
    {
        public XmlBaseControl SelectedControl
        {
            get { return (XmlBaseControl)propertyGrid1.SelectedObject; }
            set { propertyGrid1.SelectedObject = value; }
        }

        public PropertyBox()
        {
            InitializeComponent();
        }
    }
}
