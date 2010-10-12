using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

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

    public class MarkerEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the currently selected <see cref="Marker"/>.
        /// </summary>
        public Marker SelectedMarker { get; private set; }

        public MarkerEventArgs(Marker selectedMarker)
        {
            SelectedMarker = selectedMarker;
        }
        
    }
}
