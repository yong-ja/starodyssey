using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class RasterizerStateChangeCommand : BaseCommand
    {
        private readonly RasterizerStateDescription rasterizerStateDesc;

        public RasterizerStateChangeCommand(RasterizerStateDescription rStateDesc) : 
            base(CommandType.RasterizerStateChange)
        {
            rasterizerStateDesc = rStateDesc;
        }

        public override void Execute()
        {
            RenderForm11.Device.ImmediateContext.Rasterizer.State = RasterizerState.FromDescription(
                RenderForm11.Device, rasterizerStateDesc);
        }

        protected override void OnDispose()
        {
            return;
        }
    }
}
