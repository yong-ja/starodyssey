using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Effects;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public abstract class ShaderMaterial : AbstractMaterial, IColorMaterial
    {
        Color4 ambientColor;
        Color4 diffuseColor;
        Color4 specularColor;
        float kA;
        float kD;
        float kS;
        float sP;

        protected LightingAlgorithm lightingAlgorithm;
        protected LightingTechnique lightingTechnique;


        public ShaderMaterial(string filename, RenderableCollectionDescription description) : base(filename, description)
        {
            kA = 0;
            kD = 1;
            kS = 1;
            sP = 16;
            specularColor = new Color4(1f, 1f, 1f, 1f);
            ambientColor = new Color4(1f, 1f, 1f, 1f);
            diffuseColor = new Color4(1, 0, 0, 1);
        }

        public Color4 DiffuseColor
        {
            get { return diffuseColor; }
            set
            {
                if (diffuseColor != value)
                {
                    diffuseColor = value;
                    EffectDescription.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.Diffuse, EffectDescription.Effect));
                }
            }
        }

        public Color4 AmbientColor
        {
            get { return ambientColor; }
            set { ambientColor = value; }
        }

        public Color4 SpecularColor
        {
            get { return specularColor; }
            set
            {
                if (specularColor != value)
                {
                    specularColor = value;
                    EffectDescription.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.Specular, EffectDescription.Effect));
                }
            }
        }

        public float AmbientCoefficient
        {
            get { return kA; }
                        set
            {
                if (kA != value)
                {
                    kA = value;
                    EffectDescription.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.AmbientCoefficient, EffectDescription.Effect));
                }
            }
        }

        public float SpecularCoefficient
        {
            get { return kS; }
        }

        public float DiffuseCoefficient
        {
            get { return kD; }
            set
            {
                if (kD != value)
                {
                    kD = value;
                    EffectDescription.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.DiffuseCoefficient, EffectDescription.Effect));
                }
            }
        }

        public float SpecularPower
        {
            get { return sP;}
        }

        public LightingTechnique LightingTechnique
        {
            get { return lightingTechnique; }
        }

        protected override void OnInstanceParametersInit()
        {
            if ((lightingTechnique & LightingTechnique.Diffuse) != LightingTechnique.Diffuse)
            {
                AmbientColor = new Color4(0, 0, 0, 0);
                DiffuseColor = new Color4(1, 0, 1, 0);
            }
            if ((lightingTechnique & LightingTechnique.Specular) != LightingTechnique.None)
                SpecularColor = new Color4(1, 1, 1, 1);
        }

        public void SetLightingTechnique(LightingTechnique technique, bool value)
        {
            bool previousValue = (LightingTechnique & technique) == technique;
            if (value)
                lightingTechnique |= technique;
            else
                lightingTechnique ^= technique;

            //if (value != previousValue)
            //    ChooseTechnique();
        }

    }
}
