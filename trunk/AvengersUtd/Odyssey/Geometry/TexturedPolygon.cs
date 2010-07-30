using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace AvengersUtd.Odyssey.Geometry
{
    public class TexturedPolygon : Polygon, IDiffuseMap
    {
        private List<ShaderResourceView> shaderResources;
        private Texture2D diffuseMap;

        public TexturedPolygon(Buffer vertices, Buffer indices, int vertexCount) : base(vertices, indices, vertexCount, TexturedVertex.Description)
        {
            shaderResources = new List<ShaderResourceView>();
        }

        public Texture2D DiffuseMap
        {
            get { return diffuseMap; }
            set
            {
                diffuseMap = value;
                shaderResources.Add(new ShaderResourceView(RenderForm11.Device, diffuseMap));
            }
        }

        #region IDiffuseMap Members


        public ShaderResourceView DiffuseMapResource
        {
            get { return shaderResources[0]; }
        }
        #endregion
    }
}
