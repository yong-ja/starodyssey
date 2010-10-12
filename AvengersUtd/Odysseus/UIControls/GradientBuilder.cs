using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AvengersUtd.Odysseus.UIControls
{
    
    public partial class GradientBuilder : UserControl
    {

        public GradientBuilder()
        {
            InitializeComponent();
            Marker startMarker = new Marker(Color.Red, 0.0f);
            Marker endMarker = new Marker(Color.Green, 1.0f);
            gradientContainer.Markers = new List<Marker>() { startMarker ,endMarker };
            gradientContainer.SelectedMarkerChanged += gradientContainer_SelectedMarkerChanged;
        }

        void gradientContainer_SelectedMarkerChanged(object sender, MarkerEventArgs e)
        {
            tbHexColor.Text = e.SelectedMarker.Color.ToArgb().ToString("X8");
            ctlOffset.Value = (decimal) e.SelectedMarker.Offset;
        }

        private void ctlOffset_ValueChanged(object sender, EventArgs e)
        {
            if (ctlOffset.Value == 0 || ctlOffset.Value == 1.0m)
            {
                ctlOffset.Enabled = false;
            }
            else
            {
                ctlOffset.Enabled = true;
                gradientContainer.SelectedMarker.Offset = (float)ctlOffset.Value;
                gradientContainer.SortMarkers();
                gradientContainer.Invalidate();
            }

        }

        private void btColorWheel_Click(object sender, EventArgs e)
        {
            // Create a new instance of the ColorDialog.
            ColorChooser cc = new ColorChooser
                                  {
                                      Color = Color.FromArgb(Int32.Parse(tbHexColor.Text,
                                                                     NumberStyles.HexNumber))
                                  };

            // Show the dialog.
            cc.ShowDialog();

            // Return the newly selected color.

            gradientContainer.SelectedMarker.Color = cc.Color;

            tbHexColor.Text = cc.Color.ToArgb().ToString("X8");

        }

        

    }
}
