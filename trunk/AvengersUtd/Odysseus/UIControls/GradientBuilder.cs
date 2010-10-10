using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AvengersUtd.Odysseus.UIControls
{
    public class Marker
    {
        public Color Color { get; set; }
        public float Offset { get; set; }
        public bool Selected { get; set; }

        public Marker(Color color, float offset) 
        {
            Color = color;
            Offset = offset;
            Selected = false;
        }
    }
    public partial class GradientBuilder : UserControl
    {

        public GradientBuilder()
        {
            InitializeComponent();
            Marker startMarker = new Marker(Color.Red, 0.0f);
            Marker endMarker = new Marker(Color.Green, 1.0f);
            gradientContainer1.Markers = new SortedList<float, Marker>() { { startMarker.Offset, startMarker }, { endMarker.Offset, endMarker } };

        }

        

    }
}
