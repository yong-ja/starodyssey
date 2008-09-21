using System.Xml.Serialization;

namespace AvengersUtd.Odyssey.Resources
{
    public struct MeshDescriptor
    {
        string meshFilename;

        [XmlAttribute]
        public string MeshFilename
        {
            get { return meshFilename; }
            set { meshFilename = value; }
        }

        public MeshDescriptor(string meshFilename)
        {
            this.meshFilename = meshFilename;
        }
    }
}