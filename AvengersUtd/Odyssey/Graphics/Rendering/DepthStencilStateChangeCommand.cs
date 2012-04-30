using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class DepthStencilStateChangeCommand : BaseCommand
    {
        private DepthStencilState depthStencilState;
        public DepthStencilStateDescription Description { get; set; }

        public DepthStencilStateChangeCommand(DepthStencilStateDescription dStateDesc)
            : base(CommandType.DepthStencilStateChange)
        {
            Description = dStateDesc;
        }

        public override void Execute()
        {
            depthStencilState = DepthStencilState.FromDescription(Game.Context.Device, Description);
            Game.Context.Immediate.OutputMerger.DepthStencilState = depthStencilState;
        }

        protected override void OnDispose()
        {
            if (depthStencilState != null)
                depthStencilState.Dispose();
        }

        public static DepthStencilStateChangeCommand Default
        {
            get
            {
                DepthStencilStateDescription dsStateDesc = new DepthStencilStateDescription
                           {
                               IsDepthEnabled = true,
                               IsStencilEnabled = false,
                               DepthWriteMask = DepthWriteMask.All,
                               DepthComparison = Comparison.LessEqual,
                           };

                return new DepthStencilStateChangeCommand(dsStateDesc);
            }
        }

        public static DepthStencilStateChangeCommand DepthWriteDisabled
        {
            get
            {
                DepthStencilStateDescription dsStateDesc = new DepthStencilStateDescription
                                                               {
                                                                   IsDepthEnabled = false,
                                                                   IsStencilEnabled = false,
                                                                   DepthWriteMask = DepthWriteMask.All,
                                                                   DepthComparison = Comparison.Always
                                                               };
                return new DepthStencilStateChangeCommand(dsStateDesc);
            }
        }
    }
}
