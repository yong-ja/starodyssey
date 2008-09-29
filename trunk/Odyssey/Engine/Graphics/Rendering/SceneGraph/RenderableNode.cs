using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public class RenderableNode : SceneNode
    {
        const string nodeTag="RN_";
        static int count;

        IRenderable renderableObject;
        TransformNode tParent;

        public Matrix CurrentAbsoluteWorldMatrix
        {
            get
            {
                return tParent.AbsoluteWorldMatrix;
            }
        }

        protected override void OnParentChanged(object sender, AvengersUtd.Odyssey.Utils.Collections.NodeEventArgs e)
        {
            base.OnParentChanged(sender, e);
            tParent = FindFirstTParentNode<TransformNode>(this);
        }

        public IRenderable RenderableObject
        {
            get { return renderableObject; }
        }

        public RenderableNode(IRenderable renderableObject) : 
            base(renderableObject.Descriptor.Label, SceneNodeType.Renderable)
        {
            IsLeaf = true;
            this.renderableObject = renderableObject;
            renderableObject.ParentNode = this;
        }

        public override void Init()
        {
            renderableObject.Init();
        }

        public override void Update()
        {
            renderableObject.PositionV3 = new Vector3(tParent.AbsoluteWorldMatrix.M41,
                                                      tParent.AbsoluteWorldMatrix.M42,
                                                      tParent.AbsoluteWorldMatrix.M43);
            renderableObject.CurrentRotation = Quaternion.RotationYawPitchRoll(
                tParent.Rotation.X, tParent.Rotation.Y, tParent.Rotation.Z);
        }

        public void Render()
        {
            Game.Device.SetTransform(TransformState.World, tParent.AbsoluteWorldMatrix);
            if (renderableObject.IsInViewFrustum())
                renderableObject.Render();
        }

    }
}
