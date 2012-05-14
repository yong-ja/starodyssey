﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Graphics.Meshes;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public interface IMouseBehaviour
    {
        IRenderable RenderableObject { get;  }
        void OnMouseDown(object sender, MouseEventArgs e);
        void OnMouseClick(object sender, MouseEventArgs e);
        void OnMouseUp(object sender, MouseEventArgs e);
        void OnMouseMove(object sender, MouseEventArgs e);

    }
}
