using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Objects.Effects;
using AvengersUtd.Odyssey.Meshes;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public abstract class AbstractMaterial : IMaterialContainer
    {
        protected bool disposed;
        protected Material material;
        protected TextureDescriptor textureDescriptor;
        protected EffectDescriptor effectDescriptor;

        protected FXType fxType;
        IEntity owningEntity;

        #region Properties
        public Material Material
        {
            get { return material; }
            set { material = value; }
        }

        public IEntity OwningEntity
        {
            get { return owningEntity; }
            set { owningEntity = value; }
        }

        public FXType FXType
        {
            get { return fxType; }
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
        #endregion

        public AbstractMaterial()
        {
            material = new Material();
        }

        public virtual void Create(params object[] data)
        {
            effectDescriptor = EffectManager.CreateEffect(owningEntity, fxType, data);
            effectDescriptor.UpdateStatic();
        }

        public abstract void Apply();

        public virtual void Dispose()
        {
        }

        public abstract bool Disposed { get; }
    }
}