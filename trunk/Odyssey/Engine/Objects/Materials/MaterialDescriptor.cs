namespace AvengersUtd.Odyssey.Objects.Materials
{
    public enum TextureType : int
    {
        Diffuse,
        Normal,
        Texture1 
    }


    public struct TextureDescriptor
    {
        string label;
        string[] textureFilenames;

        public TextureDescriptor(string label)
        {
            this.label = label;
            textureFilenames = new string[0];
        }

        public TextureDescriptor(string label, params string[] texFilenames)
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