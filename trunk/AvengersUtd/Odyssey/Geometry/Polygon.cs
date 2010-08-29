﻿using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Geometry
{
    public partial class Polygon : BaseMesh<ColoredVertex>
    {

        public Polygon(Vector3 topLeftVertex, float width, float height, Color4[] Color4s) : base(ColoredVertex.Description)
        {
            short[] indices;
            Vertices = Polygon.CreateQuad(topLeftVertex.ToVector4(), width, height, Color4s, out indices);
            Indices = indices;
           
        }

       public void UpdateVertices(ColoredVertex[] vertices)
        {
            DataBox db = Game.Context.Device.ImmediateContext.MapSubresource(VertexBuffer, 0,
                                                                                 VertexBuffer.Description.SizeInBytes,
                                                                                 MapMode.WriteDiscard, MapFlags.None);
            foreach (ColoredVertex vertex in vertices)
            {
                db.Data.Write(vertex.Position);
                db.Data.Write(vertex.Color);
            }

            db.Data.Position = 0;
            Game.Context.Device.ImmediateContext.UnmapSubresource(VertexBuffer, 0);
        }

        
    }
}
