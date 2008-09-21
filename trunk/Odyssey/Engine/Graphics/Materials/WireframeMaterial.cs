using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Effects;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class WireframeMaterial : AbstractMaterial
    {
        public WireframeMaterial() : base(LightingAlgorithm.Wireframe, "Wireframe.fx")
        {
            lightingTechnique = LightingTechnique.Diffuse;
        }

        protected override void OnDynamicParametersInit()
        {
            effectDescriptor.SetDynamicParameter(FXParameterType.WorldViewProjection);
        }
    }
}
