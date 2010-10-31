using AvengersUtd.Odyssey.Geometry;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public struct VertexDescription
    {
        public VertexFormat Format { get; private set; }
        public int Stride { get; private set; }

        public VertexDescription(VertexFormat format, int stride) : this()
        {
            Format = format;
            Stride = stride;
        }
    }
}
