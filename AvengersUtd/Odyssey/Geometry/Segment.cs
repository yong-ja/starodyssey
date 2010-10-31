using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    /// <summary>
    /// Represents a two dimensional segment between two points.
    /// </summary>
    public struct Segment : IEquatable<Segment>
    {
        public Vector2 StartPoint { get; set; }
        public Vector2 EndPoint { get; set; }

        /// <summary>
        /// Returns the vector going from the start point to the end point.
        /// </summary>
        public Vector2 Direction
        {
            get { return EndPoint - StartPoint; }
        }

        public Vector2 Normal
        {
            get
            {
                Vector2 dir = Direction;
                return dir.Perp();
            }
        }

        public Segment(Vector2 startPoint, Vector2 endPoint) : this()
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public bool Intersects(Segment segment)
        {
            return Intersection.SegmentIntersectsSegment(this, segment);
        }


        /// <summary>
        /// Inverts the start point with the end point and vice versa.
        /// </summary>
        public void Invert()
        {
            Vector2 temp = StartPoint;
            StartPoint = EndPoint;
            EndPoint = temp;
        }

        #region Equality
        public bool Equals(Segment other)
        {
            return other.StartPoint.Equals(StartPoint) && other.EndPoint.Equals(EndPoint);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Segment)) return false;
            return Equals((Segment)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StartPoint.GetHashCode() * 397) ^ EndPoint.GetHashCode();
            }
        }

        public static bool operator ==(Segment left, Segment right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Segment left, Segment right)
        {
            return !left.Equals(right);
        } 
        #endregion


        public static Segment Invert(Segment segment)
        {
            return new Segment(segment.EndPoint, segment.StartPoint);
        }

        public static int DetermineSide(Segment segment, Vector2 point)
        {
            float c = Vector2.Dot(-segment.EndPoint, segment.Normal);
            float fp = Vector2.Dot(segment.Normal, point) + c;
            if (fp > 0) return -1; // Left side
            if (fp < 0) return 1; // Right side
            return 0; // p belongs to segment
        }

        public override string ToString()
        {
            return string.Format
                (CultureInfo.InvariantCulture,"P1({0:f2},{1:f2}) -> P2({2:f2},{3:f2})", StartPoint.X, StartPoint.Y, EndPoint.X, EndPoint.Y);
        }
    }
}
