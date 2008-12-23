using System;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Graphics.Materials;
using SlimDX;

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
        protected MaterialNode materialNode;

        protected MeshPartCollection meshPartCollection;

        //adiacency buffer
        protected int[] adiacency;

        #region Properties
        public int MaterialCount
        {
            get { return materialNode.Materials.Count; }
        }

        public MaterialNode MaterialNode
        {
            get { return materialNode; }
        }

        public MaterialCollection Materials
        {
            //return a material
            get { return materialNode.Materials; }
        }

        public MeshT Mesh
        {
            get { return meshObject; }
        } 
        #endregion

        public void SetMaterial(int index, AbstractMaterial material)
        {
            materialNode.Materials[index] = material;
        }


        #region Lock/Unlock methods
        /// <summary>
        /// lock vertex buffer
        /// </summary>
        /// <typeparam name="K">vertex type</typeparam>
        /// <returns>graphics buffer that contains vertices</returns>
        public DataStream LockVertexBuffer<K>() where K : struct
        {
            //lock vertex buffer and return graphics buffer
            return meshObject.LockVertexBuffer(LockFlags.None);
        }


        /// <summary>
        /// release vertex buffer
        /// </summary>
        public void UnlockVertexBuffer()
        {
            //release vertex buffer
            meshObject.UnlockVertexBuffer();
        }


        /// <summary>
        /// lock index buffer
        /// </summary>
        /// <typeparam name="K">index type</typeparam>
        /// <returns>graphics buffer that contain index</returns>
        public DataStream LockIndexBuffer<K>() where K : struct
        {
            return meshObject.LockIndexBuffer(LockFlags.None);
        }


        /// <summary>
        /// unlock index buffer
        /// </summary>
        public void UnlockIndexBuffer()
        {
            meshObject.UnlockIndexBuffer();
        } 
        #endregion

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
                DrawMeshWithMaterial(Materials[0]);
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
            material.Apply();
            Effect effect = material.EffectDescriptor.Effect;
            int pass = material.EffectDescriptor.Pass;

            effect.Begin(FX.None);
            effect.BeginPass(pass);

            for (int i = 0; i < meshPartCollection.Count; i++)
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