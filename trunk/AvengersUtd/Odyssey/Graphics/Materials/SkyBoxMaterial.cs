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

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class SkyBoxMaterial : AbstractMaterial
    {
        public SkyBoxMaterial() : base("SkyBox.fx")
        {
            RenderableCollectionDescription = new RenderableCollectionDescription
            {
                CommonResources = false,
                PrimitiveTopology = PrimitiveTopology.TriangleList,
                IndexFormat = DeviceContext11.DefaultIndexFormat,
                RenderingOrderType = RenderingOrderType.Last,
                InputElements = Textured3DVertex.InputElements,
                PreferredRenderCommandType = typeof(RenderCommand)
            };
        }

        protected override void OnDynamicParametersInit()
        {
            EffectDescription.SetDynamicParameter(SceneVariable.CameraRotation);
            EffectDescription.SetDynamicParameter(SceneVariable.CameraProjection);
         }

        protected override void OnInstanceParametersInit()
        {
            EffectDescription.SetInstanceParameter(InstanceVariable.DiffuseMap);
        }
    }
}
