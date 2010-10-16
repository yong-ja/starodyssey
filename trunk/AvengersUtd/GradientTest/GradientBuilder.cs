#region Disclaimer
// /* 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com/blog
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */
#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace AvengersUtd.GradientTest
{
    public partial class GradientBuilder : UserControl
    {
        public GradientBuilder()
        {
            InitializeComponent();
            Marker startMarker = new Marker(Color.Red, 0.0f);
            Marker endMarker = new Marker(Color.Green, 1.0f);
            gradientContainer.Markers = new List<Marker> {startMarker, endMarker};
            gradientContainer.SelectedMarkerChanged += gradientContainer_SelectedMarkerChanged;
            gradientContainer.OffsetChanged += gradientContainer_OffsetChanged;
            gradientContainer.SelectedMarker = startMarker;

            // Tooltip data
            toolTip.SetToolTip(cmdDel, "Delete selected marker");
            toolTip.SetToolTip(cmdColorWheel, "Open color wheel");
            toolTip.SetToolTip(tbHexColor, "Color ARGB in hexadecimal value");
            toolTip.SetToolTip(ctlOffset,"Selected marker offset");
        }

        public Marker[] CurrentMarkers
        {
            get { return gradientContainer.Markers.ToArray(); }
        }
        
        public ColorBlend CurrentColorBlend
        {
            get { return ConvertToColorBlend(gradientContainer.Markers); }
        }

        public void RefreshLabels(float value)
        {
            ctlOffset.Value = (decimal) value;
        }

        public void SetMarkers(Marker[] markers)
        {
            gradientContainer.Markers = markers.ToList();
            gradientContainer.SelectedMarker = gradientContainer.Markers[0];
            gradientContainer.Invalidate();
        }

        #region Events

        private void gradientContainer_OffsetChanged(object sender, MarkerEventArgs e)
        {
            RefreshLabels(e.Marker.Offset);
        }

        private void gradientContainer_SelectedMarkerChanged(object sender, MarkerEventArgs e)
        {
            tbHexColor.Text = e.Marker.Color.ToArgb().ToString("X8");
            RefreshLabels(e.Marker.Offset);
            if (e.Marker.Offset == 0f || e.Marker.Offset == 1f)
            {
                cmdDel.Enabled = false;
                return;
            }
            cmdDel.Enabled = true;

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
                gradientContainer.SelectedMarker.Offset = (float) ctlOffset.Value;
                gradientContainer.SortMarkers();
                gradientContainer.Invalidate();
            }
        }

        private void btColorWheel_Click(object sender, EventArgs e)
        {
            // Create a new instance of the ColorDialog.
            ColorChooser cc = new ColorChooser
                         {
                             Color = Color.FromArgb(Int32.Parse(tbHexColor.Text, NumberStyles.HexNumber))
                         };

            // Show the dialog.
            cc.ShowDialog();

            // Return the newly selected color.
            gradientContainer.SelectedMarker.Color = cc.Color;
            tbHexColor.Text = cc.Color.ToArgb().ToString("X8");

            gradientContainer.Invalidate();
        }

        private void ctlOffset_Validating(object sender, CancelEventArgs e)
        {
            decimal value;
            if (!decimal.TryParse(ctlOffset.Text, out value))
            {
                return;
            }
            if (value > 0.99m)
            {
                e.Cancel = true;
                //MessageBox.Show("Please insert a value between 0.01 and 0.99.", "Warning", MessageBoxButtons.OK,
                //    MessageBoxIcon.Exclamation);

                ctlOffset.Value = 0.99m;
                ctlOffset.Select(0, ctlOffset.Text.Length);
            }
        }

        private void cmdDel_Click(object sender, EventArgs e)
        {
            gradientContainer.DeleteSelectedMarker();
        }

        #endregion

        public static ColorBlend ConvertToColorBlend(IEnumerable<Marker> markers)
        {
            ColorBlend colorBlend = new ColorBlend(markers.Count())
            {
                Colors = markers.Select(m => m.Color).ToArray(),
                Positions = markers.Select(m => m.Offset).ToArray()
            };
            return colorBlend;
        }
    }
}