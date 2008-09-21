using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Meshes;
using AvengersUtd.Odyssey.Objects.Effects;
using AvengersUtd.Odyssey.Resources;
using SlimDX;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public class SpecularMaterial : AbstractMaterial
    {
        Color4 diffuseColor;


        public Color4 Diffuse
        {
            get
            {
                return diffuseColor;
            }
            set {
                if (value != diffuseColor)
                {
                    diffuseColor = value;
                    CreateEffect(OwningEntity);
                }
                
            }
        }

        public SpecularMaterial()
        {
            diffuseColor = new Color4(0f, 1.0f, 0f);
            fxType = FXType.Specular;
        }


        public override void CreateIndividualParameters()
        {
            AddIndividualParameter(EffectParameter.CreateCustomParameter(ParamHandles.Colors.Diffuse,
                        effectDescriptor.Effect, diffuseColor));
        }

    }
}
