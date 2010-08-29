using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Controls;

namespace AvengersUtd.Odyssey.UserInterface
{
    public struct UpdateElement
    {
        public BaseControl Control { get; set; }
        public UpdateAction Action { get; set; }
    }

}
