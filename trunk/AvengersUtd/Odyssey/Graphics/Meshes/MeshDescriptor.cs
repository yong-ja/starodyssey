using System.Xml.Serialization;

namespace AvengersUtd.Odyssey.Resources
{
    public struct MeshDescriptor
    {
        bool isDynamicallyBuilt;
        string meshFilename;

        public static MeshDescriptor Empty
        {
            get { return new MeshDescriptor(string.Empty); }
        }

        [XmlAttribute]
        public string MeshFilename
        {
            get { return meshFilename; }
            set { meshFilename = value; }
        }

        public bool IsDynamicallyBuilt
        {
            get { return isDynamicallyBuilt; }
        }

        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(meshFilename);}
        }

        public MeshDescriptor(string meshFilename) :
            this(meshFilename, false)
        {
        }

        public MeshDescriptor(string meshFilename, bool isDynamicallyBuilt)
        {
            this.meshFilename = meshFilename;
            this.isDynamicallyBuilt = isDynamicallyBuilt;
        }
    }
}