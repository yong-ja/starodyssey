﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class WireframeMaterial : ShaderMaterial
    {
        public static RenderableCollectionDescription ItemsDescription
        {
            get
            {
                return new RenderableCollectionDescription
                    {
                        CommonResources = false,
                        PrimitiveTopology = PrimitiveTopology.TriangleList,
                        IndexFormat = DeviceContext11.DefaultIndexFormat,
                        RenderingOrderType = RenderingOrderType.MixedGeometry,
                        InputElements = TexturedMeshVertex.InputElements,
                        PreferredRenderCommandType = typeof(RenderCommand)
                    };
            }
        }
        public WireframeMaterial()
            : base("SolidWireframe.fx", ItemsDescription)
        {
        }

        protected override void OnDynamicParametersInit()
        {
            EffectDescription.SetDynamicParameter(SceneVariable.CameraView);
            EffectDescription.SetDynamicParameter(SceneVariable.CameraProjection);
            //EffectDescription.SetDynamicParameter(SceneVariable.CameraWorld);
        }

        protected override void OnInstanceParametersInit()
        {
            EffectDescription.SetInstanceParameter(InstanceVariable.ObjectWorld);
            EffectDescription.SetInstanceParameter(InstanceVariable.Diffuse);
        }
    }
}
