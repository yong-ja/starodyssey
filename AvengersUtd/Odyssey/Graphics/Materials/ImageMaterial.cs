using System;
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
    public class ImageMaterial : AbstractMaterial
    {
        static RenderableCollectionDescription ItemsDescription
        {
            get
            {
                return new RenderableCollectionDescription
                    {
                        CommonResources = false,
                        PrimitiveTopology = PrimitiveTopology.TriangleList,
                        IndexFormat = DeviceContext11.DefaultIndexFormat,
                        RenderingOrderType = RenderingOrderType.OpaqueGeometry,
                        InputElements = TexturedVertex.InputElements,
                        PreferredRenderCommandType = typeof(RenderCommand)
                    };
            }
        }

        public ImageMaterial() : base("Texture.fx", ItemsDescription)
        {
        }

        protected override void OnDynamicParametersInit()
        {
            //EffectDescription.SetDynamicParameter(SceneVariable.CameraWorld);
            EffectDescription.SetDynamicParameter(SceneVariable.CameraView);
            EffectDescription.SetDynamicParameter(SceneVariable.CameraOrthographicProjection);
            
        }

        protected override void OnInstanceParametersInit()
        {
            EffectDescription.SetInstanceParameter(InstanceVariable.ObjectWorld);
            EffectDescription.SetInstanceParameter(InstanceVariable.DiffuseMap);
        }
       
    }
}
