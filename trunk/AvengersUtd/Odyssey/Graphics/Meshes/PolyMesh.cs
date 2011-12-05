using System;
using System.Collections.Generic;
using System.Linq;
using AvengersUtd.Odyssey.Geometry;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public partial class PolyMesh : BaseMesh<ColoredVertex>
    {

        public PolyMesh(Vector3 topLeftVertex, float width, float height, Color4[] Color4s) : base(ColoredVertex.Description)
        {
            ushort[] indices;
            Vertices = CreateQuad(topLeftVertex.ToVector4(), width, height, Color4s, out indices);
            Indices = indices;
           
        }

       public void UpdateVertices(ColoredVertex[] vertices)
        {
            DataBox db = Game.Context.Device.ImmediateContext.MapSubresource(VertexBuffer, 0,
                                                                                 //VertexBuffer.Description.SizeInBytes,
                                                                                 MapMode.WriteDiscard, MapFlags.None);
            foreach (ColoredVertex vertex in vertices)
            {
                db.Data.Write(vertex.Position);
                db.Data.Write(vertex.Color);
            }

            db.Data.Position = 0;
            Game.Context.Device.ImmediateContext.UnmapSubresource(VertexBuffer, 0);
        }

        public static ColoredVertex[] WeldVertices2D(IEnumerable<ColoredVertex> vertices, int digits)
        {
            Dictionary<int, ColoredVertex> hashMap = new Dictionary<int, ColoredVertex>();

            foreach (ColoredVertex v in vertices)
            {
                ColoredVertex v1 = new ColoredVertex(new Vector4((float) Math.Round(v.Position.X, digits), (float) Math.Round(v.Position.Y, digits),
                                v.Position.Z, v.Position.W), v.Color);
                int hash = v1.Position.GetHashCode();
                if (!hashMap.Keys.Contains(hash))
                    hashMap.Add(hash, v1);
                else
                    hashMap[hash] = v1;
            }

            return hashMap.Values.ToArray();
        }
    }
}
