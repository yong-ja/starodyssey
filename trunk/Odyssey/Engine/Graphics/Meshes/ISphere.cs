using System;
using System.Collections.Generic;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public interface ISphere : IEntity
    {
        Vector3 Center { get; }
        float Radius { get; }

        BoundingSphere BoundingSphere { get; }
    }
}