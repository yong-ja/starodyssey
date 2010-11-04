using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public struct Line : IEquatable<Line>
    {
        public Vector2D Origin;
        public Vector2D Direction;
        public Vector2D Normal;

        public double Ax;
        public double By;
        public double C;

        public Line(Vector2D origin, Vector2D direction) : this()
        {
            Origin = origin;
            Direction = direction;

            Normal = Vector2D.Perp(Direction);
            Ax = Normal.X;
            By = Normal.Y;
            C = Vector2D.Dot(-PointAtDistance(1.0), Normal);
        }

        public Vector2D PointAtDistance(double distance)
        {
           return Origin + distance*Vector2D.Normalize(Direction); 
        }

        public static int DetermineSide(Line line, Vector2D point)
        {
            return DetermineSide(line, line.Normal, point);
        }

        public static int DetermineSide(Line line, Vector2D normal, Vector2D point)
        {
            double value = Vector2D.Dot(line.Normal, point) + line.C;

            if (Math.Abs(value) < MathHelper.Epsilon)
                return 0;
            else if (value < 0)
                return -1;
            else //if (value > 0)
                return 1;
        }

        public static Line FromTwoPoints(Vector2D point1, Vector2D point2)
        {
            return new Line(point1, point2-point1);
        }

        public static explicit operator Line(Segment segment)
        {
            return FromTwoPoints(segment.StartPoint, segment.EndPoint);
        }

        #region Equality
        public bool Equals(Line other)
        {
            return other.Origin.Equals(Origin) && other.Direction.Equals(Direction);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Line)) return false;
            return Equals((Line)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Origin.GetHashCode() * 397) ^ Direction.GetHashCode();
            }
        }

        public static bool operator ==(Line left, Line right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Line left, Line right)
        {
            return !left.Equals(right);
        } 
        #endregion
    }
}
