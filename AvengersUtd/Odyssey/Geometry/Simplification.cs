using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public static class Simplification
    {
        private static bool[] _usePt;
        private static double _distanceTolerance;
        /// <summary>
        /// Reduces the polygon by distance.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="distance">The distance between points. Points closer than this will be 'joined'.</param>
        /// <returns></returns>
        public static Vertices ReduceByDistance(Vertices vertices, float distance)
        {
            //We can't simplify polygons under 3 vertices
            if (vertices.Count < 3)
                return vertices;

            Vertices simplified = new Vertices();

            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2D current = vertices[i];
                Vector2D next = vertices.NextVertex(i);

                //If they are closer than the distance, continue
                if ((next - current).LengthSquared() <= distance)
                    continue;

                simplified.Add(current);
            }

            return simplified;
        }

        /// <summary>
        /// Ramer-Douglas-Peucker polygon simplification algorithm. This is the general recursive version that does not use the
        /// speed-up technique by using the Melkman convex hull.
        /// 
        /// If you pass in 0, it will remove all collinear points
        /// </summary>
        /// <returns>The simplified polygon</returns>
        public static Vertices DouglasPeuckerSimplify(Vertices vertices, float distanceTolerance)
        {
            _distanceTolerance = distanceTolerance;

            _usePt = new bool[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
                _usePt[i] = true;

            SimplifySection(vertices, 0, vertices.Count - 1);
            Vertices result = new Vertices();

            for (int i = 0; i < vertices.Count; i++)
                if (_usePt[i])
                    result.Add(vertices[i]);

            return result;
        }

        private static void SimplifySection(Vertices vertices, int i, int j)
        {
            if ((i + 1) == j)
                return;

            Vector2D A = vertices[i];
            Vector2D B = vertices[j];
            double maxDistance = -1.0;
            int maxIndex = i;
            for (int k = i + 1; k < j; k++)
            {
                double distance = DistancePointLine(vertices[k], A, B);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxIndex = k;
                }
            }
            if (maxDistance <= _distanceTolerance)
                for (int k = i + 1; k < j; k++)
                    _usePt[k] = false;
            else
            {
                SimplifySection(vertices, i, maxIndex);
                SimplifySection(vertices, maxIndex, j);
            }
        }

        private static double DistancePointPoint(Vector2D p, Vector2D p2)
        {
            double dx = p.X - p2.X;
            double dy = p.Y - p2.X;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private static double DistancePointLine(Vector2D p, Vector2D A, Vector2D B)
        {
            // if start == end, then use point-to-point distance
            if (A.X == B.X && A.Y == B.Y)
                return DistancePointPoint(p, A);

            // otherwise use comp.graphics.algorithms Frequently Asked Questions method
            /*(1)     	      AC dot AB
                        r =   ---------
                              ||AB||^2
             
		                r has the following meaning:
		                r=0 Point = A
		                r=1 Point = B
		                r<0 Point is on the backward extension of AB
		                r>1 Point is on the forward extension of AB
		                0<r<1 Point is interior to AB
	        */

            double r = ((p.X - A.X) * (B.X - A.X) + (p.Y - A.Y) * (B.Y - A.Y))
                       /
                       ((B.X - A.X) * (B.X - A.X) + (B.Y - A.Y) * (B.Y - A.Y));

            if (r <= 0.0) return DistancePointPoint(p, A);
            if (r >= 1.0) return DistancePointPoint(p, B);


            /*(2)
		                    (Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
		                s = -----------------------------
		             	                Curve^2

		                Then the distance from C to Point = |s|*Curve.
	        */

            double s = ((A.Y - p.Y) * (B.X - A.X) - (A.X - p.X) * (B.Y - A.Y))
                       /
                       ((B.X - A.X) * (B.X - A.X) + (B.Y - A.Y) * (B.Y - A.Y));

            return Math.Abs(s) * Math.Sqrt(((B.X - A.X) * (B.X - A.X) + (B.Y - A.Y) * (B.Y - A.Y)));
        }

    }
}
