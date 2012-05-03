using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public interface ISpriteObject : IDisposable
    {
        IRenderable RenderableObject { get; }
        bool Inited { get; }
        bool Disposed { get; }
        void CreateResource();
        void CreateShape();
        void ComputeAbsolutePosition();
    }
}
