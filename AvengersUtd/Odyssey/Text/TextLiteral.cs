using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering;
using SlimDX;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Settings;

namespace AvengersUtd.Odyssey.Text
{
    public class TextLiteral : IEntity
    {
        private readonly TexturedPolygon quad;
        private string text;
        private Color4 color;
        private readonly Texture2D texture;

        public string Content
        {
            get { return text; }
        }

        public TextLiteral(string text, Vector3 screenPosition)
        {
            float newX =((VideoSettings.ScreenWidth/2f)*-1f) + screenPosition.X;
            float newY = (VideoSettings.ScreenHeight/2f) - screenPosition.Y;

            //newX = screenPosition.X;
            //newY = screenPosition.Y;

            texture = TextManager.DrawText(text);
            Texture2DDescription tDesc = texture.Description;

            quad = Polygon.CreateTexturedQuad(new Vector4(newX, newY, screenPosition.Z, 1.0f),
                                              tDesc.Width, tDesc.Height);
            quad.DiffuseMap = texture;
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
