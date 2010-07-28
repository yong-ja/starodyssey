using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using AvengersUtd.Odyssey.Rendering;
using AvengersUtd.Odyssey.Geometry;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using SlimDX.DXGI;
using AvengersUtd.Odyssey.Graphics.Rendering;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class TestRenderer : Renderer
    {
        Polygon triangle;
        //private Effect effect;
        //private EffectTechnique technique;
        //private EffectPass pass;

        SceneGraph sceneGraph;
        RenderableNode rNode;
        SceneOrganizer sOrg;

        public void TestInit2()
        {
            AvengersUtd.Odyssey.Text.TextManager.DrawText("prova");

            triangle = Polygon.CreateTexturedQuad(new Vector4(0f, 0.5f, 0.5f, 1f), 0.5f, 0.5f);


            sceneGraph = new SceneGraph();

            FunctionalMaterial texturer = new FunctionalMaterial();
            MaterialNode mNode = new MaterialNode(texturer);
            rNode = new RenderableNode(triangle);
            mNode.AppendChild(rNode);
            sceneGraph.RootNode.AppendChild(mNode);

            sOrg = new SceneOrganizer();
            sOrg.BuildRenderScene(sceneGraph);


        }
        public override void Init()
        {
            //SceneGraph sceneGraph = new SceneGraph();
            //triangle = Polygon.CreateTexturedQuad(new Vector4(0f, 0.5f, 0.5f, 1f), 0.5f, 0.5f);
            //ShaderBytecode bytecode = ShaderBytecode.CompileFromFile("Effects\\Texture.fx", "fx_5_0", ShaderFlags.None, EffectFlags.None);
            //effect = new Effect(Device, bytecode);
            //technique = effect.GetTechniqueByIndex(0);
            //pass = technique.GetPassByIndex(0);

            //ShaderResourceView srv = ShaderResourceView.FromFile(Device, "prova.jpg");
            //EffectResourceVariable erv = effect.GetVariableByName("tDiffuseMap").AsResource();
            //EffectMatrixVariable emv = effect.GetVariableByName("mWorld").AsMatrix();

            //EffectConstantBuffer ecb = effect.GetConstantBufferByName("Matrices");

            //emv.SetMatrix(Matrix.Translation(new Vector3(0,0.5f,0)));
            //erv.SetResource(srv); 
            //Device.ImmediateContext.InputAssembler.InputLayout = new InputLayout(Device, pass.Description.Signature,
            //        TexturedVertex.InputElements);
           

            TestInit2();
        }

        public override void Render()
        {
            //for (int i = 0; i < technique.Description.PassCount; ++i)
            //{
            //    pass.Apply(Device.ImmediateContext);
            //    Device.ImmediateContext.DrawIndexed(triangle.IndexCount, 0, 0);
            //}

            Device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            Device.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(triangle.Vertices, 24, 0));
            Device.ImmediateContext.InputAssembler.SetIndexBuffer(triangle.Indices, Format.R16_UInt, 0);
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
            
            sOrg.Display();
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
