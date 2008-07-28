using System;
using AvengersUtd.Odyssey.Resources;
using SlimDX;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Engine.Meshes;
using AvengersUtd.Odyssey.UserInterface.RenderableControls.Interfaces;

namespace AvengersUtd.Odyssey.Meshes
{
    public abstract class BaseEntity: IEntity, I3dEntity
    {
        protected bool disposed = false;
        protected Vector3 position;
        protected SimpleMesh mesh;

        #region Properties

        public SimpleMesh MeshObject
        {
            get { return mesh; }
        }

        public EntityDescriptor Descriptor
        {
            get { return mesh.EntityDescriptor; }
        }

        public AbstractMaterial[] Materials
        {
            get { return mesh.Materials; }
        }

        #endregion

        public BaseEntity(EntityDescriptor entityDesc)
        {
            mesh = new SimpleMesh(this, entityDesc);
        }

        public virtual void Render()
        {
            mesh.Render();
        }

        //public abstract void Init();
        //public abstract void UpdatePosition();

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

        #region I3dEntity Members

        SlimDX.Direct3D9.Mesh I3dEntity.Mesh
        {
            get { return mesh.Mesh; }
        }

        #endregion
    }
}