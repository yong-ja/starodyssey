using System;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    /// <summary>
    /// abstract mesh
    /// </summary>
    /// <typeparam name="MeshT">mesh type</typeparam>
    public abstract class AbstractMesh<MeshT> : IMesh
        where MeshT:BaseMesh
    {
        protected bool disposed;

        protected MeshT meshObject;
        protected AbstractMaterial[] materials;

        protected MeshPartCollection meshPartCollection;

        //adiacency buffer
        protected int[] adiacency;

        #region Properties
        public int MaterialCount
        {
            get { return materials.Length; }
        }

        public AbstractMaterial[] Materials
        {
            //return a material
            get { return materials; }
        }

        public MeshT Mesh
        {
            get { return meshObject; }
        } 
        #endregion

        public void SetMaterial(int index, AbstractMaterial material)
        {
            materials[index] = material;
        }

        /*
        /// <summary>
        /// lock vertex buffer
        /// </summary>
        /// <typeparam name="K">vertex type</typeparam>
        /// <returns>graphics buffer that contains vertices</returns>
        public K[] LockVertexBuffer<K>() where K : struct
        {
            //lock vertex buffer and return graphics buffer
            //blocca il vertex buffer e restituisce il graphics buffer
            return (K[]) meshObject.LockVertexBuffer(typeof (K), LockFlags.None, new int[] {meshObject.VertexCount});
        }
        */

        /// <summary>
        /// release vertex buffer
        /// </summary>
        public void UnlockVertexBuffer()
        {
            //release vertex buffer
            //libera il vertex buffer
            meshObject.UnlockVertexBuffer();
        }

        /*
        /// <summary>
        /// lock index buffer
        /// </summary>
        /// <typeparam name="K">index type</typeparam>
        /// <returns>graphics buffer that contain index</returns>
        public K[] LockIndexBuffer<K>() where K : struct
        {
            return (K[]) meshObject.LockIndexBuffer(typeof (K), LockFlags.None, new int[] {meshObject.FaceCount*3});
        }
        */

        /// <summary>
        /// lock index buffer
        /// </summary>
        public void UnlockIndexBuffer()
        {
            meshObject.UnlockIndexBuffer();
        }

        /// <summary>
        /// generate adiacency
        /// </summary>
        public void GenerateAdiacency()
        {
            adiacency = meshObject.GenerateAdjacency(1e-6f);
        }


        #region Drawing methods

        public void Render()
        {
            if (meshPartCollection.AreMaterialsEqual)
            {
                DrawMeshWithMaterial(materials[0]);
            }
            else
            {
                DrawMeshWithMaterials();
            }
        }

        public void DrawMeshWithMaterials()
        {
            for (int i = 0; i < meshPartCollection.Count; i++)
            {
                MeshPart meshPart = meshPartCollection[i];
                meshPart.Material.Apply();
                DrawSubsetWithMaterial(meshPartCollection[i].Material, meshPartCollection[i].Subset);
            }
        }

        // Draws a particular subset with the specified effect
        public void DrawSubsetWithMaterial(AbstractMaterial material, int subset)
        {
            Effect effect = material.EffectDescriptor.Effect;
            int pass = material.EffectDescriptor.Pass;

            int passes = effect.Begin(FX.None);

            effect.BeginPass(pass);
            Draw(subset);
            effect.EndPass();

            effect.End();
        }

        public void DrawMeshWithMaterial(AbstractMaterial material)
        {
            materials[0].Apply();
            Effect effect = material.EffectDescriptor.Effect;
            int pass = material.EffectDescriptor.Pass;

            effect.Begin(FX.None);
            effect.BeginPass(pass);

            for (int i = 0; i < MaterialCount; i++)
                Draw(i);
            effect.EndPass();
            effect.End();
        }

        /// <summary>
        /// Draw mesh using specified subset, without applying material.
        /// </summary>
        /// <param name="subset">The subset of the mesh to draw.</param>
        public void Draw(int subset)
        {
            meshObject.DrawSubset(subset);
        }

        /// <summary>
        /// Draws every subset, without applying material.
        /// </summary>
        public void DrawMesh()
        {
            for (int i = 0; i < MaterialCount; i++)
                meshObject.DrawSubset(i);
        } 
        #endregion

        #region IDisposable
        /// <summary>
        /// Dispose meshObject and materials
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose managed components
                }

                // dispose unmanaged components
                if (meshObject != null)
                {
                    meshObject.Dispose();
                    meshObject = null;
                }
            }
            disposed = true;
        }

        ~AbstractMesh()
        {
            Dispose(false);
        }
        #endregion

        #region IMesh Members

        BaseMesh IMesh.Mesh
        {
            get { return meshObject; }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}