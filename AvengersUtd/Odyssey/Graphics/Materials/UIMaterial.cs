using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class UIMaterial : AbstractMaterial
    {
        public static RenderableCollectionDescription ItemsDescription
        {
            get
            {
                return new RenderableCollectionDescription
                    {
                        CommonResources = true,
                        PrimitiveTopology = PrimitiveTopology.TriangleList,
                        IndexFormat = DeviceContext11.DefaultIndexFormat,
                        RenderingOrderType = RenderingOrderType.MixedGeometry,
                        InputElements = ColoredVertex.InputElements,
                        PreferredRenderCommandType = typeof(UserInterfaceRenderCommand)
                    };
            }
        }

        public UIMaterial() : base("UIMaterial.fx", ItemsDescription)
        {
            PreRenderStateList.Add(BlendStateChangeCommand.DefaultEnabled);
            PreRenderStateList.Add(DepthStencilStateChangeCommand.DepthWriteDisabled);
            PostRenderStateList.Add(BlendStateChangeCommand.DefaultDisabled);
            PostRenderStateList.Add(DepthStencilStateChangeCommand.Default);

        }

        protected override void OnInstanceParametersInit()
        {
            EffectDescription.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.ObjectWorld,
                EffectDescription.Effect));
        }

        protected override void OnDynamicParametersInit()
        {
            EffectDescription.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraView, EffectDescription.Effect));
            EffectDescription.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraOrthographicProjection, EffectDescription.Effect));
        }
    }
}
