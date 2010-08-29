using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Utils.Collections;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public class FreeTransformNode : TransformNode
    {
        const string nodeTag = "FtN_";
        static int count;

        public RenderableNode LeadNode { get; set; }

        public FreeTransformNode()
            : base(nodeTag + (++count), true)
        {
        }


        public override void Init()
        {
            Inited = true;
            if (LeadNode == null)
                foreach (SceneNode sNode in ChildrenIterator)
                {
                    RenderableNode rNode = sNode as RenderableNode;
                    if (rNode == null) continue;

                    LeadNode = rNode;
                    UpdateLocalWorldMatrix();
                    return;
                }

            else
            {
                UpdateLocalWorldMatrix();
                return;
            }

            throw Error.InvalidOperation(Properties.Resources.ERR_FtNodeRequiresRNode);
        }

        public override void UpdateLocalWorldMatrix()
        {
            IRenderable renderableObject = LeadNode.RenderableObject;

            Matrix mTemp = Matrix.Identity;
            //if (renderableObject.RotationDelta != Vector3.Zero)
            //{

            //    float yaw = renderableObject.RotationDelta.X;
            //    float pitch = renderableObject.RotationDelta.Y;
            //    float roll = renderableObject.RotationDelta.Z;

            //    Quaternion qRotationChange = Quaternion.RotationYawPitchRoll(yaw, pitch, roll);
            //    Quaternion qCurrentRotation = qRotationChange * renderableObject.CurrentRotation;

            //    Matrix mRotation = Matrix.RotationQuaternion(qCurrentRotation);
            //    mTemp *= mRotation;
            //    renderableObject.CurrentRotation = qCurrentRotation;
            //    renderableObject.RotationDelta = Vector3.Zero;
            //}
            //else
            //    mTemp *= Matrix.RotationQuaternion(renderableObject.CurrentRotation);

            Matrix mTranslation = Matrix.Translation(renderableObject.PositionV3);
            mTemp *= mTranslation;

            LocalWorldMatrix = mTemp;
            
        }

    }
}