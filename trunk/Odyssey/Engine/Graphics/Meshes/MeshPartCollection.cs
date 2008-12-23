using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public class MeshPartCollection : Collection<MeshPart>
    {
        readonly bool areMaterialsEqual;

        public bool AreMaterialsEqual
        {
            get { return areMaterialsEqual; }
        }

        public MeshPartCollection(IMesh mesh, MaterialNode materialNode)
        {
            MaterialCollection materialCollection = materialNode.Materials;
            areMaterialsEqual = true;

            for (int i = 0; i < materialCollection.Count; i++)
            {
                AbstractMaterial material = materialCollection[i];
                MeshPart meshPart = new MeshPart(i, material, mesh as AbstractMesh<BaseMesh>);

                Add(meshPart);
            }

            Type previousMaterial = materialCollection[0].GetType();
            for (int i = 1; i < materialCollection.Count; i++)
                areMaterialsEqual = materialCollection[i].GetType() == previousMaterial;
        }
    }
}
