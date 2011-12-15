using System;
using System.Diagnostics.Contracts;
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
        protected Renderer Renderer { get; private set; }
        protected MaterialNode MaterialNode {get; private set; }
        protected InputLayout InputLayout { get; private set; }
        protected IMaterial Material { get; private set; }
        protected EffectTechnique Technique { get; private set; }
        protected EffectPass Pass { get; private set; }

        public RenderableCollection Items { get; internal set; }

        public RenderCommand(Renderer renderer, MaterialNode mNode) : base(CommandType.Render)
        {
            Contract.Requires<NullReferenceException>(renderer != null);
            Contract.Requires<NullReferenceException>(mNode != null);
            Renderer = renderer;
            MaterialNode = mNode;
            Material = MaterialNode.Material;
            Technique = Material.EffectDescription.Technique;
            Pass = Technique.GetPassByIndex(Material.EffectDescription.Pass);
            Items = new RenderableCollection(Material.RenderableCollectionDescription, MaterialNode.SelectDescendants<RenderableNode>());
            InputLayout = new InputLayout(Game.Context.Device, Pass.Description.Signature, Items.Description.InputElements);
            
        }

        public RenderCommand(Renderer renderer,MaterialNode mNode, RenderableCollection sceneNodeCollection)
            : base(CommandType.Render)
        {
            Renderer = renderer;
            Items = sceneNodeCollection;
            MaterialNode = mNode;
            Material = MaterialNode.Material;
            Technique = Material.EffectDescription.Technique;
            Pass = Technique.GetPassByIndex(Material.EffectDescription.Pass);
            InputLayout = new InputLayout(Game.Context.Device, Pass.Description.Signature, Items.Description.InputElements);
        }

        public RenderCommand(MaterialNode mNode, RenderableCollection sceneNodeCollection)
            : this(Game.CurrentRenderer, mNode, sceneNodeCollection)
        {
        }

        public override void Execute()
        {
            PerformRender();
        }

        public virtual void Init()
        {
            Material.InitParameters(Renderer);
        }

        public virtual void PerformRender()
        {
            Game.Context.Device.ImmediateContext.InputAssembler.InputLayout = InputLayout;
           
            Material.ApplyDynamicParameters(Renderer);

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