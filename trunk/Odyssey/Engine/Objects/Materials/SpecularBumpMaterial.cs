using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Materials;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public class SpecularBumpMaterial : TexturedEffectMaterial
    {
        protected Texture normal;

        public Texture Normal
        {
            get { return normal; }
        }

        public override void Init(Material mat, MaterialDescriptor descriptor)
        {
            base.Init(mat, descriptor);
            normal = TextureManager.LoadTexture(descriptor.GetTextureFilename(TextureType.Normal));
        }
    }
}