using System;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Meshes;
using AvengersUtd.Odyssey.Objects;

namespace AvengersUtd.Odyssey.Engine.Meshes
{
    /// <summary>
    /// base meshObject class
    /// </summary>
    /// <typeparam name="MaterialT">material type</typeparam>
    public class SimpleMesh<MaterialT> : AbstractMesh<Mesh, MaterialT>
        where MaterialT : IMaterialContainer, new()
    {
        int faceCount;
        int vertexCount;
        protected EntityDescriptor entityDescriptor;

        #region Properties

        public EntityDescriptor EntityDescriptor
        {
            get { return entityDescriptor; }
            set { entityDescriptor = value; }
        }

        #endregion

        #region Constructors

        public SimpleMesh(Mesh mesh)
        {
            Init(mesh);
        }

        /// <summary>
        /// Creates a new progressive meshObject.
        /// </summary>
        /// <param name="entityDesc">Contains data on the meshObject and texture file paths</param>
        public SimpleMesh(EntityDescriptor entityDesc)
        {
            //create the meshObject from file
            entityDescriptor = entityDesc;
            Init(EntityManager.LoadMesh(entityDesc.MeshDescriptor.MeshFilename));
        }

        #endregion

        void Init()
        {
            string filename = entityDescriptor.MeshDescriptor.MeshFilename;
            disposed = false;
            ExtendedMaterial[] list;

            meshObject = EntityManager.LoadMesh(filename);
            list = EntityManager.LoadMaterials(filename);

            //create adiacency buffer
            adiacency = meshObject.GenerateAdjacency(1e-6f);
            
           
            ComputeTangentsAndBinormal();

            //create material list array
            materials = new MaterialT[list.Length];

            //for each material description initialise the material list
            if (list.Length != entityDescriptor.TextureDescriptors.Length)
                throw new ArgumentException(
                    "Number of materials stored in the meshObject file do not match with the number of materials in the EntityDescriptor");

            int i = 0;
            foreach (ExtendedMaterial mate in list)
            {
                materials[i] = new MaterialT();
                materials[i].Material = mate.MaterialD3D;
                materials[i].TextureDescriptor = entityDescriptor.TextureDescriptors[i];
                i++;
            }
        }

        public void Init(Mesh mesh, TextureDescriptor texDescriptor)
        {
            meshObject = mesh;
            int[] adjacency = meshObject.GenerateAdjacency(1e-6f);
            
            ComputeTangentsAndBinormal();

            materials = new MaterialT[1];
            MaterialT material = new MaterialT();
            material.TextureDescriptor = texDescriptor;
            materials[0] = material;
        }

        public void Init(Mesh mesh)
        {
            meshObject = mesh;
            ComputeTangentsAndBinormal();
            materials = new MaterialT[1];
            MaterialT material = new MaterialT();
            materials[0] = material;
        }


        /// <summary>
        /// change vertex format
        /// </summary>
        /// <param name="dev">device</param>
        /// <param name="elements">elements description</param>
        public void ChangeVertexFormat(VertexElement[] elements)
        {
            meshObject = meshObject.Clone(Game.Device,MeshFlags.Managed, elements);
        }


        public void ComputeTangentsAndBinormal()
        {
            
            //VertexElement[] elements = meshObject.GetDeclaration();

            //elements[3] = new VertexElement(0,
            //                            32,
            //                            DeclarationType.Float3,
            //                            DeclarationMethod.Default,
            //                            DeclarationUsage.Tangent,
            //                            0);
            //elements[4] = new VertexElement(0,
            //                            32,
            //                            DeclarationType.Float3,
            //                            DeclarationMethod.Default,
            //                            DeclarationUsage.Binormal,
            //                            0);
            //elements[5] = VertexElement.VertexDeclarationEnd;
            
            VertexElement[] elements = new VertexElement[]{
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
                new VertexElement(0, 24, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                new VertexElement(0, 36, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Tangent, 0),
                new VertexElement(0, 48, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Binormal, 0),
                VertexElement.VertexDeclarationEnd
            };
            Mesh tempMesh = meshObject.Clone(Game.Device, MeshFlags.Managed, elements);
            meshObject.Dispose();
            meshObject = tempMesh;

            meshObject.ComputeNormals();
        }


        /// <summary>
        /// reset meshObject
        /// </summary>
        public void Reset()
        {
            Dispose(false);
        }
    }
}