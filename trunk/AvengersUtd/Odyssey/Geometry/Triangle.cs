namespace AvengersUtd.Odyssey.Geometry
{
	/// <summary>
	/// Triangle made from three point indexes
	/// </summary>
	public struct Triangle
	{
	    /// <summary>
	    /// First vertex index in triangle
	    /// </summary>
        public ushort Point1 { get; private set; }
		/// <summary>
		/// Second vertex index in triangle
		/// </summary>
        public ushort Point2 { get; private set; }
		/// <summary>
		/// Third vertex index in triangle
		/// </summary>
        public ushort Point3 { get; private set; }

	    /// <summary>
	    /// Initializes a new instance of a triangle
	    /// </summary>
	    /// <param name="point1">Vertex 1</param>
	    /// <param name="point2">Vertex 2</param>
	    /// <param name="point3">Vertex 3</param>
	    public Triangle(ushort point1, ushort point2, ushort point3):this()
	    {
	        Point1 = point1;
	        Point2 = point2;
	        Point3 = point3;
	    }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", Point1, Point2, Point3);
        }

	    public ushort[] Array
	    {
	        get { return new [] {Point1, Point2, Point3};}
	    }

	    public ushort[] ArrayCCW
	    {
	        get { return new [] {Point3, Point2, Point1};}
	    }

        public ushort[] ArrayCCW2
        {
            get { return new[] { Point3, Point1, Point2 }; }
        }

	}
}
