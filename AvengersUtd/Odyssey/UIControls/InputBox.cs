using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AvengersUtd.Odysseus.UIControls
{
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        public string DialogTitle
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        public string InputValue
        {
            get { return textBox.Text; }
        }
    }
}
