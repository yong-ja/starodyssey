using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public interface ISphere
    {
        Vector3 AbsolutePosition { get; }
        float Radius { get; }
    }
}
