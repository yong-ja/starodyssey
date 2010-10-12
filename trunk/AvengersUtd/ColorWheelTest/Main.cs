using System;
using System.Windows.Forms;

namespace AvengersUtd.ColorChooserTest
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorChooser colorChooser = new ColorChooser {Color = panel1.BackColor};

            colorChooser.ShowDialog();
            panel1.BackColor = colorChooser.Color;
        }
    }
}
