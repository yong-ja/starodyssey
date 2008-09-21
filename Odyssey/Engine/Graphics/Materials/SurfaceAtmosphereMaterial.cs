using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Effects;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class SurfaceAtmosphereMaterial : TexturedMaterial
    {
        Texture normalMap;
        Texture cloudMap;
        Texture specularMap;

        public Texture NormalMap
        {
            get { return normalMap; }
        }

        protected override void OnIndividualParametersInit()
        {
            SetIndividualParameter(EffectParameter.CreateCustomParameter(
                                       ParamHandles.Textures.DiffuseMap, effectDescriptor.Effect, diffuseMap));
            SetIndividualParameter(EffectParameter.CreateCustomParameter(
                                       ParamHandles.Textures.NormalMap, effectDescriptor.Effect, normalMap));
            SetIndividualParameter(EffectParameter.CreateCustomParameter(
                                       ParamHandles.Textures.Texture1, effectDescriptor.Effect, cloudMap));
            SetIndividualParameter(EffectParameter.CreateCustomParameter(
                                       ParamHandles.Textures.SpecularMap, effectDescriptor.Effect, specularMap));

            FloatOp cameraHeight =
                delegate() { return (Game.CurrentScene.Camera.PositionV3 - OwningEntity.PositionV3).Length(); };

            EffectParameter epCameraHeight = EffectParameter.CreateCustomParameter("cameraHeight",
                                                                                   effectDescriptor.Effect, cameraHeight);
            SetIndividualParameter(epCameraHeight);

            FloatOp cameraHeight2 =
                delegate() { return (Game.CurrentScene.Camera.PositionV3 - OwningEntity.PositionV3).LengthSquared(); };
            EffectParameter epCameraHeight2 = EffectParameter.CreateCustomParameter("cameraHeight2",
                                                                                    effectDescriptor.Effect,
                                                                                    cameraHeight2);
            SetIndividualParameter(epCameraHeight2);
        }

        public SurfaceAtmosphereMaterial()
        {
            fxType = FXType.SurfaceFromSpaceWithAtmosphere;
        }

        public override void LoadTextures(MaterialDescriptor materialDescriptor)
        {
            base.LoadTextures(materialDescriptor);
            normalMap = TextureManager.LoadTexture(materialDescriptor[TextureType.Normal].TextureFilename);
            cloudMap = TextureManager.LoadTexture(materialDescriptor[TextureType.Texture1].TextureFilename);
            specularMap = TextureManager.LoadTexture(materialDescriptor[TextureType.Specular].TextureFilename);
        }


        //protected override void OnDisposing()
        //{
        //    base.OnDisposing();
        //    normalMap.Dispose();
        //    cloudMap.Dispose();
        //    specularMap.Dispose();
        //}
    }
}