using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class RenderToTextureCommand : BaseCommand
    {
        private BaseCommand[] commands;
        private Texture2D texture;

        private RenderTargetView rTargetView;
        public RenderToTextureCommand(Size textureSize, params BaseCommand[] commands) : base(CommandType.Action)
        {
            this.commands = commands;
            Texture2DDescription textureDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.Write,
                Format = Format.D32_Float,
                Width = textureSize.Width,
                Height = textureSize.Height,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            };
            
            texture = new Texture2D(Game.Context.Device, textureDesc);
            rTargetView = new RenderTargetView(Game.Context.Device, texture);

            //new Texture2DDescription()
            //texture = new Texture2D(Game.Context.Device, );
            Game.Context.SwapChain.
        }

        public override void Execute()
        {
            //Game.Context.Device.ImmediateContext.r
            throw new NotImplementedException();
        }

        protected override void OnDispose()
        {
            throw new NotImplementedException();
        }
    }
}
