using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using MouseEventArgs = AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public interface IBehaviour
    {
        IRenderable RenderableObject { get; set; }
        void Add();
        void Remove();
        string Name { get; }
    }

    public interface IMouseBehaviour : IBehaviour
    {
        void OnMouseDown(object sender, MouseEventArgs e);
        void OnMouseClick(object sender, MouseEventArgs e);
        void OnMouseUp(object sender, MouseEventArgs e);
        void OnMouseMove(object sender, MouseEventArgs e);

    }
}
