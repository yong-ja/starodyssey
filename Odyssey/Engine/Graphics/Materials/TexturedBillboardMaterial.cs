using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class TexturedBillboardMaterial : TexturedMaterial
    {
        float billboardSize;

        public float BillboardSize
        {
            get { return billboardSize; }
            set
            {
                if (billboardSize != value)
                {
                    billboardSize = value;
                    OnIndividualParametersInit();
                }
            }
        }

        public TexturedBillboardMaterial()
        {
            fxType = FXType.SelfAlign;
        }

        protected override void OnIndividualParametersInit()
        {
            base.OnIndividualParametersInit();
            billboardSize = ((IBillboard) OwningEntity).BillboardSize;
            SetIndividualParameter(EffectParameter.CreateCustomParameter(
                                       ParamHandles.Floats.Scale, effectDescriptor.Effect, billboardSize));
            SetIndividualParameter(EffectParameter.CreateCustomParameter(
                                       ParamHandles.Vectors.EntityPosition, effectDescriptor.Effect,
                                       OwningEntity.PositionV4));
        }
    }
}