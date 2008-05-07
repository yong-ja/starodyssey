using System;
using System.Collections.Generic;
using System.Text;
using SlimDX;
using AvengersUtd.Odyssey.Objects.Effects;
using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public class DiffuseMaterial:AbstractMaterial,IEffectMaterial
    {
        protected Color4 diffuseColor;

        public Color4 DiffuseColor
        {
            get { return diffuseColor; }
        }

        public DiffuseMaterial()
        {
            effectDescriptor = EffectManager.CreateEffect(FXType.Diffuse);
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


        #region IEffectMaterial Members

        public EffectDescriptor EffectDescriptor
        {
            get { return effectDescriptor; }
        }

        #endregion
    }
}
