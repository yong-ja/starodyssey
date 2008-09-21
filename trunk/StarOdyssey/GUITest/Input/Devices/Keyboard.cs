using System.Windows.Forms;

namespace AvengersUtd.Odyssey.Input.Devices
{
    public class Keyboard
    {
        bool[] keystate;
        static Keyboard singletonKeyboard;

        Keyboard()
        {
            keystate = new bool[256];
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