using System;
using System.Collections.Generic;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics
{
    public class PositionEventArgs : EventArgs
    {
        Vector3 previousValue;

        public Vector3 PreviousValue
        {
            get { return previousValue; }
        }

        public Vector3 NewValue { get; set; }

        public new static PositionEventArgs Empty
        {
            get { return new PositionEventArgs(new Vector3(), new Vector3()); }
        }

        public PositionEventArgs(Vector3 previousValue, Vector3 newValue)
        {
            this.previousValue = previousValue;
            this.NewValue = newValue;
        }
    }
}
