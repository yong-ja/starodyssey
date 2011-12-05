using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public partial class Polygon : IPolygon
    {
        #region Properties

        internal Vertices Vertices { get; private set; }

        public Vector2D[] VerticesArray
        {
            get { return Vertices.ToArray(); }
        }

        public Vector2D Centroid
        {
            get { return ComputeCentroid(this); }
        }

        public double Area
        {
            get { return Math.Abs(Polygon.ComputeSignedArea(this)); }
        }

        public int Count
        {
            get { return Vertices.Count; }
        }

        public bool IsCounterClockWise
        {
            get
            {
                //We just return true for lines
                if (Vertices.Count < 3)
                    return true;

                return (ComputeSignedArea(this) > 0.0);
            }
        }

        #endregion

        #region Constructors

        public Polygon(IEnumerable<Vector2D> points)
            : this()
        {
            Vertices.AddRange(points);
        }

        public Polygon()
        {
            Vertices = new Vertices();
        }

        #endregion

        public static Vector4[] ComputeVector4Array(Vertices vertices, float zIndex)
        {
            Vector4[] pointsArray = new Vector4[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2D point = vertices[i];
                pointsArray[i] = new Vector4((float) point.X, (float) point.Y, zIndex, 1.0f);
            }
            return pointsArray;
        }

        /// <summary>
        /// Forces counter clock wise order.
        /// </summary>
        public void ForceCounterClockWise()
        {
            if (!IsCounterClockWise)
                Vertices.Reverse();
        }

        /// <summary>
        /// Returns an AABB for vertex.
        /// </summary>
        /// <returns></returns>
        public AABB GetCollisionBox()
        {
            AABB aabb;
            Vector2D lowerBound = new Vector2D(float.MaxValue, float.MaxValue);
            Vector2D upperBound = new Vector2D(float.MinValue, float.MinValue);

            for (int i = 0; i < Vertices.Count; ++i)
            {
                if (Vertices[i].X < lowerBound.X)
                    lowerBound.X = Vertices[i].X;
                if (Vertices[i].X > upperBound.X)
                    upperBound.X = Vertices[i].X;
                if (Vertices[i].Y < lowerBound.Y)
                    lowerBound.Y = Vertices[i].Y;
                if (Vertices[i].Y > upperBound.Y)
                    upperBound.Y = Vertices[i].Y;
            }

            aabb.LowerBound = lowerBound;
            aabb.UpperBound = upperBound;

            return aabb;
        }

        public void Detail(double segmentLength)
        {
            PathFigure figure = (PathFigure) Copy();
            figure.Detail(segmentLength);
            Vertices = (Vertices) figure;
        }



        public void Translate(Vector2D vector)
        {
            Translate(ref vector);
        }

        /// <summary>
        /// Translates the vertices with the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public void Translate(ref Vector2D vector)
        {
            for (int i = 0; i < Vertices.Count; i++)
                Vertices[i] = Vector2D.Add(Vertices[i], vector);
        }

        public bool IsPointInside(Vector2D point)
        {
            return Intersection.PolygonPointTest(Vertices, point);
        }

        public ColoredVertex[] CreateColoredVertexArray(Color4[] colors, float zDepth)
        {
            ColoredVertex[] coloredVertices = new ColoredVertex[Count];
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vector2D vector = Vertices[i];
                coloredVertices[i] = new ColoredVertex(new Vector4(vector, zDepth, 1.0f), colors[i]);
            }
            return coloredVertices;
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
            double a; // Partial signed area
            Vector2D[] vertices = polygon.Vertices.ToArray();

            // For all vertices except last
            int i;
            for (i = 0; i < polygon.Vertices.Count - 1; ++i)
            {
                x0 = vertices[i].X;
                y0 = vertices[i].Y;
                x1 = vertices[i + 1].X;
                y1 = vertices[i + 1].Y;
                a = x0*y1 - x1*y0;
                signedArea += a;
                centroid.X += (x0 + x1)*a;
                centroid.Y += (y0 + y1)*a;
            }

            // Do last vertex
            x0 = vertices[i].X;
            y0 = vertices[i].Y;
            x1 = vertices[0].X;
            y1 = vertices[0].Y;
            a = x0*y1 - x1*y0;
            signedArea += a;
            centroid.X += (x0 + x1)*a;
            centroid.Y += (y0 + y1)*a;

            signedArea *= 0.5f;
            centroid.X /= (6*signedArea);
            centroid.Y /= (6*signedArea);

            return centroid;
        }

        public static bool ConvexityTest(Polygon polygon)
        {
            if (polygon.Vertices.Count < 3) return false;

            int xCh = 0, yCh = 0;

            Vector2D a = polygon.Vertices[0] - polygon.Vertices[polygon.Vertices.Count - 1];
            for (int i = 0; i < polygon.Vertices.Count - 1; ++i)
            {
                Vector2D b = polygon.Vertices[i] - polygon.Vertices[i + 1];

                if (Math.Sign(a.X) != Math.Sign(b.X)) ++xCh;
                if (Math.Sign(a.Y) != Math.Sign(b.Y)) ++yCh;

                a = b;
            }

            return (xCh <= 2 && yCh <= 2);
        }

        #endregion

        public static explicit operator PathFigure(Polygon polygon)
        {
            List<Segment> segments = new List<Segment>();
            Vector2D s = polygon.Vertices[polygon.Vertices.Count - 1];
            for (int i = 0; i < polygon.Vertices.Count; i++)
            {
                Vector2D p = polygon.Vertices[i];
                segments.Add(new Segment(s, p));

                s = p;
            }

            return new PathFigure(segments);
        }

        public Polygon Copy()
        {
            return new Polygon(Vertices);
        }
    }
}