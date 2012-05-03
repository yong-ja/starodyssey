using System;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Resources;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public partial class TexturedPolygon : BaseMesh<TexturedVertex>, IDiffuseMap
    {
        private string diffuseMapKey;
        private Texture2D diffuseMap;
                
        public TexturedPolygon(string tag, Vector3 topLeftVertex, float width, float height, bool dynamic = false) : base(TexturedVertex.Description)
        {
            ushort[] indices;
            Vertices = CreateTexturedQuad(topLeftVertex, width, height, out indices);
            Indices = indices;
            if (dynamic)
            {
                CpuAccessFlags = CpuAccessFlags.Write;
                ResourceUsage = ResourceUsage.Dynamic;
            }
           
            Name = diffuseMapKey = tag;
        }

        #region IDiffuseMap Members
        public string DiffuseMapKey
        {
            get { return diffuseMapKey; }
            set
            {
                if (diffuseMapKey == value) return;

                diffuseMapKey = value;
                ShaderResourceView srv = ResourceManager.GetResource(diffuseMapKey);
                diffuseMap = (Texture2D) srv.Resource;
                if (ShaderResourceList.Count == 0)
                    ShaderResourceList.Add(srv);
                else
                    ShaderResourceList[0] = srv;
            }
        }

        public Texture2D DiffuseMapTexture2D
        {
            get { return diffuseMap; }
        }

        public ShaderResourceView DiffuseMapResource
        {
            get { return ShaderResourceList[0]; }
            set
            {
                if (ShaderResourceList.Count == 0)
                    ShaderResourceList.Add(value);
                else
                    ShaderResourceList[0] = value;
                diffuseMapKey = Properties.Resources.RES_TextureNotCached;
                diffuseMap = (Texture2D)value.Resource;
            }
        }
        #endregion

        protected override void OnDisposing(EventArgs e)
        {
            base.OnDisposing(e);
            if (diffuseMap != null)
                diffuseMap.Dispose();

            if (ShaderResourceList.Count == 0)
                return;

            if (!ShaderResourceList[0].Disposed)
                ShaderResourceList[0].Dispose();
        }

    }
}
