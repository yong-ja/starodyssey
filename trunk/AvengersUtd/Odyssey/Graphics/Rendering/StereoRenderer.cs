using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Settings;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public abstract class StereoRenderer : Renderer
    {
        RenderToTextureCommand cStereoRenderer;
        ResourceRegion stereoSourceBox;
        Texture2D backBuffer;

        protected StereoRenderer(IDeviceContext deviceContext) : base(deviceContext)
        {
            DeviceSettings settings = Game.Context.Settings;
            Camera = new StereoCamera();
            Camera.Reset();

            stereoSourceBox = new ResourceRegion
            {
                Front = 0,
                Back = 1,
                Top = 0,
                Bottom = settings.ScreenHeight,
                Left = 0,
                Right = settings.ScreenWidth
            };

            backBuffer = Game.Context.GetBackBuffer();
            cStereoRenderer = new RenderStereoTextureCommand(settings.ScreenWidth, settings.ScreenHeight, this);
        }

        public override void EndInit()
        {
            base.EndInit();
            Scene.CommandManager.AddBaseCommand(cStereoRenderer);
            cStereoRenderer.Execute();
        }

        public override void Present()
        {
            DeviceContext.Immediate.CopySubresourceRegion(cStereoRenderer.Texture, 0, stereoSourceBox, backBuffer, 0, 0, 0, 0);
            DeviceContext.Present();
        }

        public override void Render()
        {
            Game.Logger.Update();
            Scene.Display();
            
        }


    }
}
