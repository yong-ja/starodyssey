using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class TexturedMaterial : AbstractMaterial, ITexturedMaterial
    {
        protected Texture diffuseMap;

        public Texture DiffuseMap
        {
            get { return diffuseMap; }
            set { diffuseMap = value; }
        }

        //protected override void OnDisposing()
        //{
        //    if (diffuseMap != null)
        //        diffuseMap.Dispose();
        //}

        public TexturedMaterial()
        {
            fxType = FXType.Textured;
        }


        public virtual void LoadTextures(MaterialDescriptor materialDescriptor)
        {
            diffuseMap = TextureManager.LoadTexture(materialDescriptor[TextureType.Diffuse].TextureFilename);
        }

        protected override void OnIndividualParametersInit()
        {
            SetIndividualParameter(EffectParameter.CreateCustomParameter(
                                       ParamHandles.Textures.DiffuseMap, effectDescriptor.Effect, diffuseMap));
        }

    }
}