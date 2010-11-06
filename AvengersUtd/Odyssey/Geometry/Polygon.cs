using System;
using System.Collections.Generic;
using System.Linq;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public partial class Polygon : IPolygon
    {
        private readonly Vector2D[] vertices;

        #region Properties

        public Vector2D this[int index]
        {
            get { return vertices[index]; }
        }

        public Vector2D Centroid
        {
            get { return ComputeCentroid(this); }
        }

        public double Area { get { return ComputeArea(this); } }

        public int VerticesCount
        {
            get { return vertices.Length; }
        }

        public bool IsCounterClockWise
        {
            get
            {
                //We just return true for lines
                if (VerticesCount < 3)
                    return true;

                return (Polygon.ComputeSignedArea(this) > 0.0);
            }
        }
        #endregion

        #region Constructors
        public Polygon(IEnumerable<Vector2D> points)
            : this()
        {
            vertices = points.ToArray();
        }

        private Polygon()
        {
            vertices = new Vector2D[0];
        }
        #endregion

        public Vector4[] ComputeVector4Array(float zIndex)
        {
            Vector4[] pointsArray = new Vector4[VerticesCount];
            for (int i = 0; i < VerticesCount; i++)
            {
                Vector2D point = vertices[i];
                pointsArray[i] = new Vector4((float)point.X, (float)point.Y, zIndex, 1.0f);
            }
            return pointsArray;
        }

        public bool IsPointInside(Vector2D point)
        {
            // Crossing Test
            // Source: Real Time Rendering 3rd Edition, p. 754

            bool inside = false;
            Vector2D t = point;
            Vector2D e0 = vertices[VerticesCount - 1];
            bool y0 = e0.Y >= t.Y;

            for (int i = 0; i < VerticesCount - 1; i++)
            {
                Vector2D e1 = vertices[i];
                bool y1 = e1.Y >= t.Y;
                if (y0 != y1)
                    if (((e1.Y - t.Y) * (e0.X - e1.X) >= (e1.X - t.X) * (e0.Y - e1.Y)) == y1)
                        inside = !inside;
                y0 = y1;
                e0 = e1;
            }
            return inside;
        }

        public void ReverseVerticesOrder()
        {
            vertices.Reverse();
        }

        #region Static methods
        public static Vector2D ComputeCentroid(Polygon polygon)
        {
            Vector2D centroid = new Vector2D(0, 0);
            double signedArea = 0.0f;
            double x0; // Current vertex X
            double y0; // Current vertex Y
            double x1; // Next vertex X
            double y1; // Next vertex Y
            double a;  // Partial signed area
            Vector2D[] vertices = polygon.vertices.ToArray();

            // For all vertices except last
            int i;
            for (i = 0; i < polygon.VerticesCount - 1; ++i)
            {
                x0 = vertices[i].X;
                y0 = vertices[i].Y;
                x1 = vertices[i + 1].X;
                y1 = vertices[i + 1].Y;
                a = x0 * y1 - x1 * y0;
                signedArea += a;
                centroid.X += (x0 + x1) * a;
                centroid.Y += (y0 + y1) * a;
            }

            // Do last vertex
            x0 = vertices[i].X;
            y0 = vertices[i].Y;
            x1 = vertices[0].X;
            y1 = vertices[0].Y;
            a = x0 * y1 - x1 * y0;
            signedArea += a;
            centroid.X += (x0 + x1) * a;
            centroid.Y += (y0 + y1) * a;

            signedArea *= 0.5f;
            centroid.X /= (6 * signedArea);
            centroid.Y /= (6 * signedArea);

            return centroid;
        }

        public static double ComputeArea(Polygon polygon)
        {
            // Return the result.
            return Math.Abs(ComputeSignedArea(polygon));
        }

        public static double ComputeSignedArea(Polygon polygon)
        {
            // Add the first point to the end.
            int numPoints = polygon.VerticesCount;

            // Get the areas.
            double area = 0;
            for (int i = 0; i < numPoints; i++)
            {
                area +=
                    (polygon[(i + 1) % numPoints].X - polygon[i].X) *
                    (polygon[(i + 1) % numPoints].Y + polygon[i].Y) / 2;
            }

            return area;
        }

        public static bool ConvexityTest(Polygon polygon)
        {
            
                if (polygon.VerticesCount < 3) return false;

                int xCh = 0, yCh = 0;

                Vector2D a = polygon[0] - polygon[polygon.VerticesCount-1];
                for (int i = 0; i < polygon.VerticesCount - 1; ++i)
                {
                    Vector2D b = polygon[i] - polygon[i + 1];

                    if (Math.Sign(a.X) != Math.Sign(b.X)) ++xCh;
                    if (Math.Sign(a.Y) != Math.Sign(b.Y)) ++yCh;

                    a = b;
                }

                return (xCh <= 2 && yCh <= 2);
            }

        #endregion

        static Vector2D CreateEllipseVertex(double x, double y, double radiusX, double radiusY, double theta)
        {
            return new Vector2D
                    (x + Math.Cos(theta)*radiusX,
                    y - Math.Sin(theta)*radiusY);
        }

        public static Polygon CreateEllipse(Vector2D center, double radiusX, double radiusY, int slices)
        {
            double x = center.X;
            double y = center.Y;
            double delta = MathHelper.TwoPi / slices;
            Vector2D[] points = new Vector2D[slices];
            for (int i = 0; i < slices; i++)
            {
                double theta = i * delta;
                points[i] = CreateEllipseVertex(x, y, radiusX, radiusY, theta);
            }
            
            return new Polygon(points);
        }

        public static double ComputeEllipseSegmentLength(Vector2D center, double radiusX, double radiusY, int slices)
        {
            double x = center.X;
            double y = center.Y;
            double delta = MathHelper.TwoPi / slices;
            Vector2D[] points = new Vector2D[2];
            for (int i = 0; i < 2; i++)
            {
                double theta = i * delta;
                points[i] = CreateEllipseVertex(x, y, radiusX, radiusY, theta);
            }

            Segment segment = new Segment(points[0], points[1]);
            return segment.Length;
        }

        public static explicit operator PathFigure(Polygon v)
        {
            List<Segment> segments = new List<Segment>();
            Vector2D s = v[v.VerticesCount - 1];
            for (int i = 0; i < v.VerticesCount; i++)
            {
                Vector2D p = v[i];
                segments.Add(new Segment(s,p));

                s = p;
            }

            return new PathFigure(segments);
        }


        
    }
}
