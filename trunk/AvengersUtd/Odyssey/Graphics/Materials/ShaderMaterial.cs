using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Effects;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class ShaderMaterial : AbstractMaterial, IColorMaterial
    {
        Color4 ambientColor = new Color4(0, 0, 0, 0);
        Color4 diffuseColor = new Color4(0, 0, 0, 1);
        Color4 specularColor;
        float kA;
        float kD;
        float kS;

        protected LightingAlgorithm lightingAlgorithm;
        protected LightingTechnique lightingTechnique;


        public Color4 DiffuseColor
        {
            get { return diffuseColor; }
            set
            {
                if (diffuseColor != value)
                {
                    diffuseColor = value;
                    effectDescriptor.SetStaticParameter(SharedParameter.CreateDefault(MaterialParameter.Diffuse, effectDescriptor.Effect, this));
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
                    effectDescriptor.SetStaticParameter(SharedParameter.CreateDefault(MaterialParameter.Specular, effectDescriptor.Effect, this));
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
