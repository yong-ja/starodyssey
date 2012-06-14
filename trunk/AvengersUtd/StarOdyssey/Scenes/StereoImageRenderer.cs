using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.UserInterface;
using SlimDX.Direct3D11;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class StereoImageRenderer : Renderer
    {
        private Texture2D stereoTexture;
        private Texture2D backBuffer;
        private ResourceRegion stereoSourceBox;

        public StereoImageRenderer(DeviceContext11 deviceContext11) : base(deviceContext11)
        {
        }


        public override void Init()
        {
            //OdysseyUI.RemoveHooks(Global.FormOwner);

            ImageLoadInformation imageLoadInfo = new ImageLoadInformation()
            {
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Read,
                FilterFlags = FilterFlags.None,
                Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm,
                MipFilterFlags = FilterFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Staging,
                MipLevels = 1
            };

            // Make texture 3D
            Texture2D sourceTexture = Texture2D.FromFile(Game.Context.Device, "medusa.jpg", imageLoadInfo);
            stereoTexture = Stereo.Make3D(sourceTexture);
            backBuffer = Game.Context.GetBackBuffer();

            stereoSourceBox = new ResourceRegion
                                  {
                                      Front = 0,
                                      Back = 1,
                                      Top = 0,
                                      Bottom = Game.Context.Settings.ScreenHeight,
                                      Left = 0,
                                      Right = Game.Context.Settings.ScreenWidth
                                  };
        }

        public override void Render()
        {
            Game.Context.Device.ImmediateContext.CopySubresourceRegion(stereoTexture, 0, stereoSourceBox, backBuffer, 0, 0, 0, 0);
        }

        public override void ProcessInput()
        {
            ;
        }
    }
}
