using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Objects.Effects;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public interface IEffectMaterial
    {
        EffectDescriptor EffectDescriptor { get; }
    }
}
