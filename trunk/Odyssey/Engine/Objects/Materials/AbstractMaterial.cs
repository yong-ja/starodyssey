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

        protected FXType fxType;
        
        #region Properties
        public Material Material
        {
            get { return material; }
            set { material = value; }
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

        public virtual void Create(params object[] data)
        {
            effectDescriptor = EffectManager.CreateEffect(fxType, data);
            effectDescriptor.UpdateStatic();
        }

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