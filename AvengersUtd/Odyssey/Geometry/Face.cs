namespace AvengersUtd.Odyssey.Geometry
{
	/// <summary>
	/// Face made from three point indexes
	/// </summary>
	public struct Face
	{
	    /// <summary>
	    /// First vertex index in triangle
	    /// </summary>
        public ushort Index1 { get; private set; }
		/// <summary>
		/// Second vertex index in triangle
		/// </summary>
        public ushort Index2 { get; private set; }
		/// <summary>
		/// Third vertex index in triangle
		/// </summary>
        public ushort Index3 { get; private set; }

	    /// <summary>
	    /// Initializes a new instance of a triangle
	    /// </summary>
	    /// <param name="index1">Vertex 1</param>
	    /// <param name="index2">Vertex 2</param>
	    /// <param name="index3">Vertex 3</param>
	    public Face(ushort index1, ushort index2, ushort index3):this()
	    {
	        Index1 = index1;
	        Index2 = index2;
	        Index3 = index3;
	    }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", Index1, Index2, Index3);
        }

	    public ushort[] Array
	    {
	        get { return new [] {Index1, Index2, Index3};}
	    }

	    public ushort[] ArrayCCW
	    {
	        get { return new [] {Index3, Index2, Index1};}
	    }
	}
}
