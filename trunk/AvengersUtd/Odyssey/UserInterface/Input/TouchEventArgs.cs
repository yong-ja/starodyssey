using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using System.Windows;
using AvengersUtd.Odyssey.Utils.Logging;
using System.Windows.Input;

namespace AvengersUtd.Odyssey.UserInterface.Input
{
    public class TouchEventArgs : EventArgs
    {
        public Vector2 Location { get; private set; }
        public TouchDevice TouchDevice { get; private set; }

        public TouchEventArgs(Vector2 location, TouchDevice touchDevice)
        {
            Location = location;
            TouchDevice = touchDevice;
        }

        public static explicit operator TouchEventArgs(System.Windows.Input.TouchEventArgs e)
        {
            Point p = e.GetTouchPoint(Global.Window).Position;
            Vector2 vP = new Vector2((float)p.X, (float)p.Y);
            TouchEventArgs eTouch = new TouchEventArgs(vP, e.TouchDevice);
            return eTouch;
        }
    }
}
