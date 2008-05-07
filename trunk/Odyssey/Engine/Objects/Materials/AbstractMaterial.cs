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
        protected TextureDescriptor textureDescriptor;
        protected EffectDescriptor effectDescriptor;

        public Material Material
        {
            get { return material; }
            set { material = value; }
        }

        public virtual TextureDescriptor TextureDescriptor
        {
            get { return textureDescriptor; }
            set { textureDescriptor = value; }
        }

        public virtual EffectDescriptor EffectDescriptor
        {
            get { return effectDescriptor; }
            set { effectDescriptor = value; }
        }

        public AbstractMaterial()
        {
            material = new Material();
        }

        //public virtual void Init(Material mat, MaterialDescriptor matDescriptor)
        //{
        //    Init(mat, matDescriptor, null);
        //}

        //public virtual void Init(Material mat, MaterialDescriptor matDescriptor, 
        //    EffectDescriptor fxDescriptor)
        //{
        //    material = mat;
        //    materialDescriptor = matDescriptor;
        //    effectDescriptor = fxDescriptor;
        //}


        public abstract void Apply();

        public virtual void Dispose()
        {
        }

        public abstract bool Disposed { get; }
    }
}