using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public static class Intersection
    {
        #region Line - X intersection methods
        public static Vector2D LineLineIntersection(Line line1, Line line2)
        {
            // Source: Real-Time Rendering, Third Edition
            // Reference: Page 780

            Vector2D d1 = line1.Direction;
            Vector2D d2 = line2.Direction;
            Vector2D d1P = Vector2D.Perp(d1);
            Vector2D d2P = Vector2D.Perp(d2);

            double d1d2P = Vector2D.Dot(d1, d2P);

            if (d1d2P == 0)
                return new Vector2D(Double.NaN); // parallel

            Vector2D o1 = line1.Origin;
            Vector2D o2 = line2.Origin;

            double s = Vector2D.Dot((o2 - o1), d2P) / d1d2P;
            return o1 + s * d1;

        } 

        public static bool LineLineTest(Line line1, Line line2, out Vector2D intersection)
        {
            intersection = LineLineIntersection(line1, line2);
            return !double.IsNaN(intersection.X);
        }
        #endregion

        #region Segment - X intersection methods
        public static bool SegmentSegmentTest(Segment a, Segment b)
        {
            Vector2D u = a.Direction;
            Vector2D v = b.Direction;
            
            double d = u.X*v.Y - u.Y*v.X;
            
            if (Math.Abs(d) < MathHelper.Epsilon) return false; //parallel test

            Vector2D w = a.StartPoint - b.StartPoint;

            double s = v.X*w.Y - v.Y*w.X;
            if (s < 0 || s > d) return false;

            double t = u.X*w.Y - u.Y*w.X;
            if (t < 0 || t > d) return false;

            return true;
        }

        /// <summary>
        /// Performs segment-segment intersection.
        /// </summary>
        /// <param name="segment1">First segment.</param>
        /// <param name="segment2">Second segment.</param>
        /// <param name="intersectionPoint">Returns the computed intersection point.</param>
        /// <param name="inboundIntersection">if set to <c>true</c> the intersection is inbound.</param>
        /// <returns>Returns true if the segments intersect.</returns>
          public static bool SegmentSegmentTest(Segment segment1, Segment segment2, out Vector2D intersectionPoint, out bool inboundIntersection)
        {
            //Source: Real-Time Rendering, Third Edition
            //Reference: Page 781
            intersectionPoint = new Vector2D();

            Vector2D a = segment2.Direction;
            Vector2D b = segment1.Direction;

            Vector2D c = segment1.StartPoint - segment2.StartPoint;
            Vector2D aP = Vector2D.Perp(a);
            Vector2D bP = Vector2D.Perp(b);

            double bDotaP = Vector2D.Dot(b, aP);
            inboundIntersection = bDotaP < 0;

            if (Math.Abs(bDotaP) < MathHelper.Epsilon)
                return false; // parallel test

            double d = Vector2D.Dot(c, aP);
            double f = Vector2D.Dot(a, bP);

            if (f > 0)
            {
                if (d < 0 || d > f)
                    return false;
            }
            else if (d > 0 || d < f)
                return false;


            double e = Vector2D.Dot(c, bP);
            if (f > 0)
            {
                if (e < 0 || e > f)
                    return false;
            }
            else if (e > 0 || e < f)
                return false;

            intersectionPoint = segment1.StartPoint + (d / f) * b;
            return true;
        }
        public static Vector2D SegmentSegmentIntersection(Segment segment1, Segment segment2)
        {
            Vector2D intersectionPoint;
            return SegmentSegmentTest(segment1, segment2, out intersectionPoint)
                       ? intersectionPoint
                       : new Vector2D(double.NaN);
        }


        public static bool SegmentSegmentTest(Segment segment1, Segment segment2, out Vector2D intersectionPoint)
        {
            bool inboundIntersection;
            return SegmentSegmentTest(segment1, segment2, out intersectionPoint, out inboundIntersection);
        }

        #endregion

        public static bool CirclePointTest(Circle circle, Vector2D point)
        {
            return Vector2D.DistanceSquared(circle.Center, point) < circle.Radius * circle.Radius;
        }


        public static bool PolygonHitTest(Polygon polygon, Vector2D p)
        {
            double angle = 0;
            Vector2D p1 = new Vector2D();
            Vector2D p2 = new Vector2D();

            for (int i = 0; i < polygon.Vertices.Count; i++)
            {
                p1.X = polygon.Vertices[i].X - p.X;
                p1.Y = polygon.Vertices[i].Y - p.Y;
                p2.X = polygon.Vertices[(i + 1) % polygon.Vertices.Count].X - p.X;
                p2.Y = polygon.Vertices[(i + 1) % polygon.Vertices.Count].Y - p.Y;

                angle += Angle2D(p1.X, p1.Y, p2.X, p2.Y);
            }

            if (Math.Abs(angle) < Math.PI)
                return false;
            else
                return true;
        }

        static double Angle2D(double x1, double y1, double x2, double y2)
        {
            const double TWOPI = 2 * Math.PI;
            double theta1 = Math.Atan2(y1, x1);
            double theta2 = Math.Atan2(y2, x2);
            double dtheta = theta2 - theta1;
            while (dtheta > Math.PI)
                dtheta -= TWOPI;
            while (dtheta < -Math.PI)
                dtheta += TWOPI;

            return (dtheta);
        }

        public static bool PolygonPointTest(Vertices vertices, Vector2D point)
        {
            // Crossing Test
            // Source: Real Time Rendering 3rd Edition, p. 754

            bool inside = false;
            Vector2D t = point;
            Vector2D e0 = vertices[vertices.Count - 1];
            bool y0 = e0.Y >= t.Y;

            for (int i = 0; i < vertices.Count - 1; i++)
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
    }
}
