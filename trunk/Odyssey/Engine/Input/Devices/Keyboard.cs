using System.Windows.Forms;

namespace AvengersUtd.Odyssey.Engine.Input.Devices
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
            //if (Game.Input.IsInputEnabled)
            //    OdysseyUI.ProcessKeyDown(sender, e);

            if (Game.Input.IsCameraEnabled)
                Game.CurrentScene.Camera.States.ProcessEvent(keystate);
        }

        //public void KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (Game.Input.IsInputEnabled)
        //        OdysseyUI.ProcessKeyPress(sender, e);
        //}

        public void KeyUp(object sender, KeyEventArgs e)
        {
            keystate[e.KeyValue] = false;

            if (Game.Input.IsCameraEnabled)
                Game.CurrentScene.Camera.States.ProcessEvent(keystate);
        }
    }
}