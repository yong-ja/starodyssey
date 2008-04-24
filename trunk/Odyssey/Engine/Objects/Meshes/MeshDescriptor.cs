namespace AvengersUtd.Odyssey.Resources
{
    public struct MeshDescriptor
    {
        string label;
        string meshFilename;

        public string Label
        {
            get { return label; }
            set { label = value; }
        }

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