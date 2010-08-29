using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Graphics;

namespace AvengersUtd.Odyssey.UserInterface.Input
{
    public delegate void CameraTransformState(CameraAction action, bool state);

    public class CameraBinding
    {
        readonly CameraTransformState transformState;

        public CameraAction Action { get; private set; }
        public Keys Key { get; private set; }
        public float Amount { get; private set; }

        public void Apply(bool state)
        {
            transformState(Action, state);
        }

        public CameraBinding(CameraAction action, CameraTransformState transformState, Keys key, float amount)
        {
            Action = action;
            Key = key;
            Amount = amount;
            this.transformState = transformState;
        }

    }
}
