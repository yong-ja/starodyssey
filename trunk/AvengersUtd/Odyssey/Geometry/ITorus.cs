using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public interface ITorus
    {
        Vector3 AbsolutePosition { get; }
        float InnerRadius { get; }
        float SectionRadius { get; }
        float RingRadius { get; }
    }
}
