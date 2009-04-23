using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;

namespace AvengersUtd.Odyssey.Objects.Entities
{
    public class Teapot : BaseEntity
    {
        public Teapot() :
            base(new EntityDescriptor("Teapot", new MeshDescriptor("Meshes\\teapot.x"),
                new MaterialDescriptor(typeof(PhongMaterial))))
        {


        }


    }
}
