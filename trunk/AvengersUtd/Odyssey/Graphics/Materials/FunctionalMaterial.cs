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
            effectDescriptor.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.ObjectWorld, effectDescriptor.Effect));
            effectDescriptor.SetInstanceParameter(InstanceParameter.Create(InstanceVariable.DiffuseMap, effectDescriptor.Effect));

        }

        protected override void OnDynamicParametersInit()
        {
            effectDescriptor.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraWorld, effectDescriptor.Effect));
            effectDescriptor.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraView, effectDescriptor.Effect));
            effectDescriptor.SetDynamicParameter(SharedParameter.Create(SceneVariable.CameraOrthographicProjection, effectDescriptor.Effect));
        }

        #region IDiffuseMap Members

        public Texture2D DiffuseMap
        {
            get { return TextManager.DrawText("Nadia\nTi voglio tanto bene!"); }
                 
        }

        #endregion
    }
}
