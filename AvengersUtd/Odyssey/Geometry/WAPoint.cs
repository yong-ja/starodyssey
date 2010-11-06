namespace AvengersUtd.Odyssey.Geometry
{
    public class WAPoint
    {
        public Vector2D Vertex { get; set; }
        public WAPoint NextVertex { get; set; }
        public WAPoint PrevVertex { get; set; }
        public WAPoint JumpLink { get; set; }

        public bool IsActive { get; set; }
        public bool IsEntryPoint { get; set; }
        public bool IsIntersection { get; set; }

        public bool Between(WAPoint p1, WAPoint p2)
        {
            bool lessp1 = p1.Vertex.X <= p2.Vertex.X && p1.Vertex.Y <= p2.Vertex.Y;

            if (lessp1)
            {
                return this.Vertex.X >= p1.Vertex.X &&
                       this.Vertex.X <= p2.Vertex.X &&
                       this.Vertex.Y <= p2.Vertex.Y &&
                       this.Vertex.Y >= p1.Vertex.Y;
            }
            else
            {
                return this.Vertex.X >= p2.Vertex.X &&
                       this.Vertex.X <= p1.Vertex.X &&
                       this.Vertex.Y <= p1.Vertex.Y &&
                       this.Vertex.Y >= p2.Vertex.Y;
            }
        }

        public override bool Equals(object obj)
        {
            WAPoint compare = (WAPoint)obj;
            return Vertex.X == compare.Vertex.X && Vertex.Y == compare.Vertex.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "{Point=(" + Vertex.X + "," + Vertex.Y + ") IsIntersect=" + IsIntersection + " IsEntryPoint=" + IsEntryPoint + " IsActive=" + IsActive + "}";
        }
    }
}