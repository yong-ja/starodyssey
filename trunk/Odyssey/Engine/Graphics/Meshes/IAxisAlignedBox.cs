using System;
using System.Collections.Generic;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public interface IAxisAlignedBox
    {
        Vector3 Minimum { get; }
        Vector3 Maximum { get; }

        BoundingBox BoundingBox { get; }
    }
}
