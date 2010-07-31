using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Resources;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace AvengersUtd.Odyssey.Geometry
{
    public class TexturedPolygon : Polygon, IDiffuseMap
    {
        private string diffuseMapKey;
        
        public TexturedPolygon(Buffer vertices, Buffer indices, int vertexCount) : base(vertices, indices, vertexCount, TexturedVertex.Description)
        {
            diffuseMapKey = string.Empty;
        }

        internal string DiffuseMapKey
        {
            get { return diffuseMapKey; }
            set
            {
                if (diffuseMapKey != value)
                {
                    diffuseMapKey = value;

                    ShaderResourceView srv = ResourceManager.GetResource(diffuseMapKey);
                    if (ShaderResourceList.Count == 0)
                        ShaderResourceList.Add(srv);
                    else
                        ShaderResourceList[0] = srv;
                }
            }
        }


        #region IDiffuseMap Members

        public Texture2D DiffuseMap
        {
            get { return ResourceManager.GetTexture(DiffuseMapKey); }
        }

        public ShaderResourceView DiffuseMapResource
        {
            get { return ShaderResourceList[0]; }
        }
        #endregion

      
    }
}
