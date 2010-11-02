using System;
using System.Collections.Generic;
using AvengersUtd.Odyssey.UserInterface;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public partial class Polygon
    {

        public static Polygon SutherlandHodgmanClip(OrthoRectangle bounds, Polygon p)
        {
            Polygon p1 = SutherlandHodgmanOneAxis(bounds, Borders.Left, p);
            Polygon p2 = SutherlandHodgmanOneAxis(bounds, Borders.Right, p1);
            Polygon p3 = SutherlandHodgmanOneAxis(bounds, Borders.Top, p2);
            Polygon p4 = SutherlandHodgmanOneAxis(bounds, Borders.Bottom, p3);

            return p4;
        }

        public static Polygon SutherlandHodgmanClip(Polygon bounds, Polygon p)
        {
            PathFigure figure = (PathFigure) bounds;
            foreach (Segment segment in figure.Segments)
            {
                Polygon polygon = SutherlandHodgman(segment, bounds, p);
                p = polygon;
            }

            return p;
        }


        public static Polygon SutherlandHodgman(Segment segment, Polygon bounds, Polygon vertices)
        {
            if (vertices.VertexCount== 0)
            {
                return new Polygon();
            }

            List<Vector2> polygon = new List<Vector2>();

            Vector2 centroid = bounds.Centroid;

            Vector2 s = vertices[vertices.VertexCount - 1];
            for (int i = 0; i < vertices.VertexCount; ++i)
            {
                Vector2 p = vertices[i];
                Line line = (Line) segment;

                int sign = Line.DetermineSide(line, centroid);
                int pInSign = Line.DetermineSide(line, p);
                int sInSign = Line.DetermineSide(line, s);
                bool pIn = pInSign == sign;
                bool sIn = sInSign == sign;

                if (sIn && pIn)
                {
                    // case 1: inside -> inside
                    polygon.Add(p);
                }
                else if (sIn && !pIn)
                {
                    // case 2: inside -> outside
                    //polygon.Add(Intersection.SegmentSegmentIntersection(segment, new Segment(s, p)));

                    polygon.Add(Intersection.LineLineIntersection(line,Line.FromTwoPoints(s, p)));
                    
                }
                else if (!sIn && !pIn)
                {
                    // case 3: outside -> outside
                    // emit nothing
                }
                else if (!sIn && pIn)
                {
                    // case 4: outside -> inside
                    //polygon.Add(Intersection.SegmentSegmentIntersection(segment, new Segment(s, p)));
                    polygon.Add(Intersection.LineLineIntersection(line, Line.FromTwoPoints(s, p)));
                    polygon.Add(p);
                }

                s = p;
            }

            return new Polygon(polygon);
        }

        private static Polygon SutherlandHodgmanOneAxis(OrthoRectangle bounds, Borders edge, Polygon v)
        {
            if (v.VertexCount == 0)
            {
                return new Polygon();
            }

            List<Vector2> polygon = new List<Vector2>();

            Vector2 s = v[v.VertexCount - 1];

            for (int i = 0; i < v.VertexCount; ++i)
            {
                Vector2 p = v[i];
                bool pIn = OrthoRectangle.IsInside(bounds, edge, p);
                bool sIn = OrthoRectangle.IsInside(bounds, edge, s);

                if (sIn && pIn)
                {
                    // case 1: inside -> inside
                    polygon.Add(p);
                }
                else if (sIn && !pIn)
                {
                    // case 2: inside -> outside
                    polygon.Add(OrthoRectangle.LineIntercept(bounds, edge, s, p));
                }
                else if (!sIn && !pIn)
                {
                    // case 3: outside -> outside
                    // emit nothing
                }
                else if (!sIn && pIn)
                {
                    // case 4: outside -> inside
                    polygon.Add(OrthoRectangle.LineIntercept(bounds, edge, s, p));
                    polygon.Add(p);
                }

                s = p;
            }

            return new Polygon(polygon);
        }
    }
}
