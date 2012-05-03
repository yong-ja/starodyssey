using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public interface IBox
    {
        Vector3 PositionV3 { get; }
        float Width { get; }
        float Height { get; }
        float Depth { get; }
    }
}
