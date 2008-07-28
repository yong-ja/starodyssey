using System;
using System.Collections.Generic;
using System.Text;
using SlimDX;
using AvengersUtd.Odyssey.Objects.Effects;
using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public class DiffuseMaterial:AbstractMaterial
    {
        protected Color4 diffuseColor;

        public Color4 DiffuseColor
        {
            get { return diffuseColor; }
        }

        public DiffuseMaterial()
        {
            fxType = FXType.Diffuse;
        }



        #region IEffectMaterial Members

        public EffectDescriptor EffectDescriptor
        {
            get { return effectDescriptor; }
        }

        #endregion
    }
}
