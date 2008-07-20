using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Meshes;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.Odyssey;
using SlimDX.Direct3D9;
using SlimDX;

namespace AvengersUtd.StarOdyssey
{
    public class LightWidget : BaseEntity<SpecularMaterial>
    {
        public override void Render()
        {
            Device device = Game.Device;
            device.SetTransform(TransformState.World, Matrix.Translation(position) * Matrix.Scaling(0.25f, 0.25f, 0.25f));
            MeshObject.DrawWithEffect();
            
        }

        public LightWidget() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor(
                new MeshDescriptor("LightWidget", "Meshes\\Planets\\tiny.X")))
        {
            Vector4 vLightPos = Game.CurrentScene.LightManager.GetParameter(0, AvengersUtd.Odyssey.Objects.Effects.FXParameterType.LightPosition);
            position = new Vector3(vLightPos.X, vLightPos.Y, vLightPos.Z);

            mesh.Materials[0].Diffuse = new Color4(1.0f, 1f, 0f);
        }
    }
}
