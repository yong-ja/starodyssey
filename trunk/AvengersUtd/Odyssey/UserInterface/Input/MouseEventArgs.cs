using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Input
{
    public enum MouseButton
    {
        None = 0,
        Left =1 ,
        Right = 2
    }

    public class MouseEventArgs : EventArgs
    {
        public Vector2 Location { get; private set; }
        public MouseButton Button { get; private set; }
        public int Delta { get; private set; }

        public MouseEventArgs(Vector2 location, MouseButton button, int delta)
        {
            Location = location;
            Button = button;
            Delta = delta;
        }

        static MouseButton ConvertFrom(System.Windows.Forms.MouseButtons button)
        {
            switch (button)
            {
                case System.Windows.Forms.MouseButtons.None:
                    return MouseButton.None;
                default:
                case System.Windows.Forms.MouseButtons.Left:
                    return MouseButton.Left;
                case System.Windows.Forms.MouseButtons.Right:
                    return MouseButton.Right;
                    
            }
        }

        private static MouseButton ConvertFrom(System.Windows.Input.MouseButton button)
        {
            switch (button)
            {
                default:
                case System.Windows.Input.MouseButton.Left:
                    return MouseButton.Left;
                case System.Windows.Input.MouseButton.Right:
                    return MouseButton.Right;

            }
        }

        public static explicit operator MouseEventArgs(System.Windows.Forms.MouseEventArgs e)
        {
            return new MouseEventArgs(new Vector2(e.X, e.Y),
                ConvertFrom(e.Button),
                e.Delta);
        }

        public static explicit operator MouseEventArgs(System.Windows.Input.MouseButtonEventArgs e)
        {
            
            return new MouseEventArgs(Mouse.CursorLocationWpf,
                ConvertFrom(e.ChangedButton),
                0);
        }






    }
}
