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
            Polygon p1 = SutherlandHodgmanOneAxis(bounds, Borders.Left, p.Points);
            Polygon p2 = SutherlandHodgmanOneAxis(bounds, Borders.Right, p1.Points);
            Polygon p3 = SutherlandHodgmanOneAxis(bounds, Borders.Top, p2.Points);
            Polygon p4 = SutherlandHodgmanOneAxis(bounds, Borders.Bottom, p3.Points);

            return p4;
        }

        private static Polygon SutherlandHodgmanOneAxis(OrthoRectangle bounds, Borders edge, List<Vector2> v)
        {
            if (v.Count == 0)
            {
                return new Polygon();
            }

            List<Vector2> polygon = new List<Vector2>();

            Vector2 s = v[v.Count - 1];

            for (int i = 0; i < v.Count; ++i)
            {
                Vector2 p = v[i];
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

        private static bool IsInside(OrthoRectangle bounds, Borders edge, Vector2 p)
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


        private static Vector2 LineIntercept(OrthoRectangle bounds, Borders edge, Vector2 a, Vector2 b)
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

                    return new Vector2(a.X + (((b.X - a.X) * (bounds.Bottom - a.Y)) / (b.Y - a.Y)), bounds.Bottom);

                case Borders.Left:
                    if (b.X == a.X)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new Vector2(bounds.Left, a.Y + (((b.Y - a.Y) * (bounds.Left - a.X)) / (b.X - a.X)));

                case Borders.Right:
                    if (b.X == a.X)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new Vector2(bounds.Right, a.Y + (((b.Y - a.Y) * (bounds.Right - a.X)) / (b.X - a.X)));

                case Borders.Top:
                    if (b.Y == a.Y)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new Vector2(a.X + (((b.X - a.X) * (bounds.Top - a.Y)) / (b.Y - a.Y)), bounds.Top);
            }

            throw new ArgumentException("no intercept found");
        }
    }
}
