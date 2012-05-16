using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Graphics;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Input
{
    public delegate void KeyOperation();

    public class KeyBinding
    {

        public KeyAction Action { get; private set; }
        public Keys Key { get; private set; }
        public bool State { get; private set; }
        public KeyOperation Operation { get; private set; }

        public void Apply(bool state)
        {
                State = state;
        }

        public KeyBinding(KeyAction action, Keys key, KeyOperation operation)
        {
            Action = action;
            Key = key;
            Operation = operation;
        }

        void Execute()
        {
            Operation();
        }

    }
}
