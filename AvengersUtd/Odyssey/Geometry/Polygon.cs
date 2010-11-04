using System;
using System.Collections.Generic;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public partial class Polygon : IPolygon, IList<Vector2D>
    {
        private readonly List<Vector2D> vertices;

        #region Properties
        protected List<Vector2D> Vertices
        {
            get { return vertices; }
        }

        public Vector2D Centroid
        {
            get { return ComputeCentroid(this); }
        } 

        public double Area
        { get { return PolygonArea(this); } }
        #endregion

        #region Constructors
        public Polygon(IEnumerable<Vector2D> points)
            : this()
        {
            vertices.AddRange(points);
        }

        private Polygon()
        {
            vertices = new List<Vector2D>();
        }

        #endregion

        public Vector4[] ComputeVector4Array(float zIndex)
        {
            Vector4[] pointsArray = new Vector4[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
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
            Vector2D e0 = vertices[Count - 1];
            bool y0 = e0.Y >= t.Y;

            for (int i=0; i < Count-1; i++)
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
            for (i = 0; i < polygon.Count - 1; ++i)
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

        public static double PolygonArea(Polygon polygon)
        {
            // Add the first point to the end.
            int numPoints = polygon.Count;

            // Get the areas.
            double area = 0;
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
            Vector2D s = v[v.Count - 1];
            for (int i = 0; i < v.Count; i++)
            {
                Vector2D p = v[i];
                segments.Add(new Segment(s,p));

                s = p;
            }

            return new PathFigure(segments);
        }

        #region ICollection<Vector2D> Members

        public void Add(Vector2D item)
        {
            vertices.Add(item);
        }

        void ICollection<Vector2D>.Clear()
        {
            vertices.Clear();
        }

        bool ICollection<Vector2D>.Contains(Vector2D item)
        {
            return vertices.Contains(item);
        }

        void ICollection<Vector2D>.CopyTo(Vector2D[] array, int arrayIndex)
        {
            vertices.CopyTo(array, arrayIndex);
        }

        bool ICollection<Vector2D>.IsReadOnly
        {
            get { return false; }
        }

        public int Count
        {
            get { return vertices.Count; }
        }

        public bool Remove(Vector2D item)
        {
            if (!vertices.Contains(item))
                return false;
            
            vertices.Remove(item);
            return true;
        }

        #endregion

        #region IEnumerable<Vector2D> Members

        IEnumerator<Vector2D> IEnumerable<Vector2D>.GetEnumerator()
        {
            return vertices.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return vertices.GetEnumerator();
        }

        #endregion

        #region IList<Vector2D> Members

        int IList<Vector2D>.IndexOf(Vector2D item)
        {
            return vertices.IndexOf(item);
        }

        public void Insert(int index, Vector2D item)
        {
            vertices.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            vertices.RemoveAt(index);
        }

        public Vector2D this[int index]
        {
            get
            {
                if (index < 0 || index > Count)
                    throw Error.IndexNotPresentInArray("this", index);
                return vertices[index];
            }
            set
            {
                if (index < 0 || index > Count)
                    throw Error.IndexNotPresentInArray("this", index);
                vertices[index] = value;
            }
        }


        #endregion
        
    }
}
