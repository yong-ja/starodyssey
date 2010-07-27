using System.Xml.Serialization;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Resources
{
    public struct EntityDescriptor
    {
        string label;
        MeshDescriptor meshDescriptor;
        MaterialDescriptor[] materialDescriptors;

        #region Properties
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
        #endregion

        public EntityDescriptor(string label, MeshDescriptor meshDescriptor, MaterialDescriptor materials) :
            this(label, meshDescriptor, new[] {materials})
        {
        }

        public EntityDescriptor(string label, MaterialDescriptor materialDescriptor) :
            this(label, new MeshDescriptor(label, true), materialDescriptor)
        {}

        public EntityDescriptor(string label, MeshDescriptor meshDescriptor, MaterialDescriptor[] materials)
        {
            this.label = label;
            this.meshDescriptor = meshDescriptor;
            materialDescriptors = materials;
        }
    }
}