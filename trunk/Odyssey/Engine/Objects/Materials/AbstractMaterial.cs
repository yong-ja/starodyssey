using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Materials;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public abstract class AbstractMaterial : IMaterialContainer
    {
        protected bool disposed;
        protected Material material;
        protected MaterialDescriptor materialDescriptor;

        public virtual void Init(Material mat, MaterialDescriptor descriptor)
        {
            material = mat;
            materialDescriptor = descriptor;
        }

        public abstract void Apply();

        public virtual void Dispose()
        {
        }

        public abstract bool Disposed { get; }
    }
}