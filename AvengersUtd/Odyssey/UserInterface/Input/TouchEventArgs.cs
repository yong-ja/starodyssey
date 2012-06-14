using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Input
{
    public class TouchEventArgs : EventArgs
    {
        Vector2 Location;

        public TouchEventArgs(Vector2 location)
        {
            Location = location;
        }
    }
}
