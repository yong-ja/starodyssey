using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Text;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class FunctionalMaterial:AbstractMaterial
    {
        
        public FunctionalMaterial():base("Texture.fx")
        {
            PreRenderStateList.Add(RasterizerStateChangeCommand.Default);
            PreRenderStateList.Add(BlendStateChangeCommand.DefaultEnabled);
            PostRenderStateList.Add(BlendStateChangeCommand.DefaultDisabled);
        }

        protected override void OnInstanceParametersInit()
        {
            EffectDescriptor.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.ObjectWorld, EffectDescriptor.Effect));
            EffectDescriptor.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.DiffuseMap, EffectDescriptor.Effect));

        }

        protected override void OnDynamicParametersInit()
        {
            EffectDescriptor.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraWorld, EffectDescriptor.Effect));
            EffectDescriptor.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraView, EffectDescriptor.Effect));
            EffectDescriptor.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraOrthographicProjection, EffectDescriptor.Effect));
        }

    }
}
