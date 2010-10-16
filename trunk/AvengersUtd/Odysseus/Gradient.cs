using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AvengersUtd.Odysseus.UIControls;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odysseus
{
    public class Gradient
    {
        public string Name { get; set; }
        public Marker[] Markers { get; set; }

        public Gradient(string name)
        {
            Name = name;
            Markers = new[] {new Marker(Color.White, 0f), new Marker(Color.Black, 1.0f)};
        }

        public Gradient(string name, Marker[] Markers)
        {
            Name = name;
            Markers = Markers;
        }
    }
}
