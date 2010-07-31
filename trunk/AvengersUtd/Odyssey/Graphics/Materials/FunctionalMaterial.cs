using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Text;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class FunctionalMaterial:AbstractMaterial
    {
        
        public FunctionalMaterial():base("Texture.fx")
        {
            PreRenderStateList.Add(RasterizerStateChangeCommand.Default);
            PreRenderStateList.Add(BlendStateChangeCommand.DefaultEnabled);
            PostRenderStateList.Add(BlendStateChangeCommand.DefaultDisabled);
            RenderableCollectionDescription = new RenderableCollectionDescription
                                                  {
                                                      CommonTexture = false,
                                                      PrimitiveTopology = PrimitiveTopology.TriangleList,
                                                      IndexFormat = Format.R16_UInt,
                                                      TranslucencyType = TranslucencyType.Additive,
                                                      InputElements = TexturedVertex.InputElements
                                                  };
        }

        protected override void OnInstanceParametersInit()
        {
            EffectDescription.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.ObjectWorld, EffectDescription.Effect));
            EffectDescription.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.DiffuseMap, EffectDescription.Effect));

        }

        protected override void OnDynamicParametersInit()
        {
            EffectDescription.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraWorld, EffectDescription.Effect));
            EffectDescription.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraView, EffectDescription.Effect));
            EffectDescription.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraOrthographicProjection, EffectDescription.Effect));
        }

    }
}
