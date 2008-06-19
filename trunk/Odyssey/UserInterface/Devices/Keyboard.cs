using System.Windows.Forms;

namespace AvengersUtd.Odyssey.UserInterface.Devices
{
    public class Keyboard
    {
        bool[] keystate;
        static Keyboard singletonKeyboard;

        Keyboard()
        {
            keystate = new bool[256];
        }

        public bool this[Keys key] {

            get { return keystate[(int)key]; }
        }


        public static Keyboard Instance
        {
            get
            {
                if (singletonKeyboard == null)
                    singletonKeyboard = new Keyboard();

                return singletonKeyboard;
            }
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            keystate[e.KeyValue] = true;
           
        }


        public void KeyUp(object sender, KeyEventArgs e)
        {
            keystate[e.KeyValue] = false;
        }
    }
}