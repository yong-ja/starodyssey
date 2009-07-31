using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odysseus
{
    public partial class CStyleForm : Form
    {
        public CStyleForm()
        {
            InitializeComponent();
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = new OControlStyle(StyleManager.GetControlStyle((string)listBox.SelectedItem));
        }

        private void CStyleForm_Load(object sender, EventArgs e)
        {
            foreach (ControlStyle cStyle in StyleManager.ControlStyles)
                listBox.Items.Add(cStyle.Name);
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            PropertyInfo pInfo = typeof(ColorArray).GetProperty(e.ChangedItem.PropertyDescriptor.Name,
                BindingFlags.Public | BindingFlags.Instance);
            pInfo.SetValue(StyleManager.GetControlStyle((string) listBox.SelectedItem).ColorArray,
                Color.FromArgb(((OColor)e.ChangedItem.Value).GetARGB()),null);
        }
    }
}
