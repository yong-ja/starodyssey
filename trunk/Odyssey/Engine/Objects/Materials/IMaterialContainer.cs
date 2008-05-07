using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Objects.Effects;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public interface IMaterialContainer
    {
        Material Material { get; set; }
        TextureDescriptor TextureDescriptor { get; set;}
        EffectDescriptor EffectDescriptor { get; set; }
        void Apply();
        void Dispose();
        bool Disposed { get; }
    }
}