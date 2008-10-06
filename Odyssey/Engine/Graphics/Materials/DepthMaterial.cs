using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Settings;
using SlimDX;
using AvengersUtd.Odyssey.Objects.Entities;
using AvengersUtd.Odyssey.Graphics.Effects;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class DepthMaterial : AbstractMaterial
    {
        Format format;

        public Format Format
        {
            get { return format; }
        }

        public DepthMaterial()
            : base(LightingAlgorithm.Depth, "DepthMap.fx")
        {
            lightingTechnique = LightingTechnique.Shadows;
            format = EngineSettings.Video.BestSingleChannelTextureFormat;
        }

        //public override void CreateEffect(IEntity entity)
        //{
        //    OwningEntity = entity;
        //    effectDescriptor.ApplyStaticParameters();
        //    InitParameters();
        //}

        protected override void OnDynamicParametersInit()
        {
            effectDescriptor.SetDynamicParameter(FXParameterType.World);
            effectDescriptor.SetDynamicParameter(FXParameterType.WorldViewProjection);
        }

        protected override void HandleSpotlight(Spotlight spotlight)
        {
            Effect effect = effectDescriptor.Effect;

            EffectParameter epLightPosition = spotlight.CreateEffectParameter(LightParameter.Position, effect);
            EffectParameter epRadius = spotlight.CreateEffectParameter(LightParameter.Radius, effect);

            effectDescriptor.SetStaticParameter(epLightPosition);
            effectDescriptor.SetStaticParameter(epRadius);
            
            EffectParameter epLightWorldViewProjection = spotlight.CreateEffectParameter(LightParameter.LightWorldViewProjection,effect);
            effectDescriptor.SetDynamicParameter(epLightWorldViewProjection);
        }


        protected override void ChooseTechnique()
        {
            string tag = string.Empty;
            switch (format)
            {
                case Format.R32F:
                case Format.R16F:
                    tag = "f";
                    break;

                case Format.A8R8G8B8:
                    tag = "32i";
                    break;

                case Format.X8R8G8B8:
                    tag = "24i";
                    break;
            }

            effectDescriptor.Technique = string.Format("{0}{1}{2}", lightingAlgorithm,
                                                       EngineSettings.Video.ShadowAlgorithmTag, tag);
        }


    }
}