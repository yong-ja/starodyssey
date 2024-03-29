﻿using System;
using System.Collections.Generic;
using System.Drawing;
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

            double s = Vector2D.Dot((o2 - o1), d2P)/d1d2P;
            return o1 + s*d1;
        }

        public static bool LineLineTest(Line line1, Line line2, out Vector2D intersection)
        {
            intersection = LineLineIntersection(line1, line2);
            return !Double.IsNaN(intersection.X);
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
        public static bool SegmentSegmentTest(Segment segment1, Segment segment2, out Vector2D intersectionPoint,
                                              out bool inboundIntersection)
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

            intersectionPoint = segment1.StartPoint + (d/f)*b;
            return true;
        }

        public static bool SegmentSegmentTest(Segment segment1, Segment segment2, out Vector2D intersectionPoint)
        {
            bool inboundIntersection;
            return SegmentSegmentTest(segment1, segment2, out intersectionPoint, out inboundIntersection);
        }

        #endregion

        #region Circle - X intersection methods

        public static bool CirclePointTest(Vector2 center, float radius, Vector2 cursorLocation)
        {
            return Vector2.DistanceSquared(center, cursorLocation) < (radius*radius);
        }

        public static bool CirclePointTest(Circle circle, Vector2D point)
        {
            return Vector2D.DistanceSquared(circle.Center, point) < circle.Radius*circle.Radius;
        }


        /// <summary>
        /// Determines whether a circle and a rectangle intersect.
        /// </summary>
        /// <param name="circle">The circle.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>True if the circle and rectangle intersect or collide.</returns>
        public static bool CircleRectangleTest(Circle circle, OrthoRectangle rectangle)
        {
            Vector2D circleDistance = new Vector2D(Math.Abs(circle.Center.X - rectangle.X - rectangle.Width / 2),
                                                   Math.Abs(circle.Center.Y - rectangle.Y - rectangle.Height / 2));

            if (circleDistance.X > (rectangle.Width / 2 + circle.Radius))
                return false;
            if (circleDistance.Y > (rectangle.Height / 2 + circle.Radius))
                return false;

            if (circleDistance.X <= (rectangle.Width / 2))
                return true;
            if (circleDistance.Y <= (rectangle.Height / 2))
                return true;

            double cornerDistanceSquared = Math.Pow(circleDistance.X - rectangle.Width / 2, 2) +
                                           Math.Pow(circleDistance.Y - rectangle.Height / 2, 2);

            return (cornerDistanceSquared <= (Math.Pow(circle.Radius, 2)));

        }

        #endregion

        /// <summary>
        /// Determines if an ellipse and a rectangle intersect.
        /// </summary>
        /// <param name="ellipse">The ellipse.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns></returns>
        public static bool EllipseRectangleTest(Ellipse ellipse, OrthoRectangle rectangle)
        {
            bool testXpos = (ellipse.Center.X + ellipse.RadiusX) > rectangle.Right;
            bool testXneg = (ellipse.Center.X - ellipse.RadiusX) < rectangle.Left;
            bool testYpos = (ellipse.Center.Y + ellipse.RadiusY) > rectangle.Top;
            bool testYneg = (ellipse.Center.Y - ellipse.RadiusY) < rectangle.Bottom;

            return testXpos || testXneg || testYpos || testYneg;
        }

        public static bool EllipsePointTest(Ellipse ellipse, Vector2D point)
        {
            double x = point.X;
            double y = point.Y;
            double a = ellipse.RadiusX;
            double b = ellipse.RadiusY;

            double xComponent = (Math.Pow(point.X - ellipse.Center.X, 2) / Math.Pow(ellipse.RadiusX, 2));
            double yComponent = (Math.Pow(point.Y - ellipse.Center.Y, 2) / Math.Pow(ellipse.RadiusY, 2));

            double value = xComponent + yComponent;

            if (value < 1)
                return true;

            return false; 
        }

        public static bool EllipseContainsRectangle(Ellipse ellipse, OrthoRectangle rectangle)
        {
            Vector2D[] vertices = rectangle.VerticesArray;
            return vertices.All(v => EllipsePointTest(ellipse, v));
        }

        public static bool RectangleContainsEllipse(OrthoRectangle rectangle, Ellipse ellipse)
        {
            bool testXpos = (ellipse.Center.X + ellipse.RadiusX) < rectangle.Right;
            bool testXneg = (ellipse.Center.X - ellipse.RadiusX) > rectangle.Left;
            bool testYpos = (ellipse.Center.Y + ellipse.RadiusY) < rectangle.Top;
            bool testYneg = (ellipse.Center.Y - ellipse.RadiusY) > rectangle.Bottom;

            return testXpos && testXneg && testYpos && testYneg;
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
                p2.X = polygon.Vertices[(i + 1)%polygon.Vertices.Count].X - p.X;
                p2.Y = polygon.Vertices[(i + 1)%polygon.Vertices.Count].Y - p.Y;

                angle += Angle2D(p1.X, p1.Y, p2.X, p2.Y);
            }

            if (Math.Abs(angle) < Math.PI)
                return false;
            else
                return true;
        }

        private static double Angle2D(double x1, double y1, double x2, double y2)
        {
            const double TWOPI = 2*Math.PI;
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
                {
                    if (((e1.Y - t.Y)*(e0.X - e1.X) >= (e1.X - t.X)*(e0.Y - e1.Y)) == y1)
                        inside = !inside;
                }
                y0 = y1;
                e0 = e1;
            }
            return inside;
        }

        public static bool RectangleTest(Vector2 position, Size size, Vector2 cursorLocation)
        {
            float xEvent = cursorLocation.X;
            float yEvent = cursorLocation.Y;
            float xPos = position.X;
            float yPos = position.Y;

            return (xEvent >= xPos && xEvent <= xPos + size.Width) &&
                   (yEvent >= yPos && yEvent <= yPos + size.Height);
        }


        /*
         * Ray-box intersection using IEEE numerical properties to ensure that the
         * test is both robust and efficient, as described in:
         *
         *      Amy Williams, Steve Barrus, R. Keith Morley, and Peter Shirley
         *      "An Efficient and Robust Ray-Box Intersection Algorithm"
         *      Journal of graphics tools, 10(1):49-54, 2005
         *
         */
        public static bool RayAABBTest(Ray r, IBox box)
        {
            const float t0 = float.NegativeInfinity;
            const float t1 = float.PositiveInfinity;
            Vector3[] bounds = new[] {box.Min, box.Max};
            Vector3 rInvDir = new Vector3(1 / r.Direction.X, 1 / r.Direction.Y, 1 / r.Direction.Z);
            int rSignX = rInvDir.X < 0 ? 1 : 0;
            int rSignY = rInvDir.Y < 0 ? 1 : 0;
            int rSignZ = rInvDir.Z < 0 ? 1 : 0;

            float tMin = (bounds[rSignX].X - r.Position.X) * rInvDir.X;
            float tMax = (bounds[1 - rSignX].X - r.Position.X) * rInvDir.X;
            float tyMin = (bounds[rSignY].Y - r.Position.Y) * rInvDir.Y;
            float tyMax = (bounds[1 - rSignY].Y - r.Position.Y) * rInvDir.Y;

            if ((tMin > tyMax) || (tyMin > tMax))
                return false;
            if (tyMin > tMin)
                tMin = tyMin;
            if (tyMax < tMax)
                tMax = tyMax;

            float tzMin = (bounds[rSignZ].Z - r.Position.Z)*rInvDir.Z;
            float tzMax = (bounds[1 - rSignZ].Z - r.Position.Z)*rInvDir.Z;

            if ((tMin > tzMax) || (tzMin > tMax))
                return false;
            if (tzMin > tMin)
                tMin = tzMin;
            if (tzMax < tMax)
                tMax = tzMax;
            return ((tMin < t1) && (tMax > t0));

        }

        public static bool RaySphereTest(Ray r, ISphere s)
        {
            float det, b;
            Vector3 p = r.Position - s.AbsolutePosition;
            Vector3 d = Vector3.Normalize(r.Direction);
            b = -Vector3.Dot(p, d);
            det = b * b - Vector3.Dot(p,p) + s.Radius * s.Radius;
            if (det < 0)
            {
                return false;
            }
            det = (float)Math.Sqrt(det);
            float i1 = b - det;
            float i2 = b + det;
            // intersecting with ray?
            if (i2 < 0) return false;
            if (i1 < 0) i1 = 0;
            return true;
        }


    
    }
}