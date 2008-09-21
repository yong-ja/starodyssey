using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Collections;
using AvengersUtd.Odyssey.Engine;
using AvengersUtd.Odyssey.Objects;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.Odyssey.Utils.Xml;
using SlimDX.Direct3D9;
using System.IO;
using SlimDX;


namespace AvengersUtd.Odyssey.Resources
{
    /// <summary>
    /// Offers utility method to be used in the engine
    /// </summary>
    public static class EntityManager
    {
        static Cache<string, CacheNode<Mesh>> meshCache = new Cache<string, CacheNode<Mesh>>(64 * 1024*1024);

        static SortedDictionary<string, EntityDescriptor> entityDescriptorCache =
            new SortedDictionary<string, EntityDescriptor>();

        public static bool ContainsMesh(string filename)
        {
            return meshCache.ContainsKey(filename);
        }

        public static void LoadDescriptors(string filename)
        {
            EntityDescriptor[] descriptors =
                Data.DeserializeCollection<EntityDescriptor>(filename);

            foreach (EntityDescriptor descriptor in descriptors)
                entityDescriptorCache.Add(descriptor.Label, descriptor);
        }

        public static EntityDescriptor GetEntityDescriptor(string label)
        {
            return entityDescriptorCache[label];
        }

        public static void SetMesh(string filename, Mesh mesh)
        {
            meshCache[filename] = new CacheNode<Mesh>(mesh.BytesPerVertex*mesh.VertexCount, mesh);
        }


        public static Mesh LoadMesh(string filename)
        {
            if (meshCache.ContainsKey(filename))
            {
                return meshCache[filename].Object;
            }
            else
            {
                try
                {
                    Mesh mesh = Mesh.FromFile(Game.Device, filename, MeshFlags.Managed);
                    mesh = ComputeTangentsAndBinormal(mesh);
                    meshCache.Add(filename, new CacheNode<Mesh>(mesh.BytesPerVertex*mesh.VertexCount, mesh));

                    return mesh;
                }
                catch (InvalidDataException ex)
                {
                    MessageBox.Show("You are missing this file: " + filename);

                    return null;
                }
            }
        }

        static Mesh ComputeTangentsAndBinormal(Mesh meshObject)
        {

            VertexElement[] elements = new VertexElement[]
                                           {
                                               new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default,
                                                                 DeclarationUsage.Position, 0),
                                               new VertexElement(0, 12, DeclarationType.Float3,
                                                                 DeclarationMethod.Default, DeclarationUsage.Normal, 0),
                                               new VertexElement(0, 24, DeclarationType.Float2,
                                                                 DeclarationMethod.Default,
                                                                 DeclarationUsage.TextureCoordinate, 0),
                                               new VertexElement(0, 32, DeclarationType.Float3,
                                                                 DeclarationMethod.Default, DeclarationUsage.Tangent, 0),
                                               new VertexElement(0, 44, DeclarationType.Float3,
                                                                 DeclarationMethod.Default, DeclarationUsage.Binormal, 0),
                                               VertexElement.VertexDeclarationEnd
                                           };
            Mesh tempMesh = meshObject.Clone(Game.Device, MeshFlags.Managed, elements);
            meshObject.Dispose();
            meshObject = tempMesh;

            meshObject.ComputeTangent(0, 0, 0, false);
            return meshObject;
        }

        public static void Dispose()
        {
            if (meshCache.IsEmpty)
                return;

            foreach (CacheNode<Mesh> node in meshCache)
            {
                if (!node.Object.Disposed)
                    node.Object.Dispose();   
            }
        }
    }
}