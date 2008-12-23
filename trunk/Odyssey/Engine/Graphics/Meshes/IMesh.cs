using AvengersUtd.Odyssey.Graphics.Materials;
using SlimDX.Direct3D9;
using System;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public interface IMesh :IDisposable
    {
        /// <summary>
        /// Returns the number of materials
        /// </summary>
        int MaterialCount { get; }

        /// <summary>
        /// Returns the material array.
        /// </summary>
        MaterialCollection Materials { get; }

        BaseMesh Mesh { get; }

        /// <summary>
        /// Sets the material in the specified position.
        /// </summary>
        /// <param name="index">Index of the array.</param>
        /// <param name="material">New material.</param>
        void SetMaterial(int index, AbstractMaterial material);

        /// <summary>
        /// Draws the mesh by using the material information specified in this object.
        /// </summary>
        void DrawMeshWithMaterials();

        /// <summary>
        /// Draws the mesh by specifying an effect, the subset and the pass.
        /// </summary>
        /// <param name="material">The material with which to draw this subset.</param>
        /// <param name="subset">The subset of the mesh to draw.</param>
        void DrawSubsetWithMaterial(AbstractMaterial material, int subset);

        /// <summary>
        /// Draw each subset with the same material
        /// </summary>
        /// <param name="material">The material with which to draw this subset.</param>
        void DrawMeshWithMaterial(AbstractMaterial material);

        /// <summary>
        /// Draw mesh using specified subset, without applying material.
        /// </summary>
        /// <param name="subset">The subset of the mesh to draw.</param>
        void Draw(int subset);

        /// <summary>
        /// Draws every subset, without applying material.
        /// </summary>
        void DrawMesh();
    }
}