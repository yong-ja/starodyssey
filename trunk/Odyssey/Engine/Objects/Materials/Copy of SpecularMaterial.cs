using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Resources;
using SlimDX;
using AvengersUtd.Odyssey.Objects.Effects;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public class EffectMaterial : AbstractMaterial, IEffectMaterial
    {
        //Color4 diffuseColor;


        //public Color4 Diffuse
        //{
        //    get
        //    {
        //        return diffuseColor;
        //    }
        //    set {
        //        if (value != diffuseColor)
        //        {
        //            diffuseColor = value;
        //            effectDescriptor = EffectManager.CreateEffect(FXType.Specular, diffuseColor);
        //            effectDescriptor.UpdateStatic();
                    
        //        }
                
        //    }
        //}

        public EffectMaterial()
        {
            //diffuseColor = new Color4(0f, 1.0f, 0f);
            effectDescriptor = EffectManager.CreateEffect(OwningEntity, FXType.AtmosphericScattering);
            effectDescriptor.UpdateStatic();
        }

        public override void Apply()
        {
            effectDescriptor.UpdateDynamic();
            effectDescriptor.Effect.CommitChanges();
        }

        public override bool Disposed
        {
            get { return false; }
        }
    }
}
