using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Objects.Effects;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public class SpecularBumpMaterial : TexturedEffectMaterial
    {
        protected Texture normal;
        protected Texture clouds;

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
                clouds = TextureManager.LoadTexture(TextureDescriptor.GetTextureFilename(TextureType.Texture1));

            }
        }

        public override void Create(params object[] data)
        {
            //base.Create(data);
            effectDescriptor = EffectManager.CreateEffect(OwningEntity, fxType, diffuse, normal,clouds);
            effectDescriptor.UpdateStatic();
        }

         public SpecularBumpMaterial()
        {
            fxType = FXType.GroundFromSpace;
        }

    }
}