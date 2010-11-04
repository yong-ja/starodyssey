using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public struct Circle : IEquatable<Circle>
    {
        public Vector2D Center;
        public double Radius;

        public Circle(Vector2D center, double radius)
        {
            Center = center;
            Radius = radius;
        }
        
        public bool IsInside(Vector2D point)
        {
            return Intersection.CirclePointTest(this, point);
        }

        public static Circle CircumCircle2(Vector2D point1, Vector2D point2, Vector2D point3)
        {
            Vector2D center = new Vector2D();
            Vector2D r = point2 - point1;
            Vector2D s = point3 - point1;

            double cp = Vector2D.Cross(r, s);

            if (Math.Abs(cp) > MathHelper.EpsilonD)
            {
                double p1Sq, p2Sq, p3Sq;
                double num, den;
                double cx, cy;

                p1Sq = point1.X * point1.X + point1.Y * point1.Y;
                p2Sq = point2.X * point2.X + point2.Y * point2.Y;
                p3Sq = point3.X * point3.X + point3.Y * point3.Y;
                num = p1Sq*(point2.Y - point3.Y) + p2Sq*(point3.Y - point1.Y)
                      + p3Sq*(point1.Y - point2.Y);
                cx = num/(2.0*cp);
                num = p1Sq*(point3.X - point2.X) + p2Sq*(point1.X - point3.X)
                      + p3Sq*(point2.X - point1.X);
                cy = num/(2.0*cp);

                center = new Vector2D(cx, cy);
            }

            // Radius 
            double radius = Vector2D.Distance(center, point1);

            
            return new Circle(center, radius);
        }

        public static Circle CircumCircle3(Vector2D a, Vector2D b, Vector2D c)
        {
            double x1 = a.X;
            double x2 = b.X;
            double x3 = c.X;
            double y1 = a.Y;
            double y2 = b.Y;
            double y3 = c.Y;
            const double e = MathHelper.EpsilonD;

            if (((Math.Abs(x1 - x2) < e) && (Math.Abs(y1 - y2) < e))
               || ((Math.Abs(x1 - x3) < e) && (Math.Abs(y1 - y3) < e))
               || ((Math.Abs(x2 - x3) < e) && (Math.Abs(y2 - y3) < e)))
                return default(Circle); // Test if two points coincide

            if (((Math.Abs(x1 - x2) < e) && (Math.Abs(x1 - x3) < e))
               || ((Math.Abs(y1 - y2) < e) && (Math.Abs(y2 - y3) < e)))
                return default(Circle); // Test if they're on hor/ver line

            if (Math.Abs(((y2 - y1) / (x2 - x1)) - ((y3 - y2) / (x3 - x2))) < e)
                return default(Circle); // Test if they're colinear

            // lines from a to b and a to c
            Vector2D AB = b - a;
            Vector2D AC = c - a;

            // perpendicular vector on triangle
            //Vector2D N = Vector2D.Cross(AB, AC));

            // find the points halfway on AB and AC
            Vector2D halfAB = a + AB*0.5;
            Vector2D halfAC = a + AC * 0.5;

            // build vectors perpendicular to ab and ac
            Vector2D perpAB = Vector2D.Perp(AB);
            Vector2D perpAC = Vector2D.Perp(AC);

            // find intersection between the two lines
            // D: halfAB + t*perpAB
            // E: halfAC + s*perpAC
            Vector2D center = Intersection.LineLineIntersection(new Line(halfAB, perpAB), new Line(halfAC, perpAC));
            // the radius is the distance between center and any point
            // distance(A, B) = length(A-B)
            double radius = Vector2D.Distance(center, a);

            return new Circle(center, radius);
        }

        public static Circle CircumCircle(Vector2D a, Vector2D b, Vector2D c)
        {
            double x1 = a.X;
            double x2 = b.X;
            double x3 = c.X;
            double y1 = a.Y;
            double y2 = b.Y;
            double y3 = c.Y;
            const double e = MathHelper.EpsilonD;

            if(((Math.Abs(x1 - x2) < e) && (Math.Abs(y1 - y2) < e))
               || ((Math.Abs(x1 - x3) < e) && (Math.Abs(y1 - y3) < e))
               || ((Math.Abs(x2 - x3) < e) && (Math.Abs(y2 - y3) < e)))
                return default(Circle); // Test if two points coincide

            if(((Math.Abs(x1 - x2) < e) && (Math.Abs(x1 - x3) < e))
               || ((Math.Abs(y1 - y2) < e) && (Math.Abs(y2 - y3) < e)))
                return default(Circle); // Test if they're on hor/ver line

            if(Math.Abs(((y2 - y1)/(x2 - x1)) - ((y3 - y2)/(x3 - x2))) < e)
                return default(Circle); // Test if they're colinear

            double r = (a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y));
            double cx = (a.LengthSquared() * (b.Y - c.Y) + b.LengthSquared() * (c.Y - a.Y)
                  + c.LengthSquared() * (a.Y - b.Y)) / (2 * r);

            double cy = (a.LengthSquared() * (c.X - b.X) + b.LengthSquared() * (a.X - c.X)
                  + c.LengthSquared() * (b.X - a.X)) / (2 * r);

            return new Circle(new Vector2D(cx,cy), Math.Abs(r/2));

            //double m1, b1, m2, b2;

            //m1 = (x1 - x2)/(y2 - y1); // Negative reciprocal

            //b1 = (y1 + y2)/2 - m1*(x1 + x2)/2; // Point-slope on the midpoint of AB

            //m2 = (x2 - x3)/(y3 - y2);
            //b2 = (y2 + y3)/2 - m2*(x2 + x3)/2;

            //double ix, iy;

            //ix = (b2 - b1)/(m1 - m2);
            //iy = m1*ix + b1;

            //Vector2D center = new Vector2D(ix,iy);
            //double radius = Vector2D.Distance(center, a);

            //return new Circle(center,radius);


        }

        #region Equality
        public bool Equals(Circle other)
        {
            return other.Center.Equals(Center) && other.Radius.Equals(Radius);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Circle)) return false;
            return Equals((Circle)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Center.GetHashCode() * 397) ^ Radius.GetHashCode();
            }
        }

        public static bool operator ==(Circle left, Circle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Circle left, Circle right)
        {
            return !left.Equals(right);
        } 
        #endregion
    }
}
