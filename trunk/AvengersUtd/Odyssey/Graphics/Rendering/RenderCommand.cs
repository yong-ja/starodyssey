using System;
using System.Linq;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class RenderCommand : BaseCommand, IRenderCommand
    {
        protected MaterialNode MaterialNode {get; private set; }
        protected InputLayout InputLayout { get; private set; }
        protected AbstractMaterial Material { get; private set; }
        protected EffectTechnique Technique { get; private set; }
        protected EffectPass Pass { get; private set; }

        public RenderableCollection Items { get; internal set; }

        public RenderCommand(MaterialNode mNode, RenderableCollection sceneNodeCollection)
            : base(CommandType.Render)
        {
            Items = sceneNodeCollection;
            MaterialNode = mNode;
            Material = MaterialNode.Material;
            Technique = Material.EffectDescription.Technique;
            Pass = Technique.GetPassByIndex(Material.EffectDescription.Pass);
            InputLayout = new InputLayout(Game.Context.Device, Pass.Description.Signature, Items.Description.InputElements);
        }

        public override void Execute()
        {
            PerformRender();
        }

        public virtual void PerformRender()
        {
            Game.Context.Device.ImmediateContext.InputAssembler.InputLayout = InputLayout;
           
            Material.ApplyDynamicParameters();

            foreach (RenderableNode rNode in Items)
            {
                rNode.Update();
                if (!Items.Description.CommonResources)
                    Material.ApplyInstanceParameters(rNode.RenderableObject);

                if (!rNode.RenderableObject.IsVisible) continue;

                Pass.Apply(Game.Context.Device.ImmediateContext);

                IRenderable rObject = rNode.RenderableObject;
                rObject.Render();
            }
        }

        protected override void OnDispose()
        {
            if (!InputLayout.Disposed)
                InputLayout.Dispose();

            foreach (RenderableNode rNode in Items.Where(rNode => !rNode.RenderableObject.Disposed))
            {
                rNode.RenderableObject.Dispose();
            }
        }

        public virtual void UpdateItems()
        {
            return;
        }
    }
}