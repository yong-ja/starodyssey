using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Objects.Effects;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public class SpecularBumpMaterial : TexturedMaterial
    {
        protected Texture normal;
        Texture clouds;
        Texture specular;

        public Texture Normal
        {
            get { return normal; }
        }

        public override void Create(params object[] data)
        {
            effectDescriptor = EffectManager.CreateEffect(fxType, 
                diffuse, normal,clouds, specular);
            effectDescriptor.UpdateStatic();
        }

        public SpecularBumpMaterial()
        {
            fxType = FXType.GroundFromSpace;
        }

        public override void LoadTextures(MaterialDescriptor materialDescriptor)
        {
            base.LoadTextures(materialDescriptor);
            normal = TextureManager.LoadTexture(materialDescriptor[TextureType.Normal].TextureFilename);
            clouds = TextureManager.LoadTexture(materialDescriptor[TextureType.Texture1].TextureFilename);
            specular = TextureManager.LoadTexture(materialDescriptor[TextureType.Texture2].TextureFilename);

        }

        protected override void OnDisposing()
        {
            base.OnDisposing();
            normal.Dispose();
            clouds.Dispose();
            specular.Dispose();
        }
    }
}