#region #Disclaimer

// Author: Adalberto L. Simeone (Taranto, Italy)
// E-Mail: avengerdragon@gmail.com
// Website: http://www.avengersutd.com/blog
// 
// This source code is Intellectual property of the Author
// and is released under the Creative Commons Attribution 
// NonCommercial License, available at:
// http://creativecommons.org/licenses/by-nc/3.0/ 
// You can alter and use this source code as you wish, 
// provided that you do not use the results in commercial
// projects, without the express and written consent of
// the Author.

#endregion

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Style;

#endregion

namespace AvengersUtd.Odysseus.UIControls
{
    public partial class GradientBuilder : UserControl
    {
        private string previousHexColor;

        public GradientBuilder()
        {
            InitializeComponent();
            Marker startMarker = new Marker(Color.Red, 0.0f);
            Marker endMarker = new Marker(Color.Green, 1.0f);
            gradientContainer.Markers = new List<Marker> {startMarker, endMarker};
            gradientContainer.SelectedMarkerChanged += GradientContainerSelectedMarkerChanged;
            gradientContainer.OffsetChanging += GradientContainerOffsetChanging;
            gradientContainer.SelectedMarker = startMarker;
            gradientContainer.OffsetChanged += GradientContainerOffsetChanged;
            gradientContainer.MarkersChanged += GradientContainerMarkersChanged;

            // Tooltip data
            toolTip.SetToolTip(cmdDel, "Delete selected Marker");
            toolTip.SetToolTip(cmdColorWheel, "Open color wheel");
            toolTip.SetToolTip(tbHexColor, "Color ARGB in hexadecimal value");
            toolTip.SetToolTip(ctlOffset, "Selected Marker offset");
        }


        #region Properties
        public Marker[] CurrentMarkers
        {
            get { return gradientContainer.Markers.ToArray(); }
        }

        public ColorBlend CurrentColorBlend
        {
            get { return ConvertToColorBlend(gradientContainer.Markers); }
        }

        public GradientStop[] GradientStops
        {
            get { return ConvertToGradientStop(CurrentMarkers); }
        } 
        #endregion

        #region Events
        public event MarkerEventHandler SelectedMarkerOffsetChanged;
        public event MarkerEventHandler SelectedMarkerColorChanged;
        public event EventHandler MarkersChanged;

        protected void OnMarkersChanged(EventArgs e)
        {
            EventHandler handler = MarkersChanged;
            if (handler != null) handler(this, e);
        }

        protected void OnSelectedMarkerColorChanged(MarkerEventArgs e)
        {
            MarkerEventHandler handler = SelectedMarkerColorChanged;
            if (handler != null) handler(this, e);
        }

        protected void OnSelectedMarkerOffsetChanged(MarkerEventArgs e)
        {
            MarkerEventHandler handler = SelectedMarkerOffsetChanged;
            if (handler != null) handler(this, e);
        } 
        #endregion

        #region Child controls events

        private void GradientContainerOffsetChanged(object sender, MarkerEventArgs e)
        {
            OnSelectedMarkerOffsetChanged(e);
        }
        void GradientContainerMarkersChanged(object sender, EventArgs e)
        {
            OnMarkersChanged(e);
        }

        #endregion

        private void TbHexColorEnter(object sender, EventArgs e)
        {
            int value;
            if (Int32.TryParse(tbHexColor.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
                previousHexColor = tbHexColor.Text;

            ctlOffset.Select(0, ctlOffset.Text.Length);
        }

        private void TbHexColorValidating(object sender, CancelEventArgs e)
        {
            int value;
            if (!Int32.TryParse(tbHexColor.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                tbHexColor.Text = previousHexColor;
                return;
            }
            else
            {
                gradientContainer.SelectedMarker.Color = Color.FromArgb(value);
                OnSelectedMarkerColorChanged(new MarkerEventArgs(gradientContainer.SelectedMarker));
            }

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

        public void SetMarkers(GradientStop[] gradient)
        {
            SetMarkers(ConvertToMarkers(gradient));
        }

        #region Conversion methods
        public static ColorBlend ConvertToColorBlend(IEnumerable<Marker> markers)
        {
            ColorBlend colorBlend = new ColorBlend(markers.Count())
                                        {
                                            Colors = markers.Select(m => m.Color).ToArray(),
                                            Positions = markers.Select(m => m.Offset).ToArray()
                                        };
            return colorBlend;
        }

        public static GradientStop[] ConvertToGradientStop(Marker[] markers)
        {
            GradientStop[] gradient = new GradientStop[markers.Length];

            for (int i = 0; i < markers.Length; i++)
            {
                Marker marker = markers[i];
                gradient[i] = new GradientStop(marker.Color, marker.Offset);
            }
            return gradient;
        }

        public static Marker[] ConvertToMarkers(GradientStop[] gradient)
        {
            Marker[] markers = new Marker[gradient.Length];

            for (int i = 0; i < gradient.Length; i++)
            {
                GradientStop gradientStop = gradient[i];
                markers[i] = new Marker(gradientStop.Color.ToColor(), gradientStop.Offset);
            }
            return markers;
        } 
        #endregion

        #region Events

        private void GradientContainerOffsetChanging(object sender, MarkerEventArgs e)
        {
            RefreshLabels(e.Marker.Offset);
        }

        private void GradientContainerSelectedMarkerChanged(object sender, MarkerEventArgs e)
        {
            tbHexColor.Text = e.Marker.Color.ToArgb().ToString("X8");
            previousHexColor = tbHexColor.Text;
            RefreshLabels(e.Marker.Offset);
            if (e.Marker.Offset == 0f || e.Marker.Offset == 1f)
            {
                cmdDel.Enabled = false;
                return;
            }
            cmdDel.Enabled = true;
        }

        private void CtlOffsetValueChanged(object sender, EventArgs e)
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

        private void BtColorWheelClick(object sender, EventArgs e)
        {
            Color previousColor = gradientContainer.SelectedMarker.Color;
            // Create a new instance of the ColorDialog.
            ColorChooser cc = new ColorChooser
                                  {
                                      Color = Color.FromArgb(Int32.Parse(tbHexColor.Text, NumberStyles.HexNumber))
                                  };

            // Show the dialog.
            cc.ShowDialog();

            // Return the newly selected color.
            if (cc.Color == previousColor) return;

            gradientContainer.SelectedMarker.Color = cc.Color;
            tbHexColor.Text = cc.Color.ToArgb().ToString("X8");
            gradientContainer.Invalidate();
            OnSelectedMarkerColorChanged(new MarkerEventArgs(gradientContainer.SelectedMarker));
        }


        private void CtlOffsetValidating(object sender, CancelEventArgs e)
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
            if (value == 0)
            {
                e.Cancel = true;
                ctlOffset.Value = 0.01m;
                ctlOffset.Select(0, ctlOffset.Text.Length);
            }
        }

        private void CmdDelClick(object sender, EventArgs e)
        {
            gradientContainer.DeleteSelectedMarker();
        }

        #endregion




    }
}