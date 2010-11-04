using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public partial class TexturedPolygon
    {
        #region Quads
        public static TexturedPolygon CreateTexturedQuad(Vector3 topLeftVertex, float width, float height, bool dynamic = false)
        {
            TexturedPolygon texturedPolygon = new TexturedPolygon(topLeftVertex, width, height, dynamic);
            texturedPolygon.Init();
            return texturedPolygon;
        }

        public static TexturedVertex[] CreateTexturedQuad(Vector3 topLeftVertex, float width, float height,
                                                          out ushort[] indices)
        {
            TexturedVertex[] vertices = new[]
                                            {
                                                new TexturedVertex(
                                                    new Vector4(topLeftVertex.X, topLeftVertex.Y - height,
                                                                topLeftVertex.Z, 1.0f),
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
            indices = new ushort[]
                          {
                              2, 1, 0,
                              2, 0, 3
                          };

            return vertices;
        } 
        #endregion
    }
}
