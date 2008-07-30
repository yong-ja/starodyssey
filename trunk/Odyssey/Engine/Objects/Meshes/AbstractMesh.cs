using System;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Materials;

namespace AvengersUtd.Odyssey.Objects.Meshes
{
    /// <summary>
    /// abstract mesh
    /// </summary>
    /// <typeparam name="MeshT">mesh type</typeparam>
    public abstract class AbstractMesh<MeshT>
        where MeshT : BaseMesh      
    {

        protected bool disposed = false;

        //the mesh
        protected MeshT meshObject;

        //material list
        protected AbstractMaterial[] materials;

        //adiacency buffer
        protected int[] adiacency;

        /// <summary>
        /// return the number of materials
        /// </summary>
        public int MaterialCount
        {
            get { return materials.Length; }
        }

        /// <summary>
        /// return a material
        /// </summary>
        public AbstractMaterial[] Materials
        {
            //return a material
            get { return materials; }
        }

        /// <summary>
        /// set a material
        /// </summary>
        /// <param name="index">index of the array</param>
        /// <param name="material">material</param>
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


        protected void DrawWithEffect()
        {
            Effect effect = materials[0].EffectDescriptor.Effect;
            DrawWithEffect(effect,0,0);
        }

        protected void DrawWithEffect(Effect effect, int pass, int subset)
        {
            int passes = effect.Begin(FX.None);

            //for (int pass = 0; pass < passes; pass++)
            //{

                effect.BeginPass(pass);
                Draw(subset);
                effect.EndPass();
            //}
            effect.End();
        }

        /// <summary>
        /// draw mesh
        /// </summary>
        /// <param name="dev">device</param>
        /// <param name="subset">subset</param>
        public void Draw(int subset)
        {
            meshObject.DrawSubset(subset);
        }

        public void Draw()
        {
            Draw(0);
        }

        /// <summary>
        /// draw mesh without material
        /// </summary>
        public void DrawMesh()
        {
            for (int i = 0; i < MaterialCount; i++)
                meshObject.DrawSubset(i);
        }


        /// <summary>
        /// draw mesh without material
        /// </summary>
        /// <param name="subset">subset</param>
        public void DrawMesh(int subset)
        {
            meshObject.DrawSubset(subset);
        }

        public MeshT Mesh
        {
            get
            {
                return meshObject;
            }
        }

        //TODO: cancellare quad
        /*    public static BaseMesh Quad<MaterialT>(MaterialT material) 
               where MaterialT : IMaterialContainer, new() 
            {
                GraphicsBuffer<PositionNormalTextured> buffer;
                PositionNormalTextured[] vertices = new PositionNormalTextured[4];
                VertexBuffer vertexBuffer = VertexBuffer.CreateGeneric<PositionNormalTextured>
                    (Game.Device, 4, Usage.None, PositionNormalTextured.Format, Pool.Managed, null);

                Mesh mesh = new Mesh(Game.Device, 2, 4, MeshFlags.Managed, );

                vertices[0] = new PositionNormalTextured(-1, 1, 0, 0, 0, 0, 0, 0);
                vertices[1] = new PositionNormalTextured(1, 1, 0, 0, 0, 0, 1, 0);
                vertices[2] = new PositionNormalTextured(-1, -1, 0, 0, 0, 0, 0, 1);
                vertices[3] = new PositionNormalTextured(1, -1, 0, 0, 0, 0, 1, 1);
            
                buffer = vertexBuffer.Lock<PositionNormalTextured>(0, 0, LockFlags.None)
                buffer.Write(vertices);
                vertexBuffer.Unlock();

                short[] buffI = this.LockIndexBuffer<short>();
                buffI[0] = 0;
                buffI[1] = 1;
                buffI[2] = 2;
                buffI[3] = 1;
                buffI[4] = 3;
                buffI[5] = 2;

                myMesh.UnlockIndexBuffer();

                this.materialList = new MaterialT[1];
                this.materialList[0] = material;

    
         */

        // <summary>
        // dispose meshObject and materials
        // </summary>
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
    }
}