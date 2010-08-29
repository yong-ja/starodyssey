using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class TextMaterial:AbstractMaterial
    {
        
        public TextMaterial():base("Texture.fx")
        {
            PreRenderStateList.Add(BlendStateChangeCommand.DefaultEnabled);
            PostRenderStateList.Add(BlendStateChangeCommand.DefaultDisabled);
            RenderableCollectionDescription = new RenderableCollectionDescription
                                                  {
                                                      CommonResources = false,
                                                      PrimitiveTopology = PrimitiveTopology.TriangleList,
                                                      IndexFormat = DeviceContext11.DefaultIndexFormat,
                                                      RenderingOrderType = RenderingOrderType.AdditiveBlendingGeometry,
                                                      InputElements = TexturedVertex.InputElements,
                                                      PreferredRenderCommandType = typeof(UserInterfaceRenderCommand)
                                                  };
        }

        protected override void OnInstanceParametersInit()
        {
            EffectDescription.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.ObjectWorld, EffectDescription.Effect));
            EffectDescription.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.DiffuseMap, EffectDescription.Effect));
        }

        protected override void OnDynamicParametersInit()
        {
            //EffectDescription.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraWorld, EffectDescription.Effect));
            EffectDescription.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraView, EffectDescription.Effect));
            EffectDescription.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraOrthographicProjection, EffectDescription.Effect));
        }

    }
}
