using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public interface IEntity
    {
        IRenderable RenderableObject { get; }
    }
}
