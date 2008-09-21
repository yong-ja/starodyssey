using System;
using System.Collections.Generic;
using System.Text;
using SlimDX.XInput;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Devices
{
    public class XBox360Controller : AvengersUtd.Odyssey.UserInterface.Devices.IXBox360Controller
    {
        Controller controller;
        Timer timer;
        Vector2 vLeftThumbstick;
        Vector2 vRightThumbstick;
        GamepadButtonFlags activeButtons;

        public Vector2 LeftThumbstick
        {
            get { return vLeftThumbstick; }
        }
        public Vector2 RightThumbstick
        {
            get { return vRightThumbstick; }
        }
        public GamepadButtonFlags ActiveButtons
        {
            get { return activeButtons; }
        }


        static XBox360Controller[] controllers = new XBox360Controller[4];

        XBox360Controller(int index)
        {
            controller = new Controller((UserIndex)index);
            timer = new Timer();
            timer.Interval = 20;
            timer.Elapsed += new Timer.ElapsedEventHandler(Poll);
            timer.Start();
        }

        void Poll(object sender, ElapsedEventArgs e)
        {
            Gamepad gamepad;
            if (!controller.IsConnected)
                return;
            else
                gamepad = controller.GetState().Gamepad;

            float leftX = (Math.Abs((int)gamepad.LeftThumbX) > Gamepad.GamepadLeftThumbDeadZone) ? gamepad.LeftThumbX/32768f : 0;
            float leftY = (Math.Abs((int)gamepad.LeftThumbY) > Gamepad.GamepadLeftThumbDeadZone) ? gamepad.LeftThumbY / 32768f : 0;
            float rightX = (Math.Abs((int)gamepad.RightThumbX) > Gamepad.GamepadLeftThumbDeadZone) ? gamepad.RightThumbX / 32768f : 0;
            float rightY = (Math.Abs((int)gamepad.RightThumbY) > Gamepad.GamepadLeftThumbDeadZone) ? gamepad.RightThumbY / 32768f : 0;
            activeButtons = gamepad.Buttons;

            vLeftThumbstick = new Vector2(leftX, leftY);
            vRightThumbstick = new Vector2(rightX, rightY);
            
        }

        public Gamepad GamepadState
        {
            get
            {
                State state;
                try
                {
                    state = controller.GetState();
                }
                catch (SlimDX.XInput.XInputException ex)
                {
                    return new Gamepad();
                }
                return state.Gamepad;
            }
        }

        public static XBox360Controller GetController(int index)
        {
            if (controllers[index] == null)
                controllers[index] = new XBox360Controller(index);

            return controllers[index];
        }



    }
}
