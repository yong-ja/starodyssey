namespace AvengersUtd.Odyssey.Objects.Materials
{
    public enum TextureType : int
    {
        Diffuse,
        Normal
    }


    public struct MaterialDescriptor
    {
        string label;
        string[] textureFilenames;

        public MaterialDescriptor(string label, params string[] texFilenames)
        {
            textureFilenames = texFilenames;
            this.label = label;
        }

        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        public string[] TextureFilenames
        {
            get { return textureFilenames; }
            set { textureFilenames = value; }
        }

        public string GetTextureFilename(TextureType type)
        {
            return textureFilenames[(int) type];
        }
    }
}