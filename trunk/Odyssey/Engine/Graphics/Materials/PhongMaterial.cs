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
            lightingTechnique |= LightingTechnique.Shadows;
        }

        public override void CreateEffect(IEntity entity)
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


        #region ITexturedMaterial Members

        public void LoadTextures(MaterialDescriptor materialDescriptor)
        {
            
        }

        #endregion
    }
}