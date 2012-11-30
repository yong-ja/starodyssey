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
        private readonly BlendState blendState;
        private readonly BlendStateDescription blendStateDescription;

        public BlendStateChangeCommand(RenderTargetBlendDescription bStateDesc)
            : base(CommandType.BlendStateChange)
        {
            CommandAttributes |= CommandAttributes.MonoRendering;
            Description = bStateDesc;
            blendStateDescription = new BlendStateDescription();
            blendStateDescription.RenderTargets[0] = Description;
            blendState = BlendState.FromDescription(Game.Context.Device, blendStateDescription);
        }

        public override void Execute()
        {
            Game.Context.Immediate.OutputMerger.BlendState =
                BlendState.FromDescription(Game.Context.Device, blendStateDescription);
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
                                                                BlendOperationAlpha = BlendOperation.Add,
                                                                SourceBlend = BlendOption.SourceAlpha,
                                                                SourceBlendAlpha = BlendOption.One,
                                                                DestinationBlend = BlendOption.InverseSourceAlpha,
                                                                DestinationBlendAlpha = BlendOption.One,
                                                                BlendOperation = BlendOperation.Add,
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
