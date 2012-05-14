﻿#region #Disclaimer

// /* 
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  * 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */

#endregion

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Buffer = SlimDX.Direct3D11.Buffer;
using AvengersUtd.Odyssey.Graphics.Materials;

#endregion

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public enum MouseEventType
    {
        MouseDown,
        MouseUp,
        MouseClick,
        MouseMove
    }

    public abstract class BaseMesh<TVertex> : IMesh
        where TVertex : struct, IPositionVertex
    {
        private readonly EventHandlerList eventHandlerList;
        private readonly List<ShaderResourceView> shaderResources;
        private readonly List<string> behaviours;
        private IMouseBehaviour mouseBehaviour;
        private IGamepadBehaviour gamepadBehaviour;

        private Quaternion qRotation;
        private Vector3 rotationAngles;
        private Vector3 vPosition;

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

        protected virtual void OnCollision(CollisionEventArgs e)
        {
            EventHandler<CollisionEventArgs> handler =
                (EventHandler<CollisionEventArgs>) eventHandlerList[EventCollision];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnPositionChanged(PositionEventArgs e)
        {
           Translation = Matrix.Translation(vPosition);

            EventHandler<PositionEventArgs> handler =
                (EventHandler<PositionEventArgs>) eventHandlerList[EventPositionChanged];
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
                (EventHandler<PositionEventArgs>) eventHandlerList[EventPositionChanging];
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
                (EventHandler<RotationEventArgs>) eventHandlerList[EventRotationChanged];
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
            EventHandler<EventArgs> handler = (EventHandler<EventArgs>) eventHandlerList[EventRotationChanged];
            if (handler != null)
                handler(this, e);
        }


        #endregion

        #region Properties
        public string Name { get; set; }

        protected TVertex[] Vertices { get; set; }
        protected ushort[] Indices { get; set; }
        protected CpuAccessFlags CpuAccessFlags { get; set; }
        protected ResourceUsage ResourceUsage { get; set; }

        protected List<ShaderResourceView> ShaderResourceList
        {
            get { return shaderResources; }
        }

        public Format IndexFormat { get; private set; }
        public int VertexCount { get { return Vertices.Length; }}
        public int IndexCount { get { return Indices.Length; } }
        public int TotalFaces { get { return IndexCount / 3; } }
        public Buffer VertexBuffer { get; protected set; }
        public Buffer IndexBuffer { get; protected set; }

        public ShaderResourceView[] ShaderResources
        {
            get { return shaderResources.ToArray(); }
        }

        public bool Inited { get; private set; }
        public bool IsCollidable { get; private set; }
        public bool IsVisible { get; private set; }
        public bool CastsShadows { get; private set; }
        public bool Disposed { get; private set; }

        public VertexDescription VertexDescription { get; internal set; }

        public Matrix World { get; set; }
        public Matrix Translation { get; private set; }
        public Matrix Rotation { get; private set; }
        public Vector3 ScalingValues { get; set; }
        public Vector4 PositionV4
        {
            get { return vPosition.ToVector4(); }
        }
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

        public Vector3 AbsolutePosition
        {
            get { return new Vector3(World[3, 0], World[3, 1], World[3, 2]); }
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

        public IColorMaterial Material { get; set; }


        #endregion

        #region Constructors

        static BaseMesh()
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

        protected BaseMesh(VertexDescription vertexDescription)
        {
            Name = GetType().Name.ToString();
            eventHandlerList = new EventHandlerList();
            CastsShadows = false;
            IsCollidable = false;
            IsVisible = true;
            shaderResources = new List<ShaderResourceView>();
            IndexFormat = DeviceContext11.DefaultIndexFormat;
            behaviours = new List<string>();
            
            CpuAccessFlags = CpuAccessFlags.None;
            ResourceUsage = ResourceUsage.Default;
            VertexDescription = vertexDescription;

            World = Matrix.Identity;
            CurrentRotation = Quaternion.Identity;
        }

        #endregion

        #region IRenderable Members

        public RenderableNode ParentNode { get; set; }

        public virtual void Init()
        {
            DataStream stream = new DataStream(VertexCount * VertexDescription.Stride, true, true);
            stream.WriteRange(Vertices);

            stream.Position = 0;
            
            VertexBuffer = new Buffer(Game.Context.Device, stream, new BufferDescription
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = Vertices.Length * VertexDescription.Stride,
                Usage = ResourceUsage,
            }) {Tag = Name + "_VB"};

            stream.Close();

            stream = new DataStream(IndexCount * sizeof(ushort), true, true);
            stream.WriteRange(Indices);
            stream.Position = 0;
            IndexBuffer = new Buffer(Game.Context.Device, stream, new BufferDescription
            {
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = (int)stream.Length,
                Usage =ResourceUsage
            }) { Tag = Name + "_IB" };
            stream.Dispose();

            Game.CurrentRenderer.RegisterBuffer(VertexBuffer);
            Game.CurrentRenderer.RegisterBuffer(IndexBuffer);

            Inited = true;
        }

        public void Rebuild(TVertex[] vertices, ushort[] indices)
        {
            Vertices = vertices;
            Indices = indices;

            VertexBuffer.Dispose();
            IndexBuffer.Dispose();
            Init();
        }

        /// <summary>
        /// Renders the mesh
        /// </summary>
    
        public virtual void Render()
        {
            Render(IndexCount);
        }

        public virtual void Render(int indexCount, int vertexOffset=0, int indexOffset=0, int startIndex=0, int baseVertex=0)
        {
            Game.Context.Device.ImmediateContext.InputAssembler.SetVertexBuffers(
                0, new VertexBufferBinding(VertexBuffer, VertexDescription.Stride, vertexOffset));
            Game.Context.Device.ImmediateContext.InputAssembler.SetIndexBuffer(IndexBuffer, IndexFormat, indexOffset);
            Game.Context.Device.ImmediateContext.DrawIndexed(indexCount, startIndex, baseVertex);
        }

        public bool IsInViewFrustum()
        {
            throw new NotImplementedException();
        }

        public void SetBehaviour(IMouseBehaviour mBehaviour)
        {
            behaviours.Add(mBehaviour.GetType().Name);
            
            MouseDown += mBehaviour.OnMouseDown;
            MouseUp += mBehaviour.OnMouseUp;
            MouseMove += mBehaviour.OnMouseMove;
            MouseClick += mBehaviour.OnMouseClick;
            mouseBehaviour = mBehaviour;
        }

        public void SetBehaviour(IGamepadBehaviour gBehaviour)
        {
            behaviours.Add(gBehaviour.GetType().Name);

            gamepadBehaviour = gBehaviour;
        }

        public void RemoveBehaviour(IGamepadBehaviour gBehaviour)
        {
            behaviours.Remove(gBehaviour.GetType().Name);

            gamepadBehaviour = null;
        }

        public void RemoveBehaviour(IMouseBehaviour mBehaviour)
        {
            behaviours.Remove(mBehaviour.GetType().Name);

            MouseDown -= mBehaviour.OnMouseDown;
            MouseUp -= mBehaviour.OnMouseUp;
            MouseMove -= mBehaviour.OnMouseMove;
            MouseClick -= mBehaviour.OnMouseClick;

            mouseBehaviour = null;
        }

        public bool HasBehaviour(string behaviourName)
        {
            return behaviours.Contains(behaviourName);
        }

        public void ProcessMouseEvent(MouseEventType type, MouseEventArgs e)
        {
            switch (type)
            {
                case MouseEventType.MouseDown:
                    OnMouseDown(e);
                    break;
                case MouseEventType.MouseUp:
                    OnMouseUp(e);
                    break;
                case MouseEventType.MouseClick:
                    OnMouseClick(e);
                    break;
                case MouseEventType.MouseMove:
                    OnMouseMove(e);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }


        #endregion

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
                    if (VertexBuffer!=null)
                        VertexBuffer.Dispose();
                    if (IndexBuffer!= null)
                        IndexBuffer.Dispose();
                    OnDisposing(EventArgs.Empty);
                }
            }
            Disposed = true;
        }

        ~BaseMesh()
        {
            Dispose(false);
        } 
        #endregion

        public override string ToString()
        {
            return Name;
        }


    }
}