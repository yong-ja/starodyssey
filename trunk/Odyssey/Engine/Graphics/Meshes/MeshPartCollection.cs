using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public class MeshPartCollection : Collection<MeshPart>
    {
        bool areMaterialsEqual;

        public bool AreMaterialsEqual
        {
            get { return areMaterialsEqual; }
        }

        public MeshPartCollection(IMesh mesh, AbstractMaterial[] materials)
        {
            areMaterialsEqual = true;
            Type previousMaterial;
            for (int i=0; i<materials.Length; i++)
            {
                AbstractMaterial material = materials[i];
                MeshPart meshPart = new MeshPart(i, material, mesh as AbstractMesh<BaseMesh>);

                Add(meshPart);
            }

            previousMaterial = materials[0].GetType();
            for (int i = 1; i < materials.Length; i++)
                areMaterialsEqual = materials[i].GetType() == previousMaterial;
        }
    }
}
