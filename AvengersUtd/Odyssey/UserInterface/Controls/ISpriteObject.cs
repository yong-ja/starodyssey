using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public interface ISpriteObject
    {
        IRenderable RenderableObject { get; }
        bool Inited { get; }
        void CreateResource();
        void CreateShape();
        void ComputeAbsolutePosition();
    }
}
