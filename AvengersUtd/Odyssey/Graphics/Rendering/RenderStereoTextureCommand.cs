using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using System.Drawing;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class RenderStereoTextureCommand : RenderToTextureCommand
    {
        readonly StereoCamera stereoCamera;
        private readonly Texture2D lTexture;
        private readonly Texture2D rTexture;

        private readonly RenderTargetView rTargetViewLeft;
        private readonly RenderTargetView rTargetViewRight;
        private readonly DepthStencilView dStencilView;


        public RenderStereoTextureCommand(int width, int height, SceneManager sceneTree, StereoCamera stereoCamera)
            : base(width, height, sceneTree)
        {
            this.stereoCamera = stereoCamera;
            lTexture = new Texture2D(Game.Context.Device, RenderTextureDesc);
            rTexture = new Texture2D(Game.Context.Device, RenderTextureDesc);

            Texture2DDescription StereoTextureDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.R8G8B8A8_UNorm,
                Width = 2 * width,
                Height = height,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            };

            rTargetViewLeft = new SlimDX.Direct3D11.RenderTargetView(Game.Context.Device, lTexture);
            rTargetViewRight = new SlimDX.Direct3D11.RenderTargetView(Game.Context.Device, rTexture);

            Texture = new Texture2D(Game.Context.Device, StereoTextureDesc);
        }

        public override void Execute()
        {
            //Game.RenderEvent.Wait();
            stereoCamera.Update();
            SceneTree.Update();

            stereoCamera.EnableLeftStereoProjection();
            RenderSceneImage(lTexture, rTargetViewLeft);
            stereoCamera.EnableRightStereoProjection();
            RenderSceneImage(rTexture, rTargetViewRight);
            Texture = Stereo.Make3D(lTexture, rTexture);
        }

        void RenderSceneImage(Texture2D texture, RenderTargetView rTargetView)
        {
            DeviceContext immediateContext = Game.Context.Immediate;
            immediateContext.OutputMerger.SetTargets(DepthStencilView, rTargetView);
            immediateContext.Rasterizer.SetViewports(new Viewport(0, 0, texture.Description.Width,
                                                                  texture.Description.Height, 0.0f, 1.0f));
            immediateContext.ClearRenderTargetView(rTargetView, Color.CornflowerBlue);
            immediateContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
            SceneTree.Display();
        }

    }
}
