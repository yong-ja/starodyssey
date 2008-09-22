using System;
using System.Collections.Generic;
using System.ComponentModel;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Resources;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.UserInterface.RenderableControls.Interfaces;
using AvengersUtd.Odyssey.Graphics.Effects;
using SlimDX.Direct3D9;
using System.Text.RegularExpressions;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public abstract class BaseEntity : IRenderable, I3dEntity
    {
        bool disposed;
        bool castsShadows;
        RenderableNode parentNode;
        EventHandlerList eventHandlerList;
        protected Vector3 positionV3;
        protected Vector3 rotationAngles;
        protected Vector3 scalingValues;
        protected SimpleMesh meshObject;


        #region Events

        static readonly object eventPositionChanged;
        static readonly object eventPositionChanging;

        public event EventHandler<PositionEventArgs> PositionChanged
        {
            add { eventHandlerList.AddHandler(eventPositionChanged, value); }
            remove { eventHandlerList.RemoveHandler(eventPositionChanged, value); }
        }

        protected virtual void OnPositionChanged(PositionEventArgs e)
        {
            EventHandler<PositionEventArgs> handler = (EventHandler<PositionEventArgs>)eventHandlerList[eventPositionChanged];
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<PositionEventArgs> PositionChanging
        {
            add { eventHandlerList.AddHandler(eventPositionChanging, value); }
            remove { eventHandlerList.RemoveHandler(eventPositionChanging, value); }
        }

        protected virtual void OnPositionChanging(PositionEventArgs e)
        {
            EventHandler<PositionEventArgs> handler = (EventHandler<PositionEventArgs>) eventHandlerList[eventPositionChanging];
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region Properties

        public bool CastsShadows
        {
            get { return castsShadows; }
            set { castsShadows = false; }
        }

        public SimpleMesh MeshObject
        {
            get { return meshObject; }
        }

        public EntityDescriptor Descriptor
        {
            get { return meshObject.EntityDescriptor; }
        }

        public AbstractMaterial[] Materials
        {
            get { return meshObject.Materials; }
        }

        public RenderableNode ParentNode
        {
            get { return parentNode; }
        }

        #endregion

        static BaseEntity()
        {
            eventPositionChanging = new object();
            eventPositionChanged = new object();
        }

        protected BaseEntity(EntityDescriptor descriptor, IAxisAlignedBox box) :
            this(descriptor, GeometryHelper.CreateParallelepiped(box))
        {
        }

        protected BaseEntity(EntityDescriptor descriptor, ISphere sphere) :
            this(descriptor, GeometryHelper.CreateSphere(sphere))
        {
        }

        BaseEntity(EntityDescriptor descriptor, Mesh mesh) :
            this()
        {
            meshObject = new SimpleMesh(descriptor, mesh);
        }

        protected BaseEntity(EntityDescriptor entityDesc): this()
        {
            
            //TODO: if (checked filename validity)
                meshObject = new SimpleMesh(entityDesc);
            //else
            //    throw new InvalidOperationException(string.Format("{0}{1}", Properties.Resources.ERR_FilenameNotValid));

        }

        BaseEntity()
        {
            eventHandlerList = new EventHandlerList();
            castsShadows = true;
            CurrentRotation = Quaternion.Identity;
        }

        public virtual void Init()
        {
            meshObject.Init();
        }

        /// <summary>
        /// Renders this entity using its associated material.
        /// </summary>
        public virtual void Render()
        {
            meshObject.Render();
        }

        public virtual void DrawMesh()
        {
            meshObject.DrawMesh();
        }

        public bool IsInViewFrustum()
        {
            ISphere iSphere = this as ISphere;
            if (iSphere == null)
                return true;

            BoundingSphere bSphere = new BoundingSphere(iSphere.Center, iSphere.Radius);

            ContainmentType cType = BoundingFrustum.Contains(Game.CurrentScene.Camera.Frustum, bSphere);

            if (cType == ContainmentType.Disjoint)
                return false;
            else
                return true;
        }

        public virtual bool Intersects(Ray ray)
        {
            return false;
        }

        public virtual void UpdatePosition()
        {
        }

        #region IEntity Members

        public Vector3 PositionV3
        {
            get { return positionV3; }
            set
            {
                if (positionV3 != value)
                {
                    PositionEventArgs e = new PositionEventArgs(positionV3, value);
                    OnPositionChanging(e);
                    positionV3 = e.NewValue;
                    OnPositionChanged(e);
                    
                }
            }
        }

        public Vector4 PositionV4
        {
            get { return new Vector4(positionV3, 1.0f); }
        }

        /// <summary>
        /// This vector contains the delta increment for yaw,
        /// pitch and roll values in its components, in radians.
        /// </summary>
        public Vector3 RotationDelta
        {
            get { return rotationAngles; }
            set { rotationAngles = value; }
        }

        public Vector3 ScalingValues
        {
            get { return scalingValues; }
            set { scalingValues = value; }
        }

        public Quaternion CurrentRotation { get; set; }

        #endregion

        #region IRenderable Members
        Mesh IRenderable.Mesh
        {
            get { return meshObject.Mesh; }
        }

        RenderableNode IRenderable.ParentNode
        {
            get { return parentNode; }
            set { parentNode = value; }
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
                    meshObject.Dispose();
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

        Mesh I3dEntity.Mesh
        {
            get { return meshObject.Mesh; }
        }

        #endregion

    }
}