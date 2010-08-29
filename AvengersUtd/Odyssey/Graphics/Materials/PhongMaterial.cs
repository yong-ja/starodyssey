﻿using System;
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
    public class PhongMaterial : AbstractMaterial
    {
        public PhongMaterial() : base("Phong.fx")
        {
            RenderableCollectionDescription = new RenderableCollectionDescription
            {
                CommonResources = false,
                PrimitiveTopology = PrimitiveTopology.TriangleList,
                IndexFormat = DeviceContext11.DefaultIndexFormat,
                RenderingOrderType = RenderingOrderType.MixedGeometry,
                InputElements = TexturedMeshVertex.InputElements,
                PreferredRenderCommandType = typeof(RenderCommand)
            };

        }

        public override void ApplyStaticParameters()
        {
            //EffectDescription.SetStaticParameter(SceneVariable.LightPosition);
        }

        protected override void OnDynamicParametersInit()
        {
            //EffectDescription.SetDynamicParameter(SceneVariable.CameraWorld);
            EffectDescription.SetDynamicParameter(SceneVariable.CameraView);
            EffectDescription.SetDynamicParameter(SceneVariable.CameraProjection);
            EffectDescription.SetDynamicParameter(SceneVariable.EyePosition);
        }

        protected override void OnInstanceParametersInit()
        {
            EffectDescription.SetInstanceParameter(InstanceVariable.ObjectWorld);
            EffectDescription.SetInstanceParameter(InstanceVariable.Diffuse);
        }
    }
}
