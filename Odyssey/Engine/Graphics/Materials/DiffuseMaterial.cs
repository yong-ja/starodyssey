using System;
using System.Collections.Generic;
using System.Text;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class DiffuseMaterial : TexturedMaterial
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

        //public override void OnIndividualParametersInit()
        //{
        //    //throw new NotImplementedException();
        //    return;
        //}

    }
}