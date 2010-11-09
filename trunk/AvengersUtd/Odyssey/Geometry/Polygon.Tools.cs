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


        public static Polygon SutherlandHodgman(Segment segment, Polygon bounds, Polygon polygon)
        {
            if (polygon.Vertices.Count== 0)
            {
                return new Polygon();
            }

            VerticesCollection vc = new VerticesCollection();

            Vector2D centroid = bounds.Centroid;

            Vector2D s = polygon.Vertices[polygon.Vertices.Count - 1];
            for (int i = 0; i < polygon.Vertices.Count; ++i)
            {
                Vector2D p = polygon.Vertices[i];
                Line line = (Line) segment;

                int sign = Line.DetermineSide(line, centroid);
                int pInSign = Line.DetermineSide(line, p);
                int sInSign = Line.DetermineSide(line, s);
                bool pIn = pInSign == sign;
                bool sIn = sInSign == sign;

                if (sIn && pIn)
                {
                    // case 1: inside -> inside
                    vc.Add(p);
                }
                else if (sIn && !pIn)
                {
                    // case 2: inside -> outside
                    //polygon.Add(Intersection.SegmentSegmentIntersection(segment, new Segment(s, p)));

                    vc.Add(Intersection.LineLineIntersection(line,Line.FromTwoPoints(s, p)));
                    
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
                    //polygon.Add(Intersection.LineLineIntersection(line, Line.FromTwoPoints(s, p)));
                    vc.Add(p);
                }

                s = p;
            }

            return new Polygon(vc);
        }

        private static Polygon SutherlandHodgmanOneAxis(OrthoRectangle bounds, Borders edge, Polygon polygon)
        {
            if (polygon.Vertices.Count == 0)
            {
                return new Polygon();
            }

            VerticesCollection vc = new VerticesCollection();

            Vector2D s = polygon.Vertices[polygon.Vertices.Count - 1];

            for (int i = 0; i < polygon.Vertices.Count; ++i)
            {
                Vector2D p = polygon.Vertices[i];
                bool pIn = OrthoRectangle.IsPointInside(bounds, edge, p);
                bool sIn = OrthoRectangle.IsPointInside(bounds, edge, s);

                if (sIn && pIn)
                {
                    // case 1: inside -> inside
                    vc.Add(p);
                }
                else if (sIn && !pIn)
                {
                    // case 2: inside -> outside
                    vc.Add(OrthoRectangle.LineIntercept(bounds, edge, s, p));
                }
                else if (!sIn && !pIn)
                {
                    // case 3: outside -> outside
                    // emit nothing
                }
                else if (!sIn && pIn)
                {
                    // case 4: outside -> inside
                    vc.Add(OrthoRectangle.LineIntercept(bounds, edge, s, p));
                    vc.Add(p);
                }

                s = p;
            }

            return new Polygon(vc);
        }
    }
}
