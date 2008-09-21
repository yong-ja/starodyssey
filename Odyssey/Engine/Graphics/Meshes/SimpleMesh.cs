using System;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Graphics.Meshes;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    /// <summary>
    /// base meshObject class
    /// </summary>
    public class SimpleMesh : AbstractMesh<Mesh>
    {
        int faceCount;
        int vertexCount;
        IRenderable owningEntity;
        protected EntityDescriptor entityDescriptor;

        #region Properties

        public EntityDescriptor EntityDescriptor
        {
            get { return entityDescriptor; }
            set { entityDescriptor = value; }
        }

        public IRenderable OwningEntity
        {
            get { return owningEntity; }
            set { owningEntity = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new simple mesh object.
        /// </summary> 
        /// <param name="entity">The actual IRenderable object this mesh refers to.</param>
        /// <param name="entityDesc">Contains data on the meshObject and texture file paths</param>
        public SimpleMesh(IRenderable entity, EntityDescriptor entityDesc)
        {
            owningEntity = entity;
            //create the meshObject from file
            entityDescriptor = entityDesc;
            meshObject = EntityManager.LoadMesh(entityDesc.MeshDescriptor.MeshFilename);
        }

        #endregion

        public void Init()
        {

            materials = new AbstractMaterial[entityDescriptor.MaterialDescriptors.Length];

            for (int i = 0; i < entityDescriptor.MaterialDescriptors.Length; i++)
            {
                MaterialDescriptor mDesc = entityDescriptor.MaterialDescriptors[i];
                materials[i] = (AbstractMaterial) Activator.CreateInstance(mDesc.MaterialType);

                if (materials[i] is ITexturedMaterial)
                    ((ITexturedMaterial) materials[i]).LoadTextures(entityDescriptor.MaterialDescriptors[i]);
                materials[i].CreateEffect(owningEntity);
            }

            meshPartCollection = new MeshPartCollection(this, materials);

        }


        /// <summary>
        /// change vertex format
        /// </summary>
        /// <param name="elements">elements description</param>
        public void ChangeVertexFormat(VertexElement[] elements)
        {
            meshObject = meshObject.Clone(Game.Device, MeshFlags.Managed, elements);
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