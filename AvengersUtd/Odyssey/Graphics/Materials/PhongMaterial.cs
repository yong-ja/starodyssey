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
    public class PhongMaterial : ShaderMaterial
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

        public PhongMaterial() : base("Phong.fx", ItemsDescription)
        {
            

        }

        //public override void ApplyStaticParameters()
        //{
        //    //EffectDescription.SetStaticParameter(SceneVariable.LightPosition);
        //}

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
            //EffectDescription.SetInstanceParameter(InstanceVariable.Diffuse);
            //EffectDescription.SetInstanceParameter(InstanceVariable.Ambient);
            //EffectDescription.SetInstanceParameter(InstanceVariable.AmbientCoefficient);
            EffectDescription.SetInstanceParameter(InstanceVariable.MaterialBuffer);
        }
    }
}
