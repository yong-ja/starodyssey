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

        public override TextureDescriptor TextureDescriptor
        {
            get
            {
                return base.TextureDescriptor;
            }
            set
            {
                base.TextureDescriptor = value;
                normal = TextureManager.LoadTexture(TextureDescriptor.GetTextureFilename(TextureType.Normal));
            }
        }

    }
}