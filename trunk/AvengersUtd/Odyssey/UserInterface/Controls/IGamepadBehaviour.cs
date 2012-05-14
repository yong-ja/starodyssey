using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Input;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public interface IGamepadBehaviour : IBehaviour
    {
        void OnButtonPress(object sender, GamepadEventArgs e);
    }
}
