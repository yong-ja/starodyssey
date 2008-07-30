using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Objects.Effects;
using AvengersUtd.Odyssey.Meshes;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public interface IMaterialContainer
    {
        Material Material { get; }
        EffectDescriptor EffectDescriptor { get; set; }
        IEntity OwningEntity { get; }
        void Apply();
        void Dispose();
        bool Disposed { get; }
        void CreateEffect(IEntity entity);
    }
}