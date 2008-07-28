using System;
using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Objects.Effects;
using AvengersUtd.Odyssey.Meshes;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public abstract class AbstractMaterial : IMaterialContainer, IEffectMaterial
    {
        protected bool disposed;
        protected Material material;
        TextureDescriptor[] textureDescriptors;
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

        protected AbstractMaterial()
        {
            material = new Material();
        }

        public virtual void Create(params object[] data)
        {
            effectDescriptor = EffectManager.CreateEffect(owningEntity, fxType, data);
            effectDescriptor.UpdateStatic();
        }

        public virtual void Apply()
        {
            effectDescriptor.UpdateDynamic();
            effectDescriptor.Effect.CommitChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                //if (disposing)
                //{
                //    // dispose managed components
                //}

                // dispose unmanaged components
                OnDisposing();
            }
            disposed = true;
        }

        protected virtual void OnDisposing()
        {
            effectDescriptor.Effect.Dispose();
        }


        ~AbstractMaterial()
        {
            Dispose(false);
        }

        public bool Disposed { get { return disposed; } }
    }
}