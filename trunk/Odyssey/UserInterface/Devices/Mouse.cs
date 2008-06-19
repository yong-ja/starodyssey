using System.Windows.Forms;

namespace AvengersUtd.Odyssey.UserInterface.Devices
{
    public class Mouse
    {
        bool[] mouseState;
        static Mouse singletonMouse;

        public static Mouse Instance
        {
            get
            {
                if (singletonMouse == null)
                    singletonMouse = new Mouse();

                return singletonMouse;
            }
        }

        Mouse()
        {
            // 6 is the total maximum number of buttons on a modern mouse
            mouseState = new bool[6];
        }

        public bool this[MouseButtons bt]
        {
            get { return mouseState[(int) bt]; }
        }

    }
}