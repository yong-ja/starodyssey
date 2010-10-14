using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AvengersUtd.Odysseus.UIControls;

namespace AvengersUtd.Odysseus
{
    public class Gradient
    {
        public string Name { get; set; }
        public Marker[] Markers { get; set; }


        public Gradient(string name)
        {
            Name = name;
            Markers = new Marker[] {new Marker(Color.White, 0f), new Marker(Color.Black, 1.0f)};
        }

        public Gradient(string name, Marker[] markers)
        {
            Name = name;
            Markers = markers;
        }
    }
}
