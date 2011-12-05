using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Log;

namespace AvengersUtd.Odyssey.Geometry.Clipping
{

    public class EllipseClipper
    {
        private readonly int slices;
        private readonly double radiusX;
        private readonly double radiusY;
        private readonly double delta;
        private readonly Vector2D center;
        private readonly List<Vector2D> points;
        private readonly List<ushort> indices;
        private readonly float[] offsets;
        private readonly bool[] sliceIntersections;
        private readonly Polygon clipPolygon;
        private readonly PathFigure segments;
        private int innerSegments;

        private EllipseClipper(PolyEllipse ellipse, Polygon polygon)
        {
            if (ellipse.Vertices.Count == 0)
                GeometryEvents.GeometryVerticesException.RaiseArgumentException("ellipse", ellipse.Vertices.Count, ">3");

            if (polygon.Vertices.Count == 0)
                GeometryEvents.GeometryVerticesException.RaiseArgumentException("polygon", ellipse.Vertices.Count, ">4");

            slices = ellipse.Slices;
            radiusX = ellipse.RadiusX;
            radiusY = ellipse.RadiusY;
            center = ellipse.Center;
            delta = MathHelper.TwoPi / slices;
            offsets = ellipse.RingOffsets;
            clipPolygon = polygon;
            segments = (PathFigure)polygon;
            points = new List<Vector2D>();
            indices = new List<ushort>();
            sliceIntersections = new bool[slices];
        }

        public static void ClipAgainstPolygon(PolyEllipse ellipse, Polygon polygon)
        {
            EllipseClipper clipper = new EllipseClipper(ellipse, polygon);
        }

        void FindInnerEllipses()
        {
            Ellipse[] ellipses = (from f in offsets 
                                  select new Ellipse(center, f*radiusX, f*radiusY)).ToArray();

            PolyEllipse[] pEllipses = (from e in ellipses
                                       select PolyEllipse.CreateEllipse(e, slices)).ToArray();

            innerSegments = 0;
            foreach (PolyEllipse pEllipse in pEllipses)
            {
                PolyClipError pError;
                List<Polygon> pResult = YuPengClipper.Intersect(pEllipse, clipPolygon, out pError);
                if (pResult.Count == 0)
                    innerSegments++;
                else
                    break;
            }
        }

        void ClipOuterEllipses()
        {
            
        }


    }


}
