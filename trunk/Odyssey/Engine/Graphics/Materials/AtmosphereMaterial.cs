using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Effects;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class AtmosphereMaterial : AbstractMaterial
    {
        float innerRadius;
        Vector4 wavelength;
        Vector4 invWavelength;

        public Vector4 Wavelength
        {
            get { return wavelength; }
            set
            {
                if (wavelength != value)
                {
                    wavelength = value;
                    OnIndividualParametersInit();
                }
            }
        }

        public float InnerRadius
        {
            get { return innerRadius; }
            set
            {
                if (innerRadius != value)
                    innerRadius = value;
                OnIndividualParametersInit();
            }
        }

        public AtmosphereMaterial()
        {
            fxType = FXType.AtmosphericScattering;

            wavelength = new Vector4(0.650f, 0.570f, 0.450f, 1.0f);
        }

        protected override void OnIndividualParametersInit()
        {
            innerRadius = ((ISphere) OwningEntity).Radius;
            float outerRadius = innerRadius*1.025f;
            float scale = 1.0f/(outerRadius - innerRadius);
            float scaleDepth = outerRadius - innerRadius;
            float scaleOverScaleDepth = scale/scaleDepth;

            EffectParameter epInnerRadius = EffectParameter.CreateCustomParameter("innerRadius", effectDescriptor.Effect,
                                                                                  innerRadius);
            SetIndividualParameter(epInnerRadius);

            EffectParameter epOuterRadius = EffectParameter.CreateCustomParameter("outerRadius", effectDescriptor.Effect,
                                                                                  outerRadius);
            SetIndividualParameter(epOuterRadius);

            EffectParameter epOuterRadius2 = EffectParameter.CreateCustomParameter("outerRadius2",
                                                                                   effectDescriptor.Effect,
                                                                                   outerRadius*outerRadius);
            SetIndividualParameter(epOuterRadius2);

            EffectParameter epScale = EffectParameter.CreateCustomParameter("scale", effectDescriptor.Effect, scale);
            SetIndividualParameter(epScale);

            EffectParameter epScaleDepth = EffectParameter.CreateCustomParameter("scaleDepth", effectDescriptor.Effect,
                                                                                 scaleDepth);
            SetIndividualParameter(epScaleDepth);

            EffectParameter epScaleOverScaleDepth = EffectParameter.CreateCustomParameter("scaleOverScaleDepth",
                                                                                          effectDescriptor.Effect,
                                                                                          scaleOverScaleDepth);
            SetIndividualParameter(epScaleOverScaleDepth);


            invWavelength = new Vector4(
                (float) (1.0/Math.Pow(wavelength.X, 4.0)),
                (float) (1.0/Math.Pow(wavelength.Y, 4.0)),
                (float) (1.0/Math.Pow(wavelength.Z, 4.0)),
                1);

            EffectParameter epInvWavelength = EffectParameter.CreateCustomParameter("vInvWavelength",
                                                                                    effectDescriptor.Effect,
                                                                                    invWavelength);
            SetIndividualParameter(epInvWavelength);
        }

    }
}