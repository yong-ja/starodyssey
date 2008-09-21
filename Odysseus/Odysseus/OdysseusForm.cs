using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey;

namespace AvengersUtd.Odysseus
{
    public partial class OdysseusForm : Form
    {
        public OdysseusForm()
        {
            InitializeComponent();
            Game.CurrentScene = new UIRenderer();

        }
    }
}
