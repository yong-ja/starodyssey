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
        private InputLayout inputLayout;
        private AbstractMaterial material;
        private EffectTechnique technique;
        private EffectPass pass;

        public RenderableCollection Items { get; internal set; }



        public RenderCommand(MaterialNode mNode, RenderableCollection sceneNodeCollection)
            : base(CommandType.RenderScene)
        {
            Items = sceneNodeCollection;
            materialNode = mNode;
            material = materialNode.Material;
            technique = material.EffectDescription.Technique;
            pass = technique.GetPassByIndex(material.EffectDescription.Pass);
            inputLayout = new InputLayout(RenderForm11.Device, pass.Description.Signature, Items.Description.InputElements);
        }

        public override void Execute()
        {
            PerformRender();
        }

        public virtual void PerformRender()
        {

            RenderForm11.Device.ImmediateContext.InputAssembler.InputLayout = inputLayout;
           
            material.ApplyDynamicParameters();

            foreach (RenderableNode rNode in Items)
            {
                rNode.Update();
                if (!Items.Description.CommonTexture)
                    material.ApplyInstanceParameters(rNode.RenderableObject);
                
                if (rNode.RenderableObject.IsVisible)
                {
                    pass.Apply(RenderForm11.Device.ImmediateContext);

                    IRenderable rObject = rNode.RenderableObject;

                    RenderForm11.Device.ImmediateContext.InputAssembler.SetVertexBuffers(
                        0,
                        new VertexBufferBinding(rObject.Vertices, rObject.VertexDescription.Stride, 0));

                    RenderForm11.Device.ImmediateContext.InputAssembler.SetIndexBuffer(
                        rObject.Indices, Items.Description.IndexFormat, 0);


                    rObject.Render();
                }
            }
        }

        protected override void OnDispose()
        {
        }
    }
}