using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Utils.Collections;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public sealed class FixedNode : TransformNode
    {
        static int count;
        const string nodeTag = "FN_";
        

        

        #region Constructors
        public FixedNode()
            : this(nodeTag + (count++))
        {
        }

        public FixedNode(string label)
            : base(label, false)
        {}
        #endregion

        public override void Init()
        {
            base.Init();
            UpdateLocalWorldMatrix();
        }

        protected override object OnClone()
        {
            FixedNode fNode = new FixedNode {LocalWorldMatrix = this.LocalWorldMatrix};
            return fNode;

        }

        public override void UpdateLocalWorldMatrix()
        {
            float yaw = rotation.X;
            float pitch = rotation.Y;
            float roll = rotation.Z;

            Matrix mRotation = Matrix.RotationYawPitchRoll(yaw, pitch, roll);
            Matrix mTranslation = Matrix.Translation(position);
            Matrix mScaling = Matrix.Scaling(scaling);
            LocalWorldMatrix = mRotation*mTranslation *mScaling;
        }

        protected override void OnParentChanged(object sender, NodeEventArgs e)
        {
            base.OnParentChanged(sender, e);
            UpdateAbsoluteWorldMatrix();
        }

    }
}
