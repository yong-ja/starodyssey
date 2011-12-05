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

        /// <summary>
        /// Winding number test for a point in a polygon.
        /// </summary>
        /// See more info about the algorithm here: http://softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm
        /// <param name="point">The point to be tested.</param>
        /// <returns>-1 if the winding number is zero and the point is outside
        /// the polygon, 1 if the point is inside the polygon, and 0 if the point
        /// is on the polygons edge.</returns>
        public static int PointInPolygon(Polygon polygon, Vector2D point)
        {
            // Winding number
            int wn = 0;

            Vertices polyVertices = polygon.Vertices;
            // Iterate through polygon's edges
            for (int i = 0; i < polyVertices.Count; i++)
            {
                // Get points
                Vector2D p1 = polyVertices[i];
                Vector2D p2 = polyVertices[polyVertices.NextIndex(i)];

                // Test if a point is directly on the edge
                Vector2D edge = p2 - p1;
                double area = MathHelper.Area(ref p1, ref p2, ref point);
                if (Math.Abs(area - 0f) < MathHelper.EpsilonD && Vector2D.Dot(point - p1, edge) >= 0 && Vector2D.Dot(point - p2, edge) <= 0)
                {
                    return 0;
                }
                // Test edge for intersection with ray from point
                if (p1.Y <= point.Y)
                {
                    if (p2.Y > point.Y && area > 0)
                    {
                        ++wn;
                    }
                }
                else
                {
                    if (p2.Y <= point.Y && area < 0)
                    {
                        --wn;
                    }
                }
            }
            return wn;
        }
     
    }
}
