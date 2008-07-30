using AvengersUtd.Odyssey.Meshes;
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

        public override void CreateIndividualParameters()
        {
            //base.Create(entity);
            //effectDescriptor = EffectManager.CreateEffect(fxType, 
            //    diffuse, normal,clouds, specular);

            effectDescriptor.AddStaticParameter(EffectParameter.CreateCustomParameter(
                        ParamHandles.Textures.DiffuseMap, effectDescriptor.Effect, diffuse));
            effectDescriptor.AddStaticParameter(EffectParameter.CreateCustomParameter(
                ParamHandles.Textures.NormalMap, effectDescriptor.Effect, normal));
            effectDescriptor.AddStaticParameter(EffectParameter.CreateCustomParameter(
                ParamHandles.Textures.Texture1, effectDescriptor.Effect, clouds));
            effectDescriptor.AddStaticParameter(EffectParameter.CreateCustomParameter(
                ParamHandles.Textures.Texture2, effectDescriptor.Effect, specular));

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