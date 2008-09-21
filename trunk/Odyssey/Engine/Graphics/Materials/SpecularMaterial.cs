using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class SpecularMaterial : AbstractMaterial
    {
        Color4 diffuseColor;


        public Color4 Diffuse
        {
            get { return diffuseColor; }
            set
            {
                if (value != diffuseColor)
                {
                    diffuseColor = value;
                    CreateEffect(OwningEntity);
                }
            }
        }

        public SpecularMaterial()
            : base(LightingAlgorithm.Phong,"Phong.fx")
        {
            DiffuseColor = new Color4(0f, 0.0f, 1f);
        }

        public override void CreateEffect(IEntity entity)
        {
            OwningEntity = entity;
            InitParameters();

        }

        protected override void OnStaticParametersInit()
        {
            //effectDescriptor.SetStaticParameter(FXParameterType.TextureBias);
        }

        protected override void OnDynamicParametersInit()
        {
            effectDescriptor.SetDynamicParameter(FXParameterType.EyePosition);
            effectDescriptor.SetDynamicParameter(FXParameterType.World);
            effectDescriptor.SetDynamicParameter(FXParameterType.WorldViewProjection);
        }




        protected override void OnIndividualParametersInit()
        {
            SetIndividualParameter(EffectParameter.CreateCustomParameter(ParamHandles.Colors.MaterialDiffuse,
                                                                         effectDescriptor.Effect, diffuseColor));
        }
    }
}