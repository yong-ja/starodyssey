using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public partial class Polygon
    {

        public static double ComputeSignedArea(Polygon polygon)
        {
            int i;
            double area = 0;

            Vertices vertices = polygon.Vertices;

            for (i = 0; i < vertices.Count; i++)
            {
                int j = (i + 1)%vertices.Count;
                area += vertices[i].X*vertices[j].Y;
                area -= vertices[i].Y*vertices[j].X;
            }
            area /= 2.0;
            return area;

        }
    }
}
