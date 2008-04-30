using System;
using AvengersUtd.Odyssey.Resources;
using SlimDX;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Engine.Meshes;

namespace AvengersUtd.Odyssey.Meshes
{
    public abstract class BaseEntity<MaterialT> : IEntity
        where MaterialT : IMaterialContainer, new()

    {
        protected bool disposed = false;
        protected Vector3 position;
        protected SimpleMesh<MaterialT> mesh;

        #region Properties

        public SimpleMesh<MaterialT> MeshObject
        {
            get { return mesh; }
        }

        public EntityDescriptor Descriptor
        {
            get { return mesh.EntityDescriptor; }
        }

        public IMaterialContainer[] Materials
        {
            get { return mesh.Materials as IMaterialContainer[]; }
        }

        #endregion

        public BaseEntity(EntityDescriptor entityDesc)
        {
            mesh = new SimpleMesh<MaterialT>(entityDesc);
        }

        public abstract void Render();

        public abstract void Init();
        public abstract void UpdatePosition();

        #region IEntity Members

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose managed components
                    mesh.Dispose();
                }
            }
            disposed = true;
        }

        ~BaseEntity()
        {
            Dispose(false);
        }

        #endregion
    }
}