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
    public class Grid : BaseEntity, IRotatable
    {

        float rotation = 0;
       
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }


        public Grid() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor("Grid", new MeshDescriptor("Meshes\\plane.X"),
                 new MaterialDescriptor(typeof(WireframeMaterial))))
        {
            CastsShadows = false;
           //((SpecularMaterial)mesh.Materials[0]).Diffuse = new Color4(0.0f, 1f, 0f);
        }

        #region IRotatable Members

        public float XRotation
        {
            get { return rotation; }
        }

        #endregion
    }
}
