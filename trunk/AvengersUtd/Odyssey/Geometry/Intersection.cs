using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public static class Intersection
    {
        public static bool SegmentIntersectsSegment(Segment a, Segment b)
        {
            Vector2 u = a.Direction;
            Vector2 v = b.Direction;
            
            float d = u.X*v.Y - u.Y*v.X;
            
            if (Math.Abs(d) < MathHelper.Epsilon) return false; //parallel test

            Vector2 w = a.StartPoint - b.StartPoint;

            float s = v.X*w.Y - v.Y*w.X;
            if (s < 0 || s > d) return false;

            float t = u.X*w.Y - u.Y*w.X;
            if (t < 0 || t > d) return false;

            return true;
        }

        public static Vector2 SegmentSegmentIntersection(Segment segment1, Segment segment2)
        {
            //Source: Real-Time Rendering, Third Edition
            //Reference: Page 779

            Vector2 a = segment2.Direction;
            Vector2 b = segment1.Direction;

            Vector2 c = segment1.StartPoint - segment2.StartPoint;
            Vector2 aP = a.Perp();
            Vector2 bP = b.Perp();

            if (Math.Abs(Vector2.Dot(b, aP)) < MathHelper.Epsilon) return new Vector2(float.NaN); // parallel test

            float d = Vector2.Dot(c, aP);
            float f = Vector2.Dot(a, bP);

            if (f > 0)
            {
                if (d < 0 || d > f) return new Vector2(float.NaN);
            }
            else if (d > 0 || d < f) return new Vector2(float.NaN);
            

            float e = Vector2.Dot(c, bP);
            if (f>0)
            {
                if (e < 0 || e> f) return new Vector2(float.NaN);
            }
            else if (e > 0 || e < f) return new Vector2(float.NaN);

            return segment1.StartPoint + (d/f) * b;

        }


    }
}
