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
        static readonly Vector3 DefaultScaling = new Vector3(1, 1, 1);

        Vector3 position;
        Vector3 rotation;
        Vector3 scaling;

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

        #region Constructors
        public FixedNode()
            : base((count++).ToString(), false)
        {
            scaling = DefaultScaling;
        }
        #endregion

        public override void Init()
        {
            base.Init();
            UpdateLocalWorldMatrix();
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
