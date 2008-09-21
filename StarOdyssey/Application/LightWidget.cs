using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.Odyssey;
using SlimDX.Direct3D9;
using SlimDX;

namespace AvengersUtd.StarOdyssey
{
    public class LightWidget : BaseEntity
    {
        public override void Render()
        {
            Device device = Game.Device;
            device.SetTransform(TransformState.World, Matrix.Translation(positionV3));
            base.Render();
            
        }

        public LightWidget() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor("LightWidget", new MeshDescriptor("Meshes\\Planets\\Tiny.X"),
                new MaterialDescriptor(typeof(SpecularMaterial))))
        {
            Vector4 vLightPos = Game.CurrentScene.LightManager.GetParameter(0, AvengersUtd.Odyssey.Graphics.Effects.FXParameterType.LightPosition);
            positionV3 = new Vector3(vLightPos.X, vLightPos.Y, vLightPos.Z);

            //((SpecularMaterial)mesh.Materials[0]).Diffuse = new Color4(1.0f, 1f, 0f);
            //((SpecularMaterial) mesh.Materials[0]).EffectDescriptor.Technique = "Specular";
        }
    }
}
