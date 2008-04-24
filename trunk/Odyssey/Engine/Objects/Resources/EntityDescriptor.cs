using AvengersUtd.Odyssey.Objects.Materials;
namespace AvengersUtd.Odyssey.Resources
{
    public struct EntityDescriptor
    {
        MeshDescriptor meshDescriptor;
        MaterialDescriptor[] materialDescriptors;


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

        public EntityDescriptor(MeshDescriptor meshDescriptor, params MaterialDescriptor[] materials)
        {
            this.meshDescriptor = meshDescriptor;
            materialDescriptors = materials;
        }
    }
}