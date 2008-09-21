using System;
using System.Collections.Generic;
using System.Text;
using SlimDX;
using AvengersUtd.Odyssey.Utils.Collections;

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public abstract class TransformNode : SceneNode
    {
        bool isDynamic;
        Matrix localWorldMatrix;
        Matrix absoluteWorldMatrix;
        Quaternion rotation;

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
        }
        #endregion

        public override void Init()
        {
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
