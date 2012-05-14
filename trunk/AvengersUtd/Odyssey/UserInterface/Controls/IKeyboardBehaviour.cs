using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using System.Windows.Forms;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    interface IKeyboardBehaviour : IBehaviour
    {
        void OnKeyDown(object sender, KeyEventArgs e);
        void OnKeyPress(object sender, KeyEventArgs e);
        void OnKeyUp(object sender, KeyEventArgs e);
    }
}
