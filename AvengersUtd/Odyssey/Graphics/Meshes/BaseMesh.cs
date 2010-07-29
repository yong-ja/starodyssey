using System;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Materials;
using SlimDX;
using Buffer = SlimDX.Direct3D11.Buffer;
using System.ComponentModel;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using AvengersUtd.Odyssey.Resources;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public abstract class BaseMesh : IRenderable
    {
        private Matrix mWorld;
        private Vector4 vPosition;
        private Vector3 rotationAngles;
        private Vector3 scalingValues;
        private Quaternion qRotation;

        private Buffer vertices;
        private Buffer indices;
        private bool disposed;
        EventHandlerList eventHandlerList;


        #region Events
        static readonly object EventPositionChanged;
        static readonly object EventPositionChanging;
        static readonly object EventCollision;
        static readonly object EventRotationChanged;

        public event EventHandler<CollisionEventArgs> Collision
        {
            add { eventHandlerList.AddHandler(EventCollision, value); }
            remove { eventHandlerList.RemoveHandler(EventCollision, value); }
        }

        public event EventHandler<PositionEventArgs> PositionChanged
        {
            add { eventHandlerList.AddHandler(EventPositionChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventPositionChanged, value); }
        }

        protected virtual void OnCollision(CollisionEventArgs e)
        {
            EventHandler<CollisionEventArgs> handler = (EventHandler<CollisionEventArgs>)eventHandlerList[EventCollision];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnPositionChanged(PositionEventArgs e)
        {
            mWorld = Matrix.Translation(vPosition.ToVector3());

            EventHandler<PositionEventArgs> handler = (EventHandler<PositionEventArgs>)eventHandlerList[EventPositionChanged];
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<PositionEventArgs> PositionChanging
        {
            add { eventHandlerList.AddHandler(EventPositionChanging, value); }
            remove { eventHandlerList.RemoveHandler(EventPositionChanging, value); }
        }

        protected virtual void OnPositionChanging(PositionEventArgs e)
        {
            EventHandler<PositionEventArgs> handler = (EventHandler<PositionEventArgs>)eventHandlerList[EventPositionChanging];
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<RotationEventArgs> RotationChanged
        {
            add { eventHandlerList.AddHandler(EventRotationChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventRotationChanged, value); }
        }

        protected virtual void OnRotationChanged(RotationEventArgs e)
        {
            EventHandler<RotationEventArgs> handler = (EventHandler<RotationEventArgs>)eventHandlerList[EventRotationChanged];
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region Properties

        
        public bool Inited { get; private set; }

        public bool IsCollidable
        { get; private set; }

        public bool IsVisible
        { get; private set; }

        public bool CastsShadows
        { get; private set; }

        public VertexDescription VertexDescription { get; private set; }

        public int IndexCount { get; private set; }

        public Matrix World
        {
            get { return mWorld; }
            set { mWorld = value; }
        }


        public Vector3 PositionV3
        { get { return vPosition.ToVector3(); } }

        public Vector4 PositionV4
        {
            get { return vPosition; }
            set {
                if (vPosition != value)
                {
                    PositionEventArgs e = new PositionEventArgs(vPosition, value);
                    OnPositionChanging(e);
                    if (!e.CancelEvent && vPosition != e.NewValue)
                    {
                        vPosition = e.NewValue;
                        OnPositionChanged(e);
                    }
                }
            }
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

        public Quaternion CurrentRotation
        {
            get
            {
                return qRotation;
            }
            set
            {
                RotationEventArgs e = new RotationEventArgs(qRotation, rotationAngles);

                if (!e.CancelEvent && qRotation != value)
                {
                    OnRotationChanged(e);
                    qRotation = value;
                }
            }
         }

        public Buffer Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        public Buffer Indices
        {
            get { return indices; }
            set { indices = value; }
        }

        #endregion

        #region Constructors
        static BaseMesh()
        {
            EventCollision = new object();
            EventPositionChanging = new object();
            EventPositionChanged = new object();
            EventRotationChanged = new object();
        }

        protected BaseMesh(Buffer vertices, Buffer indices, int indexCount, VertexDescription vDescription)
        {
            eventHandlerList = new EventHandlerList();
            CastsShadows = false;
            IsCollidable = false;
            IsVisible = true;
            this.vertices = vertices;
            this.indices = indices;
            IndexCount = indexCount;
            VertexDescription = vDescription;

            mWorld = Matrix.Identity;
        } 
	#endregion

        public RenderableNode ParentNode { get; set; }

        public virtual void Init()
        {
        }

        public abstract void Render();

        public void UpdatePosition()
        {
            throw new NotImplementedException();
        }

        public void CollideWith(IRenderable collidedObject)
        {
            throw new NotImplementedException();
        }

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
                    vertices.Dispose();
                    indices.Dispose();
                }
            }
            disposed = true;
        }

        ~BaseMesh()
        {
            Dispose(false);
        }

        #endregion


        public bool IsInViewFrustum()
        {
            throw new NotImplementedException();
        }
    }
}
