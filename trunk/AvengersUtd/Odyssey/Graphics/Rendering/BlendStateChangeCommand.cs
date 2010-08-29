using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class BlendStateChangeCommand : BaseCommand
    {
        public RenderTargetBlendDescription Description { get; private set; }
        private BlendState blendState;

        public BlendStateChangeCommand(RenderTargetBlendDescription bStateDesc)
            : base(CommandType.BlendStateChange)
        {
            Description = bStateDesc;
        }

        public override void Execute()
        {
            BlendStateDescription bStateDescr = new BlendStateDescription();
            bStateDescr.RenderTargets[0] = Description;

            blendState = BlendState.FromDescription(Game.Context.Device, bStateDescr);

            Game.Context.Immediate.OutputMerger.BlendState =
                BlendState.FromDescription(Game.Context.Device, bStateDescr);
        }

        protected override void OnDispose()
        {
            blendState.Dispose();
        }

        public override bool Equals(BaseCommand other)
        {
            bool equal = base.Equals(other);

            return equal && ((BlendStateChangeCommand)other).Description == Description ;
        }

        public static BlendStateChangeCommand DefaultEnabled
        {
            get
            { 
                RenderTargetBlendDescription bStateDesc = new RenderTargetBlendDescription()
                                                            {
                                                                BlendOperationAlpha = SlimDX.Direct3D11.BlendOperation.Add,
                                                                SourceBlendAlpha = BlendOption.Zero,
                                                                DestinationBlendAlpha = BlendOption.Zero,

                                                                BlendOperation = SlimDX.Direct3D11.BlendOperation.Add,
                                                                SourceBlend = BlendOption.SourceAlpha,
                                                                DestinationBlend = BlendOption.InverseSourceAlpha,
                                                                BlendEnable = true,
                                                                RenderTargetWriteMask = ColorWriteMaskFlags.All
                                                            };


                return new BlendStateChangeCommand(bStateDesc);
            }
        }

        public static BlendStateChangeCommand DefaultDisabled
        {
            get
            {
                RenderTargetBlendDescription bStateDesc = new RenderTargetBlendDescription()
                                                            {
                                                                BlendEnable = false,
                                                                RenderTargetWriteMask = ColorWriteMaskFlags.All
                                                            };

                return new BlendStateChangeCommand(bStateDesc);
            }
        }
    }
}
