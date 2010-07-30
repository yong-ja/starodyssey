using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class RenderCommand : BaseCommand
    {
        private readonly MaterialNode materialNode;
        public SceneNodeCollection<RenderableNode> Items { get; internal set; }

        public RenderCommand(MaterialNode mNode, SceneNodeCollection<RenderableNode> SceneNodeCollection)
            : base(CommandType.RenderScene)
        {
            Items = SceneNodeCollection;
            materialNode = mNode;
        }

        public override void Execute()
        {
            PerformRender();
        }

        public virtual void PerformRender()
        {
            AbstractMaterial material = materialNode.Material;
            EffectTechnique technique = material.EffectDescriptor.Technique;

            EffectPass pass = technique.GetPassByIndex(material.EffectDescriptor.Pass);
            RenderForm11.Device.ImmediateContext.InputAssembler.InputLayout =
                new InputLayout(RenderForm11.Device,
                                pass.Description.Signature,
                                TexturedVertex.InputElements);

            pass.Apply(RenderForm11.Device.ImmediateContext);
            material.ApplyDynamicParameters();

            foreach (SceneNode node in Items)
            {
                node.Update();
                RenderableNode rNode = node as RenderableNode;
                if (rNode != null && rNode.RenderableObject.IsVisible)
                {
                    IRenderable rObject = rNode.RenderableObject;

                    RenderForm11.Device.ImmediateContext.InputAssembler.SetVertexBuffers(
                        0,
                        new VertexBufferBinding(rObject.Vertices, rObject.VertexDescription.Stride, 0));

                    RenderForm11.Device.ImmediateContext.InputAssembler.SetIndexBuffer(
                        rObject.Indices, Format.R16_UInt, 0);

                    material.ApplyInstanceParameters(rObject);

                    rObject.Render();
                }
            }
        }

        protected override void OnDispose()
        {
        }
    }
}