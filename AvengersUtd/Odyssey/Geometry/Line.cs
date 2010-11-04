using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public struct Line : IEquatable<Line>
    {
        public Vector2 Origin { get; private set; }
        public Vector2 Direction { get; private set; }
        public Vector2 Normal { get; private set; }

        public float Ax { get; private set; }
        public float By { get; private set; }
        public float C { get; private set; }

        public Line(Vector2 origin, Vector2 direction) : this()
        {
            Origin = origin;
            Direction = direction;

            Normal = Direction.Perp();
            Ax = Normal.X;
            By = Normal.Y;
            C = Vector2.Dot(-PointAtDistance(1.0f), Normal);
        }

        public Vector2 PointAtDistance(float distance)
        {
           return Origin + distance*Vector2.Normalize(Direction); 
        }

        public static int DetermineSide(Line line, Vector2 point)
        {
            return DetermineSide(line, line.Normal, point);
        }

        public static int DetermineSide(Line line, Vector2 normal, Vector2 point)
        {
            float value = Vector2.Dot(line.Normal, point) + line.C;

            if (Math.Abs(value) < MathHelper.Epsilon)
                return 0;
            else if (value < 0)
                return -1;
            else //if (value > 0)
                return 1;
        }

        public static Line FromTwoPoints(Vector2 point1, Vector2 point2)
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
