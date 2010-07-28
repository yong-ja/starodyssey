using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Text;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public class FunctionalMaterial:AbstractMaterial, IDiffuseMap
    {
        
        public FunctionalMaterial():base("Texture.fx")
        {}

        protected override void OnInstanceParametersInit()
        {
            effectDescriptor.SetInstanceParameter(InstanceParameter.CreateDefault(FXParameterType.ObjectWorld, effectDescriptor.Effect));
        }

        protected override void OnDynamicParametersInit()
        {
            effectDescriptor.SetStaticParameter(SharedParameter.CreateDefault(MaterialParameter.DiffuseMap, effectDescriptor.Effect, this));
            effectDescriptor.SetDynamicParameter(SharedParameter.CreateDefault(FXParameterType.CameraWorld, effectDescriptor.Effect));
            effectDescriptor.SetDynamicParameter(SharedParameter.CreateDefault(FXParameterType.CameraView, effectDescriptor.Effect));
            effectDescriptor.SetDynamicParameter(SharedParameter.CreateDefault(FXParameterType.CameraOrthographicProjection, effectDescriptor.Effect));
        }

        #region IDiffuseMap Members

        public Texture2D DiffuseMap
        {
            get { return TextManager.DrawText("Nadia\nTi voglio tanto bene!"); }
                 
        }

        #endregion
    }
}
