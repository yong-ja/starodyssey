using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AvengersUtd.Odysseus.UIControls
{
    public partial class GradientEditor : Form
    {
        private SortedDictionary<string, Gradient> gradientDictionary;

        public GradientEditor()
        {
            InitializeComponent();
            gradientDictionary = new SortedDictionary<string, Gradient>();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            InputBox inputBox = new InputBox() {Text = "Create new gradient", DialogTitle = "New gradient name:"};
            inputBox.ShowDialog();
            
            if (inputBox.DialogResult != System.Windows.Forms.DialogResult.OK) return;

            Gradient gradient = new Gradient(inputBox.InputValue, gradientBuilder.CurrentMarkers);
            listBox.Items.Add(gradient.Name);
            gradientDictionary.Add(inputBox.InputValue, gradient);
        }

        public void SelectGradient(Gradient gradient)
        {
            gradientBuilder.SetMarkers(gradient.Markers);
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectGradient(gradientDictionary[(string)listBox.SelectedItem]);
        }
    }
}
