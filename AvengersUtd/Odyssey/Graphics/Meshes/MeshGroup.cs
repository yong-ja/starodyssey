using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SlimDX;
using AvengersUtd.Odyssey.Geometry;
using System.ComponentModel;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public abstract class MeshGroup : IRenderable
    {
        protected IRenderable[] Objects {get; private set;}
        Vector3 vPosition;
        private Quaternion qRotation;
        private Vector3 rotationAngles;
        private readonly EventHandlerList eventHandlerList;

        #region Events

        private static readonly object EventPositionChanged;
        private static readonly object EventPositionChanging;
        private static readonly object EventCollision;
        private static readonly object EventRotationChanged;
        private static readonly object EventDisposing;

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
            EventHandler<CollisionEventArgs> handler =
                (EventHandler<CollisionEventArgs>)eventHandlerList[EventCollision];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnPositionChanged(PositionEventArgs e)
        {
            Translation = Matrix.Translation(vPosition);

            EventHandler<PositionEventArgs> handler =
                (EventHandler<PositionEventArgs>)eventHandlerList[EventPositionChanged];
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
            EventHandler<PositionEventArgs> handler =
                (EventHandler<PositionEventArgs>)eventHandlerList[EventPositionChanging];
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
            Rotation = Matrix.RotationQuaternion(CurrentRotation);

            EventHandler<RotationEventArgs> handler =
                (EventHandler<RotationEventArgs>)eventHandlerList[EventRotationChanged];
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<EventArgs> Disposing
        {
            add { eventHandlerList.AddHandler(EventDisposing, value); }
            remove { eventHandlerList.RemoveHandler(EventDisposing, value); }
        }

        protected virtual void OnDisposing(EventArgs e)
        {
            EventHandler<EventArgs> handler = (EventHandler<EventArgs>)eventHandlerList[EventRotationChanged];
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Properties
        public string Name {get; private set;}
        public bool Inited { get; private set; }
        public bool IsCollidable { get; private set; }
        public bool IsVisible { get; private set; }
        public bool CastsShadows { get; private set; }
        public bool Disposed { get; private set; }
        public Matrix World { get; set; }
        public Matrix Translation { get; private set; }
        public Matrix Rotation { get; private set; }

        public Vector3 PositionV3
        {
            get { return vPosition; }
            set
            {
                if (vPosition == value) return;

                PositionEventArgs e = new PositionEventArgs(vPosition, value);
                OnPositionChanging(e);

                if (e.CancelEvent || vPosition == e.NewValue) return;
                vPosition = e.NewValue;
                OnPositionChanged(e);
            }
        }

        public Vector4 PositionV4
        {
            get { return vPosition.ToVector4(); }
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

        public Quaternion CurrentRotation
        {
            get { return qRotation; }
            set
            {
                if (qRotation == value) return;
                RotationEventArgs e = new RotationEventArgs(qRotation, rotationAngles);

                if (!e.CancelEvent && qRotation != value)
                {
                    qRotation = value;
                    OnRotationChanged(e);
                }
            }
        }

        public RenderableNode ParentNode { get; set; }

        public IColorMaterial Material
        {
            get; protected set;
        }
        #endregion

        #region Constructors

        static MeshGroup()
        {
            EventCollision = new object();
            EventPositionChanging = new object();
            EventPositionChanged = new object();
            EventRotationChanged = new object();
            EventDisposing = new object();
        }

        protected MeshGroup(int size)
        {
            Name = GetType().Name;
            eventHandlerList = new EventHandlerList();
            CastsShadows = false;
            IsCollidable = false;
            IsVisible = true;

            World = Matrix.Identity;
            CurrentRotation = Quaternion.Identity;
            Objects = new IRenderable[size];
        }

        #endregion

        public void Init()
        {
            foreach (IRenderable renderable in Objects)
                renderable.Init();

            Inited = true;
        }

        public void Render()
        {
            foreach (IRenderable renderable in Objects)
                renderable.Render();
        }

        public void Render(int indexCount, int vertexOffset = 0, int indexOffet = 0, int startIndex = 0, int baseVertex = 0)
        {
            foreach (IRenderable renderable in Objects)
                renderable.Render();
        }

      
        #region IDisposable members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    // dispose managed components
                    foreach (IRenderable renderable in Objects)
                        renderable.Dispose();
                    OnDisposing(EventArgs.Empty);
                }
            }
            Disposed = true;
        }

        ~MeshGroup()
        {
            Dispose(false);
        } 
        #endregion

        public bool IsInViewFrustum()
        {
            throw new NotImplementedException();
        }

        public Vector3 AbsolutePosition
        {
            get { throw new NotImplementedException(); }
        }

        public void SetBehaviour(UserInterface.Controls.IMouseBehaviour mouseBehaviour)
        {
            throw new NotImplementedException();
        }

        public void RemoveBehaviour(UserInterface.Controls.IMouseBehaviour mouseBehaviour)
        {
            throw new NotImplementedException();
        }

        public void ProcessMouseEvent(MouseEventType type, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public bool HasBehaviour(string behaviourName)
        {
            throw new NotImplementedException();
        }


        public void SetBehaviour(UserInterface.Controls.IGamepadBehaviour gBehaviour)
        {
            throw new NotImplementedException();
        }

        public void RemoveBehaviour(UserInterface.Controls.IGamepadBehaviour gamepadBehaviour)
        {
            throw new NotImplementedException();
        }
    }
}
