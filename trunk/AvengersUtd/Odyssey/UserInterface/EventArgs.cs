using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Controls;

namespace AvengersUtd.Odyssey.UserInterface
{
    public class ControlEventArgs : EventArgs
    {
        public ControlEventArgs(IControl control)
        {
            this.Control = control;
        }

        public IControl Control { get; set; }
    }
}
