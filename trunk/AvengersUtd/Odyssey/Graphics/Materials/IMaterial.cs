

using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using System;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public interface IMaterial
    {
        MaterialNode OwningNode { get; }
        EffectDescription EffectDescription { get; }
        void ApplyDynamicParameters();
        
    }
}