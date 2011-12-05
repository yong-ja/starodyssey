using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public class PolyEllipse : Polygon
    {
        private static readonly TraceSource ts = new TraceSource("Geometry");

        public Vector2D Center { get; private set; }
        public double RadiusX { get; private set; }
        public double RadiusY { get; private set; }
        public int Slices { get; private set; }
        public int Rings { get; private set; }
        public float[] RingOffsets { get; private set; }

        public double SegmentLength
        {
            get { return ComputeEllipseSegmentLength(Center, RadiusX, RadiusY, Slices); }
        }

        public PolyEllipse(IEnumerable<Vector2D> points, Vector2D center, double radiusX, double radiusY, int slices, float[] ringOffsets) : base(points)
        {
            Center = center;
            RadiusX = radiusX;
            RadiusY = radiusY;
            Slices = slices;
            Rings = ringOffsets.Length-1;
            RingOffsets = ringOffsets;
        }

        public Polygon GetRingSlice(int sliceIndex, int ringIndexStart, int ringIndexEnd)
        {
            double x = Center.X;
            double y = Center.Y;
            double delta = MathHelper.TwoPi/Slices;
            float offset0 = RingOffsets[ringIndexStart];
            float offset1 = RingOffsets[ringIndexEnd];
            Vector2D p2 = CreateEllipseVertex(x, y, offset1 * RadiusX, offset1* RadiusY, sliceIndex * delta);
            Vector2D p3 = CreateEllipseVertex(x, y, offset1 * RadiusX, offset1 * RadiusY, (sliceIndex + 1) * delta);

            if (ringIndexStart == 0)
                return new Polygon(new[] { Center, p2, p3 });

            Vector2D p0 = CreateEllipseVertex(x, y, offset0 * RadiusX, offset0 * RadiusY, sliceIndex + 1 * delta);
            Vector2D p1 = CreateEllipseVertex(x, y, offset0*RadiusX, offset0*RadiusY, (sliceIndex + 1)*delta);

            return new Polygon(new[] {p0, p1, p2, p3});

        }

        public static double ComputeEllipseSegmentLength(Vector2D center, double radiusX, double radiusY, int slices)
        {
            double x = center.X;
            double y = center.Y;
            double delta = MathHelper.TwoPi/slices;
            Vector2D[] points = new Vector2D[2];
            for (int i = 0; i < 2; i++)
            {
                double theta = i*delta;
                points[i] = CreateEllipseVertex(x, y, radiusX, radiusY, theta);
            }

            Segment segment = new Segment(points[0], points[1]);
            return segment.Length;
        }

        internal static Vector2D CreateEllipseVertex(Vector2D center, double radiusX, double radiusY, double theta, float offset)
        {
            return new Vector2D(center.X + Math.Cos(theta) * radiusX * offset, center.Y - Math.Sin(theta) * radiusY * offset);
        }

        internal static Vector2D CreateEllipseVertex(double x, double y, double radiusX, double radiusY, double theta)
        {
            return new Vector2D(x + Math.Cos(theta)*radiusX, y - Math.Sin(theta)*radiusY);
        }

        public static PolyEllipse CreateEllipse(Ellipse ellipse, int slices)
        {
            return CreateEllipse(ellipse.Center, ellipse.RadiusX, ellipse.RadiusY, slices, new[] { 0f, 1f});
        }

        public static PolyEllipse CreateEllipse(Ellipse ellipse, int slices, float[] offsets)
        {
            return CreateEllipse(ellipse.Center, ellipse.RadiusX, ellipse.RadiusY, slices, offsets);
        }

        public static PolyEllipse CreateEllipse(Vector2D center, double radiusX, double radiusY, int slices, float[] offsets)
        {
            double delta = MathHelper.TwoPi/slices;
            Vector2D[] points = new Vector2D[slices];
            points[0] = center;

            for (int r = 0; r < offsets.Length; r++)
                for (int i = 0; i < slices; i++)
                {
                    double theta = i*delta;
                    points[i] = CreateEllipseVertex(center, radiusX, radiusY, theta, offsets[r]);
                }
            return new PolyEllipse(points, center, radiusX, radiusY, slices, offsets);
        }

        internal static ushort[] TriangulateEllipseFirstRing(int slices)
        {
            ushort[] indices = new ushort[3 * slices];
            TriangulateEllipseFirstRing(slices, ref indices);
            return indices;
        }

        internal static void TriangulateEllipseFirstRing(int slices, ref ushort[] indices, int startIndex=0)
        {
            // First ring indices
            for (int i = 0; i < slices; i++)
            {
                indices[startIndex + (3 * i)] = 0;
                indices[startIndex + (3 * i) + 2] = (ushort)(i + 2);
                indices[startIndex + (3 * i) + 1] = (ushort)(i + 1);
            }
            indices[(slices * 3) - 1] = 1;
        }

    }
}
