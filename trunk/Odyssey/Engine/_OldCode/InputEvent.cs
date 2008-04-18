using System;

namespace AvengersUtd.Odyssey.Input
{
    public class InputEvent : EventArgs
    {
        string actionID;
        bool pressed;

        public string ActionID
        {
            get { return actionID; }
        }

        public bool Pressed
        {
            get { return pressed; }
        }

        public InputEvent(string actionID, bool pressed)
        {
            this.actionID = actionID;
            this.pressed = pressed;
        }
    }
}