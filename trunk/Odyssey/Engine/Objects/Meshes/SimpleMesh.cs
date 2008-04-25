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

        public SimpleMesh()
        {
        }

        /// <summary>
        /// Creates a new progressive meshObject.
        /// </summary>
        /// <param name="entityDesc">Contains data on the meshObject and texture file paths</param>
        public SimpleMesh(EntityDescriptor entityDesc)
        {
            //create the meshObject from file
            entityDescriptor = entityDesc;
        }

        #endregion

        public void Init()
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
            if (list.Length != entityDescriptor.MaterialDescriptors.Length)
                throw new ArgumentException(
                    "Number of materials stored in the meshObject file do not match with the number of materials in the EntityDescriptor");

            int i = 0;
            foreach (ExtendedMaterial mate in list)
            {
                materials[i] = new MaterialT();
                materials[i].Init(mate.MaterialD3D, entityDescriptor.MaterialDescriptors[i]);
                i++;
            }
        }

        public void Init(Mesh mesh, MaterialDescriptor matDescriptor)
        {
            meshObject = mesh;
            int[] adjacency = meshObject.GenerateAdjacency(1e-6f);
            

            //original.ComputeNormals(adjacency);
            //ComputeTangentsAndBinormal();
            //meshObject.Simplify(original, adjacency, 10, MeshFlags.SimplifyFace);

            ComputeTangentsAndBinormal();
            //meshObject newMesh = meshObject.Clean(CleanType.Simplification, original, adjacency, out adjacency2);

            //meshObject = new ProgressiveMesh(newMesh, adjacency2, 10, MeshFlags.SimplifyFace);
            //faceCount = original.NumberFaces;
            materials = new MaterialT[1];
            MaterialT material = new MaterialT();
            material.Init(new Material(), matDescriptor);
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
            /*
            VertexElement[] decl = meshObject.GetDeclaration();

            decl[3] = new VertexElement(0,
                                        32,
                                        DeclarationType.Float3,
                                        DeclarationMethod.Default,
                                        DeclarationUsage.Tangent,
                                        0);
            decl[4] = new VertexElement(0,
                                        32,
                                        DeclarationType.Float3,
                                        DeclarationMethod.Default,
                                        DeclarationUsage.Binormal,
                                        0);
            decl[5] = VertexElement.VertexDeclarationEnd;
             */
            VertexElement[] elements = new VertexElement[]{
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
				new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
				new VertexElement(0, 24, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
				new VertexElement(0, 36, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Tangent, 0),
				new VertexElement(0, 48, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Binormal, 0),
				VertexElement.VertexDeclarationEnd
            };
            meshObject = meshObject.Clone(Game.Device,MeshFlags.Managed, elements);
            meshObject.ComputeTangentFrame(TangentOptions.CalculateNormals);
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