using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.ImageProcessing;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class RenderToTextureCommand : BaseCommand
    {
        private readonly SceneManager sceneTree;
        private readonly Texture2D texture;

        private readonly RenderTargetView rTargetView;
        private readonly DepthStencilView dStencilView;

        public Bitmap BitmapImage
        {
            get { return ImageHelper.BitmapFromTexture(texture); }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public RenderToTextureCommand(int width, int height, SceneManager sceneTree) : base(CommandType.Action)
        {
            this.sceneTree = sceneTree;
            Texture2DDescription textureDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.R8G8B8A8_UNorm,
                Width = width,
                Height = height,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            };
            
            texture = new Texture2D(Game.Context.Device, textureDesc);

            Texture2DDescription depthBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.D32_Float,
                Width = width,
                Height = height,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            };

           
            using (Texture2D depthBuffer = new Texture2D(Game.Context.Device, depthBufferDesc))
            {
                dStencilView = new DepthStencilView(Game.Context.Device, depthBuffer);
            }

            rTargetView = new RenderTargetView(Game.Context.Device, texture);
        }

        public override void Execute()
        {
            Game.RenderEvent.Wait();

            DeviceContext immediateContext = Game.Context.Immediate;
            immediateContext.OutputMerger.SetTargets(dStencilView, rTargetView);
            immediateContext.Rasterizer.SetViewports(new Viewport(0, 0, texture.Description.Width,
                                                                  texture.Description.Height, 0.0f, 1.0f));
            immediateContext.ClearRenderTargetView(rTargetView, Color.CornflowerBlue);
            immediateContext.ClearDepthStencilView(dStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
            sceneTree.Display();
            //Game.Context.SwapChain.Present(0, PresentFlags.None);
            //immediateContext.OutputMerger.SetTargets(Game.Context.DepthStencilView, Game.Context.RenderTargetView);

            Texture2D.ToFile(Game.Context.Immediate, texture, ImageFileFormat.Png, "prova2.png");
        }

        protected override void OnDispose()
        {
            texture.Dispose();
            rTargetView.Dispose();
            dStencilView.Dispose();
        }
    }
}
