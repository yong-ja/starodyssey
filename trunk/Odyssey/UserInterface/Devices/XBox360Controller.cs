using System;
using System.Collections.Generic;
using System.Text;
using SlimDX.XInput;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using SlimDX;
using System.ComponentModel;

namespace AvengersUtd.Odyssey.UserInterface.Devices
{
    public class XBox360Controller : AvengersUtd.Odyssey.UserInterface.Devices.IXBox360Controller
    {
        readonly Controller controller;
        readonly Timer timer;
        Vector2 vLeftThumbstick;
        Vector2 vRightThumbstick;
        GamepadButtonFlags activeButtons;

        EventHandlerList eventHandlerList;

        #region Events
        static object EventButtonADown;
        static object EventButtonAPressed;
        static object EventButtonAUp;

        public event EventHandler ButtonADown
        {
            add { eventHandlerList.AddHandler(EventButtonADown, value); }
            remove { eventHandlerList.RemoveHandler(EventButtonADown, value); }
        }

        public event EventHandler ButtonAPressed
        {
            add { eventHandlerList.AddHandler(EventButtonAPressed, value); }
            remove { eventHandlerList.RemoveHandler(EventButtonAPressed, value); }
        }

        public event EventHandler ButtonAUp
        {
            add { eventHandlerList.AddHandler(EventButtonAUp, value); }
            remove { eventHandlerList.RemoveHandler(EventButtonAUp, value); }
        }

        protected virtual void OnButtonADown(EventArgs e)
        {
            EventHandler handler = (EventHandler)eventHandlerList[EventButtonADown];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnButtonAPressed(EventArgs e)
        {
            EventHandler handler = (EventHandler)eventHandlerList[EventButtonAPressed];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnButtonAUp(EventArgs e)
        {
            EventHandler handler = (EventHandler)eventHandlerList[EventButtonAUp];
            if (handler != null)
                handler(this, e);
        } 
        #endregion


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

        public bool IsLeftThumbstickPressed
        {
            get { return (activeButtons & GamepadButtonFlags.LeftThumb) == 0; }
        }

        public bool IsRightThumbstickPressed
        {
            get { return (activeButtons & GamepadButtonFlags.RightThumb) == 0; }
        }

        public bool IsButtonAPressed
        {
            get { return (activeButtons & GamepadButtonFlags.A) == GamepadButtonFlags.A; }
        }

        static readonly XBox360Controller[] controllers = new XBox360Controller[4];

        static XBox360Controller()
        {
            EventButtonADown = new object();
            EventButtonAPressed = new object();
            EventButtonAUp = new object();
        }

        XBox360Controller(int index)
        {
            eventHandlerList = new EventHandlerList();
            controller = new Controller((UserIndex)index);
            timer = new Timer();
            timer.Interval = 20;
            timer.Elapsed += Poll;
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

            CheckButtons(gamepad.Buttons);
            activeButtons = gamepad.Buttons;

            vLeftThumbstick = new Vector2(leftX, leftY);
            vRightThumbstick = new Vector2(rightX, rightY);


            
        }

        void CheckButtons(GamepadButtonFlags buttons)
        {
            if (!IsButtonAPressed && 
                ((buttons & GamepadButtonFlags.A) == GamepadButtonFlags.A))
                OnButtonADown(EventArgs.Empty);

            if (IsButtonAPressed && 
                ((buttons & GamepadButtonFlags.A) == 0))
            {
                OnButtonAPressed(EventArgs.Empty);
                OnButtonAUp(EventArgs.Empty);
            }
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
