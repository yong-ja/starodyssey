using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using AvengersUtd.Odyssey.Rendering;
using AvengersUtd.Odyssey.Geometry;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using SlimDX.DXGI;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Text;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class TestRenderer : Renderer
    {
        Polygon triangle;
        //private Effect effect;
        //private EffectTechnique technique;
        //private EffectPass pass;

        RenderableNode rNode;
        private RenderableNode rNode1;

   
        public override void Init()
        {
            //AvengersUtd.Odyssey.Text.TextManager.DrawText("prova");
            Camera.Update();
            //triangle = Polygon.CreateTexturedQuad(new Vector4(0f, 0.5f, 0.5f, 1f), 0.5f, 0.5f);
            triangle = Polygon.CreateTexturedQuad(new Vector4(0, 50, 0f, 1f), 105f, 105f);



            FunctionalMaterial texturer = new FunctionalMaterial();
            MaterialNode mNode = new MaterialNode(texturer);
            rNode = new RenderableNode(new TextLiteral(Game.Logger.FrameStats, new Vector3(10f, 500f, 10f)));
            rNode1 = new RenderableNode(new TextLiteral("12345", new Vector3(10f, 150f, 0f)));
            mNode.AppendChild(rNode);
            mNode.AppendChild(rNode1);

            //Scene.Tree.RootNode.AppendChild(mNode);
            Game.Logger.Activate();
            Scene.BuildRenderScene();
        }

        public override void Render()
        {
            //for (int i = 0; i < technique.Description.PassCount; ++i)
            //{
            //    pass.Apply(Device.ImmediateContext);
            //    Device.ImmediateContext.DrawIndexed(triangle.IndexCount, 0, 0);
            //}
            Device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            //Device.ImmediateContext.Rasterizer.State = RasterizerState.FromDescription(Device,
            //    new RasterizerStateDescription()
            //        {
            //            CullMode = CullMode.None,
            //            FillMode = FillMode.Solid,
            //            IsFrontCounterclockwise = true
            //        });

            BlendStateDescription bDescr = new BlendStateDescription();
            RenderTargetBlendDescription rtbd = new RenderTargetBlendDescription()
            {
                BlendOperationAlpha = SlimDX.Direct3D11.BlendOperation.Add,
                SourceBlendAlpha = BlendOption.Zero,
                DestinationBlendAlpha = BlendOption.Zero,

                BlendOperation = SlimDX.Direct3D11.BlendOperation.Add,
                SourceBlend = BlendOption.SourceAlpha,
                DestinationBlend = BlendOption.InverseSourceAlpha
            };

            rtbd.BlendEnable = true;
            rtbd.RenderTargetWriteMask = ColorWriteMaskFlags.All;
            bDescr.RenderTargets[0] = rtbd;

            Device.ImmediateContext.OutputMerger.BlendState = BlendState.FromDescription(Device, bDescr);
            
            Game.Logger.Update();
            Scene.Display();
        }

        public override void ProcessInput()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
