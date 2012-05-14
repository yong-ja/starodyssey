using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.Utils;
using SlimDX;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public class RenderableNode : SceneNode
    {
        static int count;

        TransformNode tParent;
        private MaterialNode mParent;

        public Matrix CurrentAbsoluteWorldMatrix
        {
            get { return tParent == null ? Matrix.Identity : tParent.AbsoluteWorldMatrix; }
        }

        public IMaterial CurrentMaterial
        {
            get { return mParent.Material; }
        }

        protected override void OnParentChanged(object sender, Utils.Collections.NodeEventArgs e)
        {
            base.OnParentChanged(sender, e);
            tParent = FindFirstTParentNode<TransformNode>(this);
            mParent = FindFirstTParentNode<MaterialNode>(this);
        }

        public IRenderable RenderableObject { get; private set; }

        public RenderableNode(ISpriteObject entityObject) : this(entityObject.RenderableObject)
        {}

        public RenderableNode(IRenderable renderableObject)
            : base(Text.GetCapitalLetters(typeof(RenderableNode).GetType().Name) + '_' + ++count,  SceneNodeType.Renderable)
        {
            IsLeaf = true;
            this.RenderableObject = renderableObject;
            renderableObject.ParentNode = this;
        }

        public override void Init()
        {
            base.Init();
            if (!RenderableObject.Inited)
                RenderableObject.Init();
        }

        public override void Update()
        {
            
            tParent.Update();
            Matrix objectMatrix = Matrix.Identity;
            bool multiply = false;
            if (RenderableObject.CurrentRotation != Quaternion.Identity)
            {
                objectMatrix *= RenderableObject.Rotation;
                multiply = true;
            }

            if (RenderableObject.HasBehaviour(typeof(KeyboardBehaviour).Name))
            {
                KeyboardBehaviour kBehaviour = RenderableObject.GetBehaviour<KeyboardBehaviour>();
                kBehaviour.Update();
            }


            if (RenderableObject.PositionV3 != Vector3.Zero)
            {
                objectMatrix *= RenderableObject.Translation;
                multiply = true;
            }

            RenderableObject.World = multiply ? objectMatrix*tParent.AbsoluteWorldMatrix : tParent.AbsoluteWorldMatrix;


            //RenderableObject.PositionV3 = new Vector3(tParent.AbsoluteWorldMatrix.M41,
            //                                  tParent.AbsoluteWorldMatrix.M42,
            //                                  tParent.AbsoluteWorldMatrix.M43);

        }

        public void Render()
        {
            RenderableObject.Render();
        }

    }
}
