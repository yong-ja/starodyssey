using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Settings;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class PhongMaterial : AbstractMaterial, ITexturedMaterial, ICastsShadows
    {
        Texture shadowMap;
        Texture diffuseMap;

        public Texture DiffuseMap
        {
            get { return diffuseMap; }
            set { diffuseMap = value; }
        }

        public Texture ShadowMap
        {
            get { return shadowMap; }
            set
            {
                if (shadowMap != value)
                {
                    shadowMap = value;
                }
            }
        }

        public PhongMaterial(): base(LightingAlgorithm.Phong,"Phong.fx")
        {
            lightingTechnique |= LightingTechnique.Diffuse | LightingTechnique.Shadows;
        }

        public override void CreateEffect(IRenderable entity)
        {
            DiffuseColor = new SlimDX.Color4(0, 0, 1);
            OwningEntity = entity;
            InitParameters();

        }

        protected override void OnStaticParametersInit()
        {
            if ((lightingTechnique & LightingTechnique.Shadows) != LightingTechnique.None)
            {
                EffectParameter epTextureType = null;
                string varName = ParamHandles.Integers.TextureType;
                switch (EngineSettings.Video.BestSingleChannelTextureFormat)
                {
                    case Format.R32F:
                        epTextureType = EffectParameter.CreateCustomParameter(varName, effectDescriptor.Effect, 0);
                        break;

                    case Format.A8R8G8B8:
                        epTextureType = EffectParameter.CreateCustomParameter(varName, effectDescriptor.Effect, 1);
                        break;

                    case Format.X8R8G8B8:
                        epTextureType = EffectParameter.CreateCustomParameter(varName, effectDescriptor.Effect, 2);
                        break;
                }
                effectDescriptor.SetStaticParameter(epTextureType);
                effectDescriptor.SetStaticParameter(FXParameterType.TextureBias);
            }
        }

        protected override void OnDynamicParametersInit()
        {
            effectDescriptor.SetDynamicParameter(FXParameterType.EyePosition);
            effectDescriptor.SetDynamicParameter(FXParameterType.World);
            effectDescriptor.SetDynamicParameter(FXParameterType.WorldViewProjection);
        }

        protected override void OnIndividualParametersInit()
        {
            if ((lightingTechnique & LightingTechnique.Shadows) != LightingTechnique.None)
            {
                SetIndividualParameter(EffectParameter.CreateCustomParameter(
                                           ParamHandles.Textures.ShadowMap, effectDescriptor.Effect, this));
            }
        }

        public override EffectParameter CreateEffectParameter(MaterialParameter parameter, Effect effect)
        {
            string varName;
            EffectHandle eh;
            Update update;

            switch (parameter)
            {
                case MaterialParameter.DiffuseMap:
                    varName = ParamHandles.Textures.DiffuseMap;
                    eh = new EffectHandle(varName);
                    update = (fxParam => fxParam.OwnerEffect.SetTexture(eh, diffuseMap));
                    break;

                default:
                    return base.CreateEffectParameter(parameter, effect);
            }

            return new EffectParameter(varName, effect, update);
        }


        #region ITexturedMaterial Members

        public void LoadTextures(MaterialDescriptor materialDescriptor)
        {
            
            foreach (TextureDescriptor tDesc in materialDescriptor.TextureDescriptors)
            {
                switch (tDesc.Type)
                {
                    case TextureType.Diffuse:
                        SetLightingTechnique(LightingTechnique.Diffuse, false);
                        SetLightingTechnique(LightingTechnique.DiffuseMap, true);
                        diffuseMap = TextureManager.LoadTexture(tDesc.TextureFilename);
                        SetIndividualParameter(CreateEffectParameter(MaterialParameter.DiffuseMap, effectDescriptor.Effect));
                        break;
                }
            }
            ChooseTechnique();
        }

        #endregion
    }
}