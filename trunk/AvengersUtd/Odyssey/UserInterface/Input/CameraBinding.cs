using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Graphics;

namespace AvengersUtd.Odyssey.UserInterface.Input
{
    public delegate void TransformStateMethod(KeyAction action, bool state);

    public class KeyBinding
    {
        readonly TransformStateMethod transformState;

        public KeyAction Action { get; private set; }
        public Keys Key { get; private set; }
        public float Amount { get; private set; }
        public bool State { get; private set; }

        public void Apply(bool state)
        {
            if (transformState == null)
                State = state;
            else 
                transformState(Action, state);
        }

        public KeyBinding(KeyAction action, TransformStateMethod transformState, Keys key, float amount)
        {
            Action = action;
            Key = key;
            Amount = amount;
            this.transformState = transformState;
        }

        public KeyBinding(KeyAction action, Keys key, float amount)
        {
            Action = action;
            Key = key;
            Amount = amount;
        }

    }
}
