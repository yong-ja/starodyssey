using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    interface ISphere
    {
        Vector3 PositionV3 { get; }
        float Radius { get; }
    }
}
