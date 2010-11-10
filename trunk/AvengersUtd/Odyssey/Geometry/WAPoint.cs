using System;

namespace AvengersUtd.Odyssey.Geometry
{
    public class WAPoint : IEquatable<WAPoint>,ICloneable
    {
        public Vector2D Vertex { get; set; }
        public WAPoint NextVertex { get; set; }
        public WAPoint PrevVertex { get; set; }
        public WAPoint JumpLink { get; set; }
        public int Index { get; set; }
        public bool Visited { get; set; }
        public bool IsEntryPoint { get; set; }
        public bool IsIntersection { get; set; }

        #region Equality

        public bool Equals(WAPoint other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Vertex.Equals(Vertex) && Equals(other.NextVertex, NextVertex) && Equals(other.PrevVertex, PrevVertex) && Equals(other.JumpLink, JumpLink) && other.Visited.Equals(Visited) && other.IsEntryPoint.Equals(IsEntryPoint) && other.IsIntersection.Equals(IsIntersection);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (WAPoint)) return false;
            return Equals((WAPoint) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Vertex.GetHashCode();
                result = (result*397) ^ (NextVertex != null ? NextVertex.GetHashCode() : 0);
                result = (result*397) ^ (PrevVertex != null ? PrevVertex.GetHashCode() : 0);
                result = (result*397) ^ (JumpLink != null ? JumpLink.GetHashCode() : 0);
                result = (result*397) ^ Visited.GetHashCode();
                result = (result*397) ^ IsEntryPoint.GetHashCode();
                result = (result*397) ^ IsIntersection.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(WAPoint left, WAPoint right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WAPoint left, WAPoint right)
        {
            return !Equals(left, right);
        }

        #endregion

        
        

        //public bool Between(WAPoint p1, WAPoint p2)
        //{
        //    bool lessp1 = p1.Vertex.X <= p2.Vertex.X && p1.Vertex.Y <= p2.Vertex.Y;

        //    if (lessp1)
        //    {
        //        return this.Vertex.X >= p1.Vertex.X &&
        //               this.Vertex.X <= p2.Vertex.X &&
        //               this.Vertex.Y <= p2.Vertex.Y &&
        //               this.Vertex.Y >= p1.Vertex.Y;
        //    }
        //    else
        //    {
        //        return this.Vertex.X >= p2.Vertex.X &&
        //               this.Vertex.X <= p1.Vertex.X &&
        //               this.Vertex.Y <= p1.Vertex.Y &&
        //               this.Vertex.Y >= p2.Vertex.Y;
        //    }
        //}

       
        //public override string ToString()
        //{
        //    return "{Point=(" + Vertex.X + "," + Vertex.Y + ") IsIntersect=" + IsIntersection + " IsEntryPoint=" + IsEntryPoint + " IsActive=" + Visited + "}";
        //}

        public WAPoint Clone()
        {
            return new WAPoint
                   {
                       Vertex = Vertex,
                       NextVertex = NextVertex,
                       PrevVertex = PrevVertex,
                       JumpLink = JumpLink,
                       Visited = Visited,
                       IsEntryPoint = IsEntryPoint,
                       IsIntersection = IsIntersection,
                       Index = Index
                   };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}