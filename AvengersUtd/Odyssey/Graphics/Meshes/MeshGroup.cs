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
using AvengersUtd.Odyssey.UserInterface.Controls;
using MouseEventArgs = AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public abstract class MeshGroup : IRenderable
    {
        protected IRenderable[] Objects {get; private set;}
        Vector3 vPosition;
        Vector3 vRotationCenter;
        private Quaternion qRotation;
        private Vector3 rotationAngles;
        private Vector3 vScale;
        private readonly EventHandlerList eventHandlerList;
        readonly Dictionary<string, IBehaviour> inputBehaviours;

        #region Events

        private static readonly object EventPositionChanged;
        private static readonly object EventPositionChanging;
        private static readonly object EventCollision;
        private static readonly object EventRotationChanged;
        private static readonly object EventDisposing;
        private static readonly object EventMouseClick;
        private static readonly object EventMouseDown;
        private static readonly object EventMouseMove;
        private static readonly object EventMouseUp;
        private static readonly object EventKeyPress;
        private static readonly object EventKeyDown;
        private static readonly object EventKeyUp;

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

            if (Objects!= null && Objects.Length>0)
                Objects.ToList().ForEach(rObj => rObj.CurrentRotation = e.NewRotation);

            EventHandler<RotationEventArgs> handler =
                (EventHandler<RotationEventArgs>)eventHandlerList[EventRotationChanged];
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<ScaleEventArgs> ScaleChanged;

        protected virtual void OnScaleChanged(ScaleEventArgs e)
        {
            Scaling = Matrix.Scaling(e.NewValue);
            if (ScaleChanged != null)
                ScaleChanged(this, e);
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
        public event EventHandler<MouseEventArgs> MouseDown
        {
            add { eventHandlerList.AddHandler(EventMouseDown, value); }
            remove { eventHandlerList.RemoveHandler(EventMouseDown, value); }
        }

        public event EventHandler<MouseEventArgs> MouseUp
        {
            add { eventHandlerList.AddHandler(EventMouseUp, value); }
            remove { eventHandlerList.RemoveHandler(EventMouseUp, value); }
        }

        public event EventHandler<MouseEventArgs> MouseClick
        {
            add { eventHandlerList.AddHandler(EventMouseClick, value); }
            remove { eventHandlerList.RemoveHandler(EventMouseClick, value); }
        }

        public event EventHandler<MouseEventArgs> MouseMove
        {
            add { eventHandlerList.AddHandler(EventMouseMove, value); }
            remove { eventHandlerList.RemoveHandler(EventMouseMove, value); }
        }

        public event KeyEventHandler KeyDown
        {
            add { eventHandlerList.AddHandler(EventKeyDown, value); }
            remove { eventHandlerList.RemoveHandler(EventKeyDown, value); }
        }

        public event KeyEventHandler KeyUp
        {
            add { eventHandlerList.AddHandler(EventKeyUp, value); }
            remove { eventHandlerList.RemoveHandler(EventKeyUp, value); }
        }

        public event KeyEventHandler KeyPress
        {
            add { eventHandlerList.AddHandler(EventKeyPress, value); }
            remove { eventHandlerList.RemoveHandler(EventKeyPress, value); }
        }

        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            EventHandler<MouseEventArgs> handler =
                (EventHandler<MouseEventArgs>)eventHandlerList[EventMouseDown];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            EventHandler<MouseEventArgs> handler =
                (EventHandler<MouseEventArgs>)eventHandlerList[EventMouseUp];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnMouseClick(MouseEventArgs e)
        {
            EventHandler<MouseEventArgs> handler =
                (EventHandler<MouseEventArgs>)eventHandlerList[EventMouseClick];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnMouseMove(MouseEventArgs e)
        {
            EventHandler<MouseEventArgs> handler =
                (EventHandler<MouseEventArgs>)eventHandlerList[EventMouseMove];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnKeyDown(KeyEventArgs e)
        {
            KeyEventHandler handler = (KeyEventHandler)eventHandlerList[EventKeyDown];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnKeyUp(KeyEventArgs e)
        {
            KeyEventHandler handler = (KeyEventHandler)eventHandlerList[EventKeyUp];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnKeyPress(KeyEventArgs e)
        {
            KeyEventHandler handler = (KeyEventHandler)eventHandlerList[EventKeyPress];
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
        bool IRenderable.IsMeshGroup { get { return true; } }
        public Matrix World { get; set; }
        public Matrix Translation { get; private set; }
        public Matrix Rotation { get; private set; }
        public Matrix Scaling { get; private set; }

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

        public Vector3 RotationCenter
        {
            get { return vRotationCenter; }
            set { vRotationCenter = value; }
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
                RotationEventArgs e = new RotationEventArgs(value, rotationAngles, qRotation);

                if (!e.CancelEvent && qRotation != value)
                {
                    qRotation = value;
                    OnRotationChanged(e);
                }
            }
        }

        public Vector3 ScalingValues
        {
            get { return vScale; }
            set
            {
                if (vScale == value) return;
                ScaleEventArgs e = new ScaleEventArgs(value, vScale);
                //OnScaleChangin
                if (e.CancelEvent || vScale == value) return;

                vScale = value;
                OnScaleChanged(e);

            }
        }

        public RenderableNode ParentNode { get; set; }

        public IMaterial Material
        {
            get;  set;
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
            EventMouseDown = new object();
            EventMouseUp = new object();
            EventMouseClick = new object();
            EventMouseMove = new object();
        }

        protected MeshGroup(int size)
        {
            Name = GetType().Name;
            eventHandlerList = new EventHandlerList();
            inputBehaviours = new Dictionary<string, IBehaviour>();
            CastsShadows = false;
            IsCollidable = false;
            IsVisible = true;

            ScalingValues = new Vector3(1f, 1f, 1f);
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

        public void SetBehaviour<T>(T inputBehaviour) where T : class,UserInterface.Controls.IBehaviour
        {
            throw new NotImplementedException();
        }

        public bool RemoveBehaviour<T>(T inputBehaviour) where T : class,UserInterface.Controls.IBehaviour
        {
            throw new NotImplementedException();
        }

        public bool HasBehaviour(string behaviourName)
        {
            return inputBehaviours.ContainsKey(behaviourName);
        }

        public T GetBehaviour<T>() where T : class,UserInterface.Controls.IBehaviour
        {
            throw new NotImplementedException();
        }

        public void ProcessMouseEvent(MouseEventType type, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }


        public void Move(float distance, Vector3 direction, float frameTime=1)
        {
            Objects.ToList().ForEach(rObj => rObj.Move(distance, direction, frameTime));
        }

        public void Rotate(float angle, Vector3 axis, float frameTime=1)
        {
            Objects.ToList().ForEach(rObj => rObj.Rotate(angle, axis,frameTime));
        }

        public virtual IEnumerable<RenderableNode> ToNodes()
        {
            RenderableNode[] nodes = new RenderableNode[Objects.Length];
            for (int i = 0; i < Objects.Length; i++)
                nodes[i] = new RenderableNode(Objects[i]);

            return nodes;
        }

        public FixedNode ToBranch()
        {
            FixedNode fNode = new FixedNode() { Position = PositionV3,
            Rotation = CurrentRotation, RotationCenter = RotationCenter};
            PositionV3 = Vector3.Zero;
            foreach (IRenderable rObject in Objects)
            {
                if (rObject.IsMeshGroup)
                    fNode.AppendChild(rObject.ToBranch());
                else
                {
                    //rObject.PositionV3 = Vector3.Zero;
                    fNode.AppendChild(new RenderableNode(rObject));
                }
            }
            return fNode;

        }
    }
}
