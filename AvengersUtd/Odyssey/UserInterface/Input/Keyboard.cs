using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace AvengersUtd.Odyssey.UserInterface.Input
{
    public class Keyboard
    {
        readonly bool[] keystate;

        public Keyboard()
        {
            keystate = new bool[256];
        }

        public bool this[Key key]
        {
            get
            {
                return keystate[(int)key];
            }
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            keystate[(int)e.Key] = true;
        }


        public void KeyUp(object sender, KeyEventArgs e)
        {
            keystate[(int)e.Key] = false;
        }


    }
}
