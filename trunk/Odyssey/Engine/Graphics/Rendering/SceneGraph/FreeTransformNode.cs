using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Utils.Collections;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public class FreeTransformNode : TransformNode
    {
        const string nodeTag = "FtN_";
        static int count;

        RenderableNode rNode;

        public FreeTransformNode()
            : this((count++).ToString())
        { }

        public FreeTransformNode(string label)
            : base(nodeTag + label, true)
        {
        }


        protected override void OnInsertBefore(INode newChild, INode refNode)
        {
            if (ChildrenCount == 1 || !(newChild is RenderableNode))
                throw new InvalidOperationException(Properties.Resources.ERR_FtNodeRequiresRNode);
            base.OnInsertBefore(newChild, refNode);
        }

        protected override void OnInsertAfter(INode newChild, INode refNode)
        {
            if (ChildrenCount == 1 || !(newChild is RenderableNode))
                throw new InvalidOperationException(Properties.Resources.ERR_FtNodeRequiresRNode);
            base.OnInsertBefore(newChild, refNode);
        }

        protected override void OnChildAdded(object sender, NodeEventArgs e)
        {
            base.OnChildAdded(sender, e);
            rNode = e.Node as RenderableNode;
            
        }

        protected override void OnChildRemoved(object sender, NodeEventArgs e)
        {
            base.OnChildRemoved(sender, e);
            rNode = null;
        }

        Vector3 previousRotation = new Vector3(0,1,0);
        public override void UpdateLocalWorldMatrix()
        {
            IRenderable renderableObject = rNode.RenderableObject;
            Matrix mTranslation = Matrix.Translation(renderableObject.PositionV3);
            //Matrix mRotationX = Matrix.RotationX(renderableObject.RotationDelta.X);
            //Matrix mRotationY = Matrix.RotationY(renderableObject.RotationDelta.Y);
            //Matrix mRotationZ = Matrix.RotationZ(renderableObject.RotationDelta.Z);

            //LocalWorldMatrix = mRotationZ * mRotationY * mRotationX * mTranslation;

            float yaw = renderableObject.RotationDelta.X;
            float pitch = renderableObject.RotationDelta.Y;
            float roll = renderableObject.RotationDelta.Z;

            Quaternion qRotationChange = Quaternion.RotationYawPitchRoll(yaw, pitch, roll);
            Quaternion qCurrentRotation = qRotationChange*renderableObject.CurrentRotation;

            Matrix mRotation = Matrix.RotationQuaternion(qCurrentRotation);
            LocalWorldMatrix = mRotation*mTranslation;
            renderableObject.CurrentRotation = qCurrentRotation;

        }
    }
}
