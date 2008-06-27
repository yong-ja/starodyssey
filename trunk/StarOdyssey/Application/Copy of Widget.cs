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
    public class Grid : BaseEntity<SpecularMaterial>
    {

        float rotation = 0;

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public override void Render()
        {
            Device device = Game.Device;
            device.SetTransform(TransformState.World, Matrix.Scaling(2.0f, 2.0f, 2.0f)*Matrix.RotationX(rotation) * Matrix.Translation(position));
            MeshObject.DrawWithEffect();
            
        }

        public Grid() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor(
                new MeshDescriptor("Airplane", "Meshes\\plane.X")))
        {
            mesh.Materials[0].Diffuse = new Color4(1.0f, 0f, 0f);
        }
    }
}
