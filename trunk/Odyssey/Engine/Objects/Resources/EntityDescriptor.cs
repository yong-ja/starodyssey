using AvengersUtd.Odyssey.Objects.Materials;
namespace AvengersUtd.Odyssey.Resources
{
    public struct EntityDescriptor
    {
        MeshDescriptor meshDescriptor;
        TextureDescriptor[] textureDescriptors;


        public MeshDescriptor MeshDescriptor
        {
            get { return meshDescriptor; }
            set { meshDescriptor = value; }
        }


        public TextureDescriptor[] TextureDescriptors
        {
            get { return textureDescriptors; }
            set { textureDescriptors = value; }
        }

        public EntityDescriptor(MeshDescriptor meshDescriptor, params TextureDescriptor[] materials)
        {
            this.meshDescriptor = meshDescriptor;
            textureDescriptors = materials;
        }
    }
}