using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class RasterizerStateChangeCommand : BaseCommand
    {
        private RasterizerState rasterizerState;
        public RasterizerStateDescription Description { get; private set; }

        public RasterizerStateChangeCommand(RasterizerStateDescription rStateDesc)
            : base(CommandType.RasterizerStateChange)
        {
            Description = rStateDesc;
        }

        public override void Execute()
        {
            rasterizerState = RasterizerState.FromDescription(Game.Context.Device, Description);
            Game.Context.Device.ImmediateContext.Rasterizer.State = rasterizerState; 
        }

        protected override void OnDispose()
        {
            rasterizerState.Dispose();
        }

        public override bool Equals(BaseCommand other)
        {
            bool equal = base.Equals(other);
            return equal && ((RasterizerStateChangeCommand) other).Description == Description;
        }

        public static RasterizerStateChangeCommand Default
        {
            get
            {
                RasterizerStateDescription rStateDesc = new RasterizerStateDescription()
                                                            {
                                                                CullMode = CullMode.Back,
                                                                IsDepthClipEnabled = true,
                                                                FillMode = FillMode.Solid,
                                                                IsAntialiasedLineEnabled = true,
                                                                IsFrontCounterclockwise = true,
                                                                IsMultisampleEnabled = true
                                                            };

                return new RasterizerStateChangeCommand(rStateDesc);
            }
        }

        public static RasterizerStateChangeCommand Wireframe
        {
            get
            {
                RasterizerStateDescription rStateDesc = new RasterizerStateDescription()
                {
                    CullMode = CullMode.None,
                    IsDepthClipEnabled = true,
                    FillMode = FillMode.Wireframe,
                    IsAntialiasedLineEnabled = true,
                    IsFrontCounterclockwise = true,
                    IsMultisampleEnabled = true
                };

                return new RasterizerStateChangeCommand(rStateDesc);
            }
        }
    }
}