using System;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Buffer = SlimDX.Direct3D11.Buffer;
using AvengersUtd.Odyssey.Graphics.Materials;
using Device = SlimDX.Direct3D11.Device;

namespace AvengersUtd.Odyssey.Geometry
{
    public class Polygon : BaseMesh
    {

        public Polygon(Buffer vertices, Buffer indices, int vertexCount, VertexDescription vertexDescription)
            : base(vertices, indices, vertexCount, vertexDescription)
        {
        }

        public override void Render()
        {
            //RenderForm11.Device.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(Vertices, VertexDescription.Stride, 0));
            //RenderForm11.Device.ImmediateContext.InputAssembler.SetIndexBuffer(Indices, Format.R16_UInt, 0);
            RenderForm11.Device.ImmediateContext.DrawIndexed(IndexCount, 0, 0);
        }

        #region Create methods
        public static TexturedPolygon CreateTexturedQuad(Vector4 topLeftVertex, float width, float height)
        {
            int numPrimitives = 2;
            int indexCount = 6;

            TexturedVertex[] vertices = new TexturedVertex[]
                                            {
                                                new TexturedVertex(
                                                    new Vector4(topLeftVertex.X, topLeftVertex.Y-height, topLeftVertex.Z, 1.0f),
                                                    new Vector2(0.0f, 1.0f)),
                                                new TexturedVertex(
                                                    new Vector4(topLeftVertex.X, topLeftVertex.Y,
                                                                topLeftVertex.Z, 1.0f), new Vector2(0.0f, 0.0f)),
                                                new TexturedVertex(
                                                    new Vector4(topLeftVertex.X + width, topLeftVertex.Y,
                                                                topLeftVertex.Z, 1.0f), new Vector2(1.0f, 0.0f)),
                                                new TexturedVertex(
                                                    new Vector4(topLeftVertex.X + width, topLeftVertex.Y - height,
                                                                topLeftVertex.Z, 1.0f), new Vector2(1.0f, 1.0f))
                                            };

           
            DataStream stream = new DataStream(4 * TexturedVertex.Stride, true, true);
            foreach (TexturedVertex vertex in vertices)
            {
                stream.Write(vertex.Position);
                stream.Write(vertex.TextureCoordinate);
            }

            stream.Position = 0;

            Buffer vertexBuffer = new Buffer(RenderForm11.Device, stream, new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = 4 * TexturedVertex.Stride,
                Usage = ResourceUsage.Default
            });

            stream.Close();

            stream = new DataStream(3 * numPrimitives * sizeof(short), true, true);
            stream.WriteRange(new short[]
                                  {
                                      0, 1, 2,
                                      3, 0, 2
                                  });
            stream.Position = 0;
            Buffer indices = new Buffer(RenderForm11.Device, stream, new BufferDescription()
            {
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = (int)stream.Length
            });
            stream.Dispose();

            TexturedPolygon quad = new TexturedPolygon(vertexBuffer, indices, indexCount);
            return quad;

        }

        public static Polygon CreateTriangle(Vector4 topVertex, float sideLength, float heightOffset)
        {
            int stride = 32;
            int vertexCount = 3;
            DataStream stream = new DataStream(vertexCount * stride, true, true);

            stream.WriteRange(new[] {
                new Vector4(topVertex.X, topVertex.Y, topVertex.Z, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                new Vector4(topVertex.X + sideLength, topVertex.Y - heightOffset, topVertex.Z, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                new Vector4(topVertex.X - sideLength, topVertex.Y - heightOffset, topVertex.Z, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
            });

            stream.Position = 0;

            Buffer vertices = new Buffer(RenderForm11.Device, stream, new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = vertexCount * stride,
                Usage = ResourceUsage.Default
            });

            stream.Close();

            stream = new DataStream(vertexCount * sizeof(short), true, true);
            stream.WriteRange(new short[]
                                  {
                                      0, 1, 2
                                  });
            stream.Position = 0;
            Buffer indices = new Buffer(RenderForm11.Device, stream, new BufferDescription()
                                                                         {
                                                                             BindFlags = BindFlags.IndexBuffer,
                                                                             CpuAccessFlags = CpuAccessFlags.None,
                                                                             OptionFlags = ResourceOptionFlags.None,
                                                                             SizeInBytes = (int)stream.Length
                                                                         });
            stream.Dispose();

            // attenzione
            Polygon triangle = new Polygon(vertices, indices, vertexCount, TexturedVertex.Description)
                                   {
                                       PositionV4 = topVertex
                                   };
            return triangle;
        } 
        #endregion

        



    }
}
