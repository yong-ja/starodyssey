using System;
using System.Collections.Generic;
using System.Text;
using SlimDX;
using AvengersUtd.Odyssey.Utils.Collections;

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public abstract class TransformNode : SceneNode
    {
        static readonly Vector3 DefaultScaling = new Vector3(1, 1, 1);
        bool isDynamic;
        Matrix localWorldMatrix;
        Matrix absoluteWorldMatrix;
        Quaternion qRotation;
        protected Vector3 position;
        protected Vector3 rotation;
        protected Vector3 scaling;

        #region Events

        static readonly object EventLocalWorldMatrixChanged;

        public event EventHandler LocalWorldMatrixChanged
        {
            add { eventHandlerList.AddHandler(EventLocalWorldMatrixChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventLocalWorldMatrixChanged, value); }
        }

        protected virtual void OnLocalWorldMatrixChanged(object sender, EventArgs e)
        {
            UpdateAbsoluteWorldMatrix();
            
            EventHandler handler = (EventHandler)eventHandlerList[EventLocalWorldMatrixChanged];
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Properties

        public bool IsDynamic
        {
            get { return isDynamic; }
        }



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
                    OnLocalWorldMatrixChanged(this, EventArgs.Empty);
                }
            }
        }

        public Matrix AbsoluteWorldMatrix
        {
            get { return absoluteWorldMatrix; }
        }
        #endregion

        #region Constructors
        static TransformNode()
        {
            EventLocalWorldMatrixChanged = new object();
        }

        protected TransformNode(string label, bool isDynamic) : base(label, SceneNodeType.Transform)
        {
            this.isDynamic = isDynamic;
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
            if (isDynamic)
                UpdateLocalWorldMatrix();
        }

        public void UpdateAbsoluteWorldMatrix()
        {
            TransformNode tNode = TransformNodeParent;

            absoluteWorldMatrix = (tNode == null) ? localWorldMatrix : localWorldMatrix * tNode.AbsoluteWorldMatrix;
        }

        public abstract void UpdateLocalWorldMatrix();

    }
}
