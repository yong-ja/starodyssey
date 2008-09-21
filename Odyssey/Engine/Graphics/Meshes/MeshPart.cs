using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    /// <summary>
    /// This class specifies which material to use when drawing a subset of the mesh.
    /// </summary>
    public class MeshPart
    {
        AbstractMesh<BaseMesh> meshObject;
        AbstractMaterial material;
        int subset;

        public AbstractMaterial Material
        {
            get { return material; }
        }

        public int Subset
        {
            get { return subset; }
        }

        /// <summary>
        /// Creates a new instance of the MeshPart class.
        /// </summary>
        /// <param name="subset">The subset of the mesh to draw. </param>
        /// <param name="material">The material with which to draw the mesh.</param>
        /// <param name="mesh">The renderable object whose mesh this object refers to.</param>
        public MeshPart(int subset, AbstractMaterial material, AbstractMesh<BaseMesh> mesh)
        {
            this.subset = subset;
            this.material = material;
            this.meshObject = mesh;
        }

        /// <summary>
        /// Draws this mesh part using the stored material and subset.
        /// </summary>
        public void Draw()
        {
            meshObject.DrawSubsetWithMaterial(material, subset);
        }
    }
}
