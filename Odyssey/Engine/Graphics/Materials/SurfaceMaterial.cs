using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class SurfaceMaterial : TexturedMaterial
    {
        Texture normalMap;
        Texture specularMap;

        public Texture NormalMap
        {
            get { return normalMap; }
        }

        public SurfaceMaterial()
        {
            fxType = FXType.SurfaceFromSpace;
        }

        public override void LoadTextures(MaterialDescriptor materialDescriptor)
        {
            base.LoadTextures(materialDescriptor);
            normalMap = TextureManager.LoadTexture(materialDescriptor[TextureType.Normal].TextureFilename);
            specularMap = TextureManager.LoadTexture(materialDescriptor[TextureType.Specular].TextureFilename);
        }

        protected override void OnIndividualParametersInit()
        {
            SetIndividualParameter(EffectParameter.CreateCustomParameter(
                                       ParamHandles.Textures.DiffuseMap, effectDescriptor.Effect, diffuseMap));
            SetIndividualParameter(EffectParameter.CreateCustomParameter(
                                       ParamHandles.Textures.NormalMap, effectDescriptor.Effect, normalMap));
            SetIndividualParameter(EffectParameter.CreateCustomParameter(
                                       ParamHandles.Textures.SpecularMap, effectDescriptor.Effect, specularMap));
        }

    }
}