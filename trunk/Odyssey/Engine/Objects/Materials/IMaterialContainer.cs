using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Materials;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public interface IMaterialContainer
    {
        void Init(Material mat, MaterialDescriptor descriptor);
        void Apply();
        void Dispose();
        bool Disposed { get; }
    }
}