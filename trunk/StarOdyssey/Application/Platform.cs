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
    public class Platform : BaseEntity
    {

        public Platform() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor("Grid", new MeshDescriptor("Meshes\\platform.X"),
                 new MaterialDescriptor(typeof(PhongMaterial))))
        {
            PositionV3 = new Vector3(0, -2, 0);
        }
    }
}
