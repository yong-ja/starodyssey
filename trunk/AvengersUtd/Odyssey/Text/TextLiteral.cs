using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Graphics.Resources;
using SlimDX;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Settings;

namespace AvengersUtd.Odyssey.Text
{
    public class TextLiteral : IEntity
    {
        private Vector3 screenPosition;
        private TexturedPolygon quad;
        private string text;
        private Color4 color;
       
        public string Content
        {
            get { return text; }
            set
            {
                if (text!=value)
                {
                    text = value;
                    Create();
                }
            }
        }

        public TextLiteral(string text, Vector3 screenPosition)
        {
            this.text = text;
            this.screenPosition = screenPosition;

            if (string.IsNullOrEmpty(text)) 
                throw new ArgumentNullException("TextLiteral content cannot be an empty string.");
            
            Create();
        }

        public void Create2(Vector3 topLeftVertex, int width, int height, DataStream stream)
        {
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


            //stream = new DataStream(4 * TexturedVertex.Stride, true, true);
            foreach (TexturedVertex vertex in vertices)
            {
                stream.Write(vertex.Position);
                stream.Write(vertex.TextureCoordinate);
            }

            stream.Position = 0;
        }
        
        void Create()
        {

            float newX = ((VideoSettings.ScreenWidth / 2f) * -1f) + screenPosition.X;
            float newY = (VideoSettings.ScreenHeight / 2f) - screenPosition.Y;

            if (!ResourceManager.Contains(text))
                ResourceManager.Add(text, TextManager.DrawText(text));

            Texture2D texture = (Texture2D)ResourceManager.GetResource(text).Resource;
            Texture2DDescription tDesc = texture.Description;

            TexturedPolygon newQuad = Polygon.CreateTexturedQuad(new Vector4(newX, newY, screenPosition.Z, 1.0f),
                                              tDesc.Width, tDesc.Height);
            if (quad != null)
            {
                //DataBox db = new DataBox(4 * TexturedVertex.Stride, 0, );
                DataBox db = RenderForm11.Device.ImmediateContext.MapSubresource(quad.Vertices, 0,
                                                                                 quad.Vertices.Description.SizeInBytes,
                                                                                 MapMode.WriteDiscard, MapFlags.None);
                Create2(new Vector3(newX, newY, screenPosition.Z), tDesc.Width, tDesc.Height, db.Data);
                //db.Data = Create2(new Vector3(newX, newY, screenPosition.Z), tDesc.Width, tDesc.Height);
                //RenderForm11.Device.ImmediateContext.UpdateSubresource(db,quad.Vertices, 0 );
                RenderForm11.Device.ImmediateContext.UnmapSubresource(quad.Vertices, 0);
                //quad.Vertices = newQuad.Vertices;
                //newQuad.Vertices.Dispose();
            }
            else
            {
                quad = newQuad;
            }

            quad.DiffuseMapKey = text;

            this.text = text;
        }

        #region IRenderable Members

        IRenderable IEntity.RenderableObject
        {
            get { return quad; }
        }
        #endregion


    }
}
