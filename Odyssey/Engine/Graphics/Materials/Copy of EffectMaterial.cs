using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Resources;
using SlimDX;
using AvengersUtd.Odyssey.Objects.Effects;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public class PlanetFromSpaceMaterial : TexturedEffectMaterial, IEffectMaterial
    {

        Texture normal;

        public PlanetFromSpaceMaterial()
        {
            //diffuseColor = new Color4(0f, 1.0f, 0f);
            fxType = FXType.GroundFromSpace;
            //effectDescriptor = EffectManager.CreateEffect(OwningEntity, FXType.AtmosphereFromSpace);
            //effectDescriptor.UpdateStatic();
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
