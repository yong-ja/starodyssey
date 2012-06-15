using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using System.Windows;

namespace AvengersUtd.Odyssey.UserInterface.Input
{
    public static class Mouse
    {
        public const MouseButton ClickButton = MouseButton.Left;

        public static Vector2 CursorLocationWpf
        {
            get
            {
                Point p = System.Windows.Input.Mouse.GetPosition(Global.Window);
                return new Vector2((float)p.X, (float)p.Y);
            }
        }
    }
}
