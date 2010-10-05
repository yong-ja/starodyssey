﻿using System;
using System.Collections.Generic;
using System.Text;
using SlimDX;
using AvengersUtd.Odyssey.Utils.Collections;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public abstract class TransformNode : SceneNode
    {
        internal static readonly Vector3 DefaultScaling = new Vector3(1, 1, 1);
        Matrix localWorldMatrix;
        Quaternion qRotation;
        private Vector3 position;
        private Vector3 rotation;
        private Vector3 scaling;

        #region Events

        static readonly object EventLocalWorldMatrixChanged;

        public event EventHandler LocalWorldMatrixChanged
        {
            add { EventHandlerList.AddHandler(EventLocalWorldMatrixChanged, value); }
            remove { EventHandlerList.RemoveHandler(EventLocalWorldMatrixChanged, value); }
        }

        protected virtual void OnLocalWorldMatrixChanged(EventArgs e)
        {
            UpdateAbsoluteWorldMatrix();
            
            EventHandler handler = (EventHandler)EventHandlerList[EventLocalWorldMatrixChanged];
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Properties

        public bool IsDynamic { get; internal set; }

        public Vector3 Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    UpdateLocalWorldMatrix();
                }
            }
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                if (rotation != value)
                {
                    rotation = value;
                    UpdateLocalWorldMatrix();
                }
            }
        }

        public Vector3 Scaling
        {
            get { return scaling; }
            set
            {
                if (scaling != value)
                {
                    scaling = value;
                    UpdateLocalWorldMatrix();
                }
            }
        }

        /// <summary>
        /// Returns the first TransformNode node that is a parent of this node.
        /// </summary>
        protected TransformNode TransformNodeParent
        {
            get { return FindFirstTParentNode<TransformNode>(Parent); }
        }

        public Matrix LocalWorldMatrix
        {
            get { return localWorldMatrix; }
            protected set
            {
                if (localWorldMatrix != value)
                {
                    localWorldMatrix = value;
                    OnLocalWorldMatrixChanged(EventArgs.Empty);
                }
            }
        }

        public Matrix AbsoluteWorldMatrix { get; private set; }

        #endregion

        #region Constructors
        static TransformNode()
        {
            EventLocalWorldMatrixChanged = new object();
        }

        protected TransformNode(string label, bool isDynamic) : base(label, SceneNodeType.Transform)
        {
            this.IsDynamic = isDynamic;
            scaling = DefaultScaling;
        }
        #endregion

        public override void Init()
        {
            base.Init();
            UpdateLocalWorldMatrix();
        }

        public override void Update()
        {
            if (IsDynamic)
                UpdateLocalWorldMatrix();
        }

        public void UpdateAbsoluteWorldMatrix()
        {
            TransformNode tNode = TransformNodeParent;

            AbsoluteWorldMatrix = (tNode == null) ? localWorldMatrix : localWorldMatrix * tNode.AbsoluteWorldMatrix;
            Position = new Vector3(AbsoluteWorldMatrix.M41, AbsoluteWorldMatrix.M42, AbsoluteWorldMatrix.M43);
        }

        public abstract void UpdateLocalWorldMatrix();

    }
}