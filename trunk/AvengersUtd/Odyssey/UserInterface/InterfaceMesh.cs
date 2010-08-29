using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.UserInterface
{
    public class InterfaceMesh : BaseMesh<ColoredVertex>
    {
        public Hud OwnerHud { get; set; }

        public InterfaceMesh(ColoredVertex[] vertices, short[] indices) : base(ColoredVertex.Description)
        {
            Vertices = vertices;
            Indices = indices;
            CpuAccessFlags = CpuAccessFlags.Write;
            ResourceUsage = ResourceUsage.Dynamic;
            VertexDescription = ColoredVertex.Description;
        }

        public void UpdateBuffers(ColoredVertex[] vertices, short[] indices)
        {
            DataBox dbVertices = Game.Context.Device.ImmediateContext.MapSubresource(VertexBuffer, 0,
                                                                                 VertexBuffer.Description.SizeInBytes,
                                                                                 MapMode.WriteDiscard, MapFlags.None);
            dbVertices.Data.WriteRange(vertices);
            dbVertices.Data.Position = 0;
            Game.Context.Device.ImmediateContext.UnmapSubresource(VertexBuffer, 0);

            DataBox dbIndices = Game.Context.Device.ImmediateContext.MapSubresource(IndexBuffer, 0,
                                                                                 IndexBuffer.Description.SizeInBytes,
                                                                                 MapMode.WriteDiscard, MapFlags.None);
            dbIndices.Data.WriteRange(indices);
            dbIndices.Data.Position = 0;

            Game.Context.Device.ImmediateContext.UnmapSubresource(IndexBuffer, 0);

            Vertices = vertices;
            Indices = indices;
        }

    }
}
