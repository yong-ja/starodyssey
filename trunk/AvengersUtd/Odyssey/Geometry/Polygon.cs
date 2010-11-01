using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public partial class Polygon
    {
        private readonly List<Vector2> vertices;

        protected internal List<Vector2> Vertices
        {
            get { return vertices; }
        }

        #region Properties
        public Vector2 this[int index]
        {
            get { return vertices[index]; }
        }

        public int VertexCount
        {
            get { return vertices.Count; }
        }

        public Vector2 Centroid
        {
            get { return ComputeCentroid(this); }
        } 

        public float Area
        { get { return Polygon.PolygonArea(this); } }
        #endregion

        #region Constructors
        public Polygon(IEnumerable<Vector2> points)
            : this()
        {
            this.vertices.AddRange(points);
        }

        private Polygon()
        {
            this.vertices = new List<Vector2>();
        }

        #endregion

        public Vector4[] ComputeVector4Array(float zIndex)
        {
            Vector4[] pointsArray = new Vector4[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 point = vertices[i];
                pointsArray[i] = new Vector4(point.X, point.Y, zIndex, 1.0f);
            }
            return pointsArray;
        }

        public bool IsPointInside(Vector2 point)
        {
            // Crossing Test
            // Source: Real Time Rendering 3rd Edition, p. 754

            bool inside = false;
            Vector2 t = point;
            Vector2 e0 = vertices[VertexCount - 1];
            bool y0 = e0.Y >= t.Y;

            for (int i=0; i < VertexCount-1; i++)
            {
                Vector2 e1 = vertices[i];
                bool y1 = e1.Y >= t.Y;
                if (y0 != y1)
                    if (((e1.Y - t.Y) * (e0.X - e1.X) >= (e1.X - t.X) * (e0.Y - e1.Y)) == y1)
                        inside = !inside;
                y0 = y1;
                e0 = e1;
            }
            return inside;
        }

        #region Static methods
        public static Vector2 ComputeCentroid(Polygon polygon)
        {
            Vector2 centroid = new Vector2(0, 0);
            float signedArea = 0.0f;
            float x0; // Current vertex X
            float y0; // Current vertex Y
            float x1; // Next vertex X
            float y1; // Next vertex Y
            float a;  // Partial signed area
            Vector2[] vertices = polygon.vertices.ToArray();

            // For all vertices except last
            int i = 0;
            for (i = 0; i < polygon.VertexCount - 1; ++i)
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

        public static float PolygonArea(Polygon polygon)
        {
            // Add the first point to the end.
            int numPoints = polygon.VertexCount;

            // Get the areas.
            float area = 0;
            for (int i = 0; i < numPoints; i++)
            {
                area +=
                    (polygon[(i + 1) % numPoints].X - polygon[i].X) *
                    (polygon[(i + 1) % numPoints].Y + polygon[i].Y) / 2;
            }

            // Return the result.
            return Math.Abs(area);
        }
      
        #endregion

        public static Polygon CreateEllipse(Vector2 center, float radiusX, float radiusY, int slices)
        {
            float x = center.X;
            float y = center.Y;
            float delta = MathHelper.TwoPi / slices;
            Vector2[] points = new Vector2[slices];
            for (int i = 0; i < slices; i++)
            {
                float theta = i * delta;
                points[i] = new Vector2
                    (x + (float) Math.Cos(theta)*radiusX,
                    y - (float) Math.Sin(theta)*radiusY);
            }
            
            return new Polygon(points);
        }

        public static explicit operator PathGeometry(Polygon v)
        {
            List<Segment> segments = new List<Segment>();
            Vector2 s = v[v.VertexCount - 1];
            for (int i = 0; i < v.VertexCount -1; i++)
            {
                Vector2 p = v[i];
                segments.Add(new Segment(s,p));

                s = p;
            }

            return new PathGeometry(segments);
        }
    }
}
