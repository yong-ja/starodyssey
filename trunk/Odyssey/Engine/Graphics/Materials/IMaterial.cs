using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Effects;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public interface IMaterial
    {
        EffectDescriptor EffectDescriptor { get; }
        IEntity OwningEntity { get; }
        void ApplyDynamicParameters();
        void CreateEffect(IEntity entity);
    }
}