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
    public class Planet1 : BaseEntity
    {
        public override void Render()
        {
            Device device = Game.Device;
            device.SetTransform(TransformState.World, Matrix.Translation(position));
            base.Render();
            
        }

        public Planet1() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor(
                new MeshDescriptor("Airplane", "Meshes\\planetOuter.X"),
                 new MaterialDescriptor(typeof(EffectMaterial))))
        {
            //mesh.Materials[0].Diffuse = new Color4(0.0f, 0f, 1f);
        }
    }
}
