using System.Xml.Serialization;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Resources
{
    public struct EntityDescriptor
    {
        string label;
        MeshDescriptor meshDescriptor;
        MaterialDescriptor[] materialDescriptors;

        [XmlAttribute]
        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        public MeshDescriptor MeshDescriptor
        {
            get { return meshDescriptor; }
            set { meshDescriptor = value; }
        }

        public MaterialDescriptor[] MaterialDescriptors
        {
            get { return materialDescriptors; }
            set { materialDescriptors = value; }
        }

        public EntityDescriptor(string label, MeshDescriptor meshDescriptor, MaterialDescriptor materials) :
            this(label, meshDescriptor, new[] {materials})
        {
        }

        public EntityDescriptor(string label, MeshDescriptor meshDescriptor, MaterialDescriptor[] materials)
        {
            this.label = label;
            this.meshDescriptor = meshDescriptor;
            materialDescriptors = materials;
        }
    }
}