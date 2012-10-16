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
        readonly StereoRenderer stereoRenderer;
        readonly StereoCamera stereoCamera;
        readonly SceneManager sceneManager;
        private Texture2D lTexture;
        private Texture2D rTexture;

        private RenderTargetView rTargetViewLeft;
        private RenderTargetView rTargetViewRight;

        private bool isDirty;


        public RenderStereoTextureCommand(int width, int height, StereoRenderer stereoRenderer)
            : base(width, height, stereoRenderer.Scene)
        {
            this.sceneManager = stereoRenderer.Scene;
            this.stereoRenderer = stereoRenderer;
            this.stereoCamera = (StereoCamera)stereoRenderer.Camera;
            this.stereoCamera.StereoParametersChanged += RequestUpdate;
            

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

            

            Texture = new Texture2D(Game.Context.Device, StereoTextureDesc);
        }

        private void RequestUpdate(object sender, EventArgs e)
        {
            isDirty = true;
        }

        //void Update()
        //{
        //    stereoCamera.Update();
        //    SceneTree.Update();
        //    isDirty = false;
        //}

        public override void Execute()
        {
            //Game.RenderEvent.Wait();

            //if (isDirty)
            //    Update();

            //stereoCamera.Update();
            //SceneTree.Update();

            lTexture = new Texture2D(Game.Context.Device, RenderTextureDesc);
            rTexture = new Texture2D(Game.Context.Device, RenderTextureDesc);

            rTargetViewLeft = new SlimDX.Direct3D11.RenderTargetView(Game.Context.Device, lTexture);
            rTargetViewRight = new SlimDX.Direct3D11.RenderTargetView(Game.Context.Device, rTexture);

            stereoCamera.EnableLeftStereoProjection();
            RenderSceneImage(lTexture, rTargetViewLeft);
            stereoCamera.EnableRightStereoProjection();
            RenderSceneImage(rTexture, rTargetViewRight);
            Texture2D newTexture = Stereo.Make3D(lTexture, rTexture);

            FreeResources();
            Texture = newTexture;
        }

        internal void FreeResources()
        {
            lTexture.Dispose();
            rTexture.Dispose();
            rTargetViewLeft.Dispose();
            rTargetViewRight.Dispose();
            Texture.Dispose();
        }

        void RenderSceneImage(Texture2D texture, RenderTargetView rTargetView)
        {
            DeviceContext immediateContext = Game.Context.Immediate;
            immediateContext.OutputMerger.SetTargets(DepthStencilView, rTargetView);
            immediateContext.Rasterizer.SetViewports(new Viewport(0, 0, texture.Description.Width,
                                                                  texture.Description.Height, 0.0f, 1.0f));
            immediateContext.ClearRenderTargetView(rTargetView, Color.CornflowerBlue);
            immediateContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
            SceneTree.Display(CommandAttributes.RequiredForSceneRender);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            FreeResources();
        }

    }
}
