using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Objects.Effects;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public abstract class AbstractMaterial : IMaterialContainer
    {
        protected bool disposed;
        protected Material material;
        protected MaterialDescriptor materialDescriptor;
        protected EffectDescriptor effectDescriptor;

        public virtual void Init() 
        {
            Init(new Material(), null, null);
        }

        public virtual void Init(Material mat, MaterialDescriptor matDescriptor)
        {
            Init(mat, matDescriptor, null);
        }

        public virtual void Init(Material mat, MaterialDescriptor matDescriptor, 
            EffectDescriptor fxDescriptor)
        {
            material = mat;
            materialDescriptor = matDescriptor;
            effectDescriptor = fxDescriptor;
        }


        public abstract void Apply();

        public virtual void Dispose()
        {
        }

        public abstract bool Disposed { get; }
    }
}