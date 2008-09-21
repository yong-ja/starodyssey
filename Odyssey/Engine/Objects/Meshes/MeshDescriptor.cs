using System.Xml.Serialization;

namespace AvengersUtd.Odyssey.Resources
{
    public struct MeshDescriptor
    {
        string label;
        string meshFilename;

        [XmlAttribute]
        public string Label
        {
            get { return label; }
            set { label = value; }
        }
        [XmlAttribute]
        public string MeshFilename
        {
            get { return meshFilename; }
            set { meshFilename = value; }
        }

        public MeshDescriptor(string label, string meshFilename)
        {
            this.label = label;
            this.meshFilename = meshFilename;
        }
    }
}