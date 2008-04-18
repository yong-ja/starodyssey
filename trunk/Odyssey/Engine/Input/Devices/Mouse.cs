using System.Windows.Forms;

namespace AvengersUtd.Odyssey.Input.Devices
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

        //public void MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (Game.Input.IsInputEnabled)
        //        OdysseyUI.ProcessMouseInputPress(sender, e);
        //}

        //public void MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (Game.Input.IsInputEnabled)
        //        OdysseyUI.ProcessMouseInputRelease(sender, e);
        //}

        //public void MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (Game.Input.IsInputEnabled)
        //        OdysseyUI.MouseMovementHandler(sender, e);
        //}
    }
}