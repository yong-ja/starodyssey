using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public interface IPlane
    {
        Vector3 Normal { get; }
        float Distance { get; }
        Plane BoundingPlane { get; }
    }
}
