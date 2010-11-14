using System;

namespace AvengersUtd.Odyssey.Geometry
{
	/// <summary>
	/// Edge made from two point indexes and two face references.
	/// Required for delaunay triangulation
	/// </summary>
	internal struct Edge : IEquatable<Edge>
	{
	    /// <summary>
	    /// Start of edge index
	    /// </summary>
	    public ushort Start;

		/// <summary>
		/// End of edge index
		/// </summary>
        public ushort End;

        /// <summary>
		/// Initializes a new edge instance
		/// </summary>
        /// <param name="startPoint">Start edge vertex index</param>
        /// <param name="endPoint">End edge vertex index</param>
        public Edge(ushort startPoint, ushort endPoint)
		{
            Start = startPoint; End = endPoint;
		}
		

		#region IEquatable<dEdge> Members

		/// <summary>
		/// Checks whether two edges are equal disregarding the direction of the edges
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
	    public bool Equals(Edge other)
	    {
	        if (ReferenceEquals(null, other)) return false;
	        if (ReferenceEquals(this, other)) return true;
	        return other.Start == Start && other.End == End;
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != typeof (Edge)) return false;
	        return Equals((Edge) obj);
	    }

	    public override int GetHashCode()
	    {
	        unchecked
	        {
	            return (Start*397) ^ End;
	        }
	    }

	    public static bool operator ==(Edge left, Edge right)
	    {
	        return Equals(left, right);
	    }

	    public static bool operator !=(Edge left, Edge right)
	    {
	        return !Equals(left, right);
	    }

	    #endregion
	}
}