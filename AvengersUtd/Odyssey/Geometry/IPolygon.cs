using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public interface IPolygon
    {
        Vector2D this[int index] { get; }
        int Count { get; }
        Vector2D Centroid { get; }
        double Area { get; }
        Vector4[] ComputeVector4Array(float zIndex);
        bool IsPointInside(Vector2D point);
    }
}
