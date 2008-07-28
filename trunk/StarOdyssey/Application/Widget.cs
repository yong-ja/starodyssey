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
    public class Planet : BaseEntity
    {
        public override void Render()
        {
            Device device = Game.Device;
            device.SetTransform(TransformState.World, Matrix.Translation(position));
            base.Render();
            
        }

        public Planet() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor(
                new MeshDescriptor("Airplane", "Meshes\\plane.X"), 
                new MaterialDescriptor(typeof(SpecularMaterial))))
        {
            ((SpecularMaterial)mesh.Materials[0]).Diffuse = new Color4(0.0f, 0f, 1f);
        }
    }
}
