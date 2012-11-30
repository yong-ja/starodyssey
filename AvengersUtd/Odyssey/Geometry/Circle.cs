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

        public static Circle CircumCircle(Vector2D a, Vector2D b, Vector2D c)
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

        public static Vector2D PointOnCircle(Circle circle, double theta)
        {
            Vector2D center = circle.Center;
            double x = center.X;
            double y = center.Y;

            return new Vector2D(x + Math.Cos(theta) * circle.Radius,
                y - Math.Sin(theta) * circle.Radius);

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
