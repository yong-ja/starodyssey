using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface;

namespace AvengersUtd.Odyssey.Geometry
{
    public partial class Polygon
    {

        public static Polygon SutherlandHodgmanClip(OrthoRectangleF bounds, Polygon p)
        {
            Polygon p1 = SutherlandHodgmanOneAxis(bounds, Borders.Left, p.Points);
            Polygon p2 = SutherlandHodgmanOneAxis(bounds, Borders.Right, p1.Points);
            Polygon p3 = SutherlandHodgmanOneAxis(bounds, Borders.Top, p2.Points);
            Polygon p4 = SutherlandHodgmanOneAxis(bounds, Borders.Bottom, p3.Points);

            return p4;
        }

        private static Polygon SutherlandHodgmanOneAxis(OrthoRectangleF bounds, Borders edge, List<PointF> v)
        {
            if (v.Count == 0)
            {
                return new Polygon();
            }

            List<PointF> polygon = new List<PointF>();

            PointF s = v[v.Count - 1];

            for (int i = 0; i < v.Count; ++i)
            {
                PointF p = v[i];
                bool pIn = IsInside(bounds, edge, p);
                bool sIn = IsInside(bounds, edge, s);

                if (sIn && pIn)
                {
                    // case 1: inside -> inside
                    polygon.Add(p);
                }
                else if (sIn && !pIn)
                {
                    // case 2: inside -> outside
                    polygon.Add(LineIntercept(bounds, edge, s, p));
                }
                else if (!sIn && !pIn)
                {
                    // case 3: outside -> outside
                    // emit nothing
                }
                else if (!sIn && pIn)
                {
                    // case 4: outside -> inside
                    polygon.Add(LineIntercept(bounds, edge, s, p));
                    polygon.Add(p);
                }

                s = p;
            }

            return new Polygon(polygon);
        }

        private static bool IsInside(OrthoRectangleF bounds, Borders edge, PointF p)
        {
            switch (edge)
            {
                case Borders.Left:
                    return !(p.X <= bounds.Left);

                case Borders.Right:
                    return !(p.X >= bounds.Right);

                case Borders.Top:
                    return !(p.Y >= bounds.Top);

                case Borders.Bottom:
                    return !(p.Y <= bounds.Bottom);

                default:
                    throw Error.WrongCase("edge", "IsInside", edge);
            }
        }


        private static PointF LineIntercept(OrthoRectangleF bounds, Borders edge, PointF a, PointF b)
        {
            if (a == b)
            {
                return a;
            }

            switch (edge)
            {
                case Borders.Bottom:
                    if (b.Y == a.Y)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new PointF(a.X + (((b.X - a.X) * (bounds.Bottom - a.Y)) / (b.Y - a.Y)), bounds.Bottom);

                case Borders.Left:
                    if (b.X == a.X)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new PointF(bounds.Left, a.Y + (((b.Y - a.Y) * (bounds.Left - a.X)) / (b.X - a.X)));

                case Borders.Right:
                    if (b.X == a.X)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new PointF(bounds.Right, a.Y + (((b.Y - a.Y) * (bounds.Right - a.X)) / (b.X - a.X)));

                case Borders.Top:
                    if (b.Y == a.Y)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new PointF(a.X + (((b.X - a.X) * (bounds.Top - a.Y)) / (b.Y - a.Y)), bounds.Top);
            }

            throw new ArgumentException("no intercept found");
        }
    }
}
