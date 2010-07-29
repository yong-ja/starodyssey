using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public class RenderableNode : SceneNode
    {
        const string nodeTag="RN_";
        static int count;

        IRenderable renderableObject;
        TransformNode tParent;
        private MaterialNode mParent;

        public Matrix CurrentAbsoluteWorldMatrix
        {
            get
            {
                if (tParent == null)
                    return Matrix.Identity;
                else
                    return tParent.AbsoluteWorldMatrix;
            }
        }

        public AbstractMaterial CurrentMaterial
        {
            get { return mParent.Material; }
        }

        protected override void OnParentChanged(object sender, AvengersUtd.Odyssey.Utils.Collections.NodeEventArgs e)
        {
            base.OnParentChanged(sender, e);
            tParent = FindFirstTParentNode<TransformNode>(this);
            mParent = FindFirstTParentNode<MaterialNode>(this);
        }

        public IRenderable RenderableObject
        {
            get { return renderableObject; }
        }

        public RenderableNode(IEntity entityObject) : this(entityObject.RenderableObject)
        {}

        public RenderableNode(IRenderable renderableObject) :
            base(nodeTag + (count++), SceneNodeType.Renderable)
        {
            IsLeaf = true;
            this.renderableObject = renderableObject;
            renderableObject.ParentNode = this;
        }

        public override void Init()
        {
            base.Init();
            if (!renderableObject.Inited)
                renderableObject.Init();
        }

        public override void Update()
        {
            //renderableObject.PositionV3 = new Vector3(tParent.AbsoluteWorldMatrix.M41,
            //                                          tParent.AbsoluteWorldMatrix.M42,
            //                                          tParent.AbsoluteWorldMatrix.M43);

        }

        public void Render()
        {
            //Game.Device.SetTransform(TransformState.World, tParent.AbsoluteWorldMatrix);
            //if (renderableObject.IsVisible && renderableObject.IsInViewFrustum())
            renderableObject.Render();
        }


        protected override object OnClone()
        {
            return new RenderableNode(renderableObject);
        }
    }
}
