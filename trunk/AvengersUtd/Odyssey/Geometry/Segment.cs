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
        #region Properties
        public Vector2 StartPoint { get; set; }
        public Vector2 EndPoint { get; set; }

        /// <summary>
        /// Returns the vector going from the start point to the end point.
        /// </summary>
        public Vector2 Direction
        {
            get { return EndPoint - StartPoint; }
        }

        /// <summary>
        /// Returns a vector perpendicular to the direction of the segment.
        /// </summary>
        public Vector2 Normal
        {
            get
            {
                Vector2 dir = Direction;
                return dir.Perp();
            }
        } 
        #endregion

        public Segment(Vector2 startPoint, Vector2 endPoint) : this()
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        #region Public Methods
        public bool Intersects(Segment segment)
        {
            return Intersection.SegmentSegmentTest(this, segment);
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
        #endregion

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

        public override string ToString()
        {
            return string.Format
                (CultureInfo.InvariantCulture, "P1({0:f2},{1:f2}) -> P2({2:f2},{3:f2})", StartPoint.X, StartPoint.Y, EndPoint.X, EndPoint.Y);
        }

        #region Static Methods
        public static Segment Invert(Segment segment)
        {
            return new Segment(segment.EndPoint, segment.StartPoint);
        }


        /// <summary>
        /// Determines which side the given point is on.
        /// </summary>
        /// <param name="segment">The segment to test.</param>
        /// <param name="point">The point to test.</param>
        /// <returns>It returns 0 if the point belongs to the segment.
        /// It returns 1 if it is on the right side.
        /// It returns -1 if it is on the left side.</returns>
        public static int DetermineSide(Segment segment, Vector2 point)
        {
            float c = Vector2.Dot(-segment.EndPoint, segment.Normal);
            float fp = Vector2.Dot(segment.Normal, point) + c;
            if (fp > 0) return -1; // Left side
            if (fp < 0) return 1; // Right side
            return 0; // p belongs to segment
        } 

        public static Vector2 PointAtDistance(Segment segment, float distance)
        {
            //C = A + k(B - A
            return segment.StartPoint + distance*segment.Direction;
        }

        public static Vector2 PointAtDistance(Vector2 startPoint, Vector2 endPoint, float distance)
        {
            return PointAtDistance(new Segment(startPoint, endPoint), distance);
        }


        #endregion


    }
}
