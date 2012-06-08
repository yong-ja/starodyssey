using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Controls;

namespace AvengersUtd.Odyssey.UserInterface
{
    public struct UpdateElement
    {
        public BaseControl Control { get; private set; }
        public UpdateAction Action { get; private set; }

        public UpdateElement(BaseControl control, UpdateAction action)
            : this()
        {
            Control = control;
            Action = action;

            if (Action == UpdateAction.None)
                    Console.WriteLine("!Terribile");
        }
    }

}
