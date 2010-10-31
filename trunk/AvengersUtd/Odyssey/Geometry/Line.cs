using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public struct Line : IEquatable<Line>
    {
        public Vector2 Origin { get; set; }
        public Vector2 Direction { get; set; }

        public Line(Vector2 origin, Vector2 direction) : this()
        {
            Origin = origin;
            Direction = direction;
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
