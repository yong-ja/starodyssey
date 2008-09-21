using System;
using System.Collections.Generic;
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
        protected EffectDescriptor effectDescriptor;
        List<EffectParameter> individualParameters = new List<EffectParameter>();
        IEntity owningEntity;

        protected FXType fxType;
        
        #region Properties
        public Material Material
        {
            get { return material; }
        }

        public IEntity OwningEntity
        {
            get { return owningEntity; }
        }

        public FXType FXType
        {
            get { return fxType; }
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

        public void CreateEffect(IEntity entity)
        {
            owningEntity = entity;
            effectDescriptor = EffectManager.CreateEffect(fxType);
            CreateIndividualParameters();
            effectDescriptor.UpdateStatic();
        }

        public abstract void CreateIndividualParameters();
        

        public virtual void Apply()
        {
            effectDescriptor.UpdateDynamic();
            UpdateIndividual();
            effectDescriptor.Effect.CommitChanges();
        }

        protected void AddIndividualParameter(EffectParameter effectParameter)
        {
            individualParameters.Add(effectParameter);
        }

        protected void UpdateIndividual()
        {
            foreach (EffectParameter p in individualParameters)
                p.Apply();
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