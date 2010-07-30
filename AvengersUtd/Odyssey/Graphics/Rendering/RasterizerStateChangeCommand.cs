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
        public RasterizerStateDescription Description { get; private set; }

        public RasterizerStateChangeCommand(RasterizerStateDescription rStateDesc)
            : base(CommandType.RasterizerStateChange)
        {
            Description = rStateDesc;
        }

        public override void Execute()
        {
            RenderForm11.Device.ImmediateContext.Rasterizer.State =
                RasterizerState.FromDescription(RenderForm11.Device, Description);
        }

        protected override void OnDispose()
        {
            return;
        }

        public override bool Equals(BaseCommand other)
        {
            if (CommandType == other.CommandType)
                return ((RasterizerStateChangeCommand) other).Description == Description;
            else
                return false;
        }

        public static RasterizerStateChangeCommand Default
        {
            get
            {
                RasterizerStateDescription rStateDesc = new RasterizerStateDescription()
                                                            {
                                                                CullMode = CullMode.Back,
                                                                FillMode = FillMode.Solid,
                                                                IsAntialiasedLineEnabled = true,
                                                                IsFrontCounterclockwise = true,
                                                                IsMultisampleEnabled = true,
                                                            };

                return new RasterizerStateChangeCommand(rStateDesc);
            }
        }
    }
}