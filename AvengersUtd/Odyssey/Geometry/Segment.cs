using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    /// <summary>
    /// Represents a two dimensional segment between two points.
    /// </summary>
    public struct Segment : IEquatable<Segment>
    {
        public PointF P1 { get; set; }
        public PointF P2 { get; set; }

        public Segment(PointF p1, PointF p2) : this()
        {
            P1 = p1;
            P2 = p2;
        }

        #region Equality
        public bool Equals(Segment other)
        {
            return other.P1.Equals(P1) && other.P2.Equals(P2);
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
                return (P1.GetHashCode() * 397) ^ P2.GetHashCode();
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
    }
}
