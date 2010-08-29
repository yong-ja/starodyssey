using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Effects;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class ShaderMaterial : AbstractMaterial, IColor4Material
    {
        Color4 ambientColor4 = new Color4(0, 0, 0, 0);
        Color4 diffuseColor = new Color4(0, 0, 0, 1);
        Color4 specularColor4;
        float kA;
        float kD;
        float kS;

        protected LightingAlgorithm lightingAlgorithm;
        protected LightingTechnique lightingTechnique;


        public ShaderMaterial(string filename) : base(filename)
        {
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

        public Color4 AmbientColor4
        {
            get { return ambientColor4; }
            set { ambientColor4 = value; }
        }

        public Color4 SpecularColor4
        {
            get { return specularColor4; }
            set
            {
                if (specularColor4 != value)
                {
                    specularColor4 = value;
                    EffectDescription.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.Specular, EffectDescription.Effect));
                }
            }
        }

        public LightingTechnique LightingTechnique
        {
            get { return lightingTechnique; }
        }

        protected override void OnInstanceParametersInit()
        {
            if ((lightingTechnique & LightingTechnique.Diffuse) != LightingTechnique.Diffuse)
            {
                AmbientColor4 = new Color4(0, 0, 0, 0);
                DiffuseColor = new Color4(1, 0, 1, 0);
            }
            if ((lightingTechnique & LightingTechnique.Specular) != LightingTechnique.None)
                SpecularColor4 = new Color4(1, 1, 1, 1);
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
