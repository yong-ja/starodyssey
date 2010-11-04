using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public static class Intersection
    {

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

        public static Vector2D LineLineIntersection(Line line1, Line line2)
        {
            // Source: Real-Time Rendering, Third Edition
            // Reference: Page 780

            Vector2D d1 = line1.Direction;
            Vector2D d2 = line2.Direction;
            Vector2D d1P = Vector2D.Perp(d1);
            Vector2D d2P = Vector2D.Perp(d2);

            double d1d2P = Vector2D.Dot(d1, d2P);

            if (d1d2P==0) 
                return new Vector2D(double.NaN); // parallel

            Vector2D o1 = line1.Origin;
            Vector2D o2 = line2.Origin;

            double s = Vector2D.Dot((o2 - o1), d2P)/d1d2P;
            return o1 + s*d1;

        }

        public static Vector2D SegmentSegmentIntersection(Segment segment1, Segment segment2)
        {
            //Source: Real-Time Rendering, Third Edition
            //Reference: Page 781

            Vector2D a = segment2.Direction;
            Vector2D b = segment1.Direction;

            Vector2D c = segment1.StartPoint - segment2.StartPoint;
            Vector2D aP = Vector2D.Perp(a);
            Vector2D bP = Vector2D.Perp(b);

            if (Math.Abs(Vector2D.Dot(b, aP)) < MathHelper.Epsilon) 
                return new Vector2D(double.NaN); // parallel test

            double d = Vector2D.Dot(c, aP);
            double f = Vector2D.Dot(a, bP);

            if (f > 0)
            {
                if (d < 0 || d > f) 
                    return new Vector2D(double.NaN);
            }
            else if (d > 0 || d < f) 
                return new Vector2D(double.NaN);
            

            double e = Vector2D.Dot(c, bP);
            if (f>0)
            {
                if (e < 0 || e> f)
                    return new Vector2D(double.NaN);
            }
            else if (e > 0 || e < f) 
                return new Vector2D(double.NaN);

            return segment1.StartPoint + (d/f) * b;

        }


    }
}
