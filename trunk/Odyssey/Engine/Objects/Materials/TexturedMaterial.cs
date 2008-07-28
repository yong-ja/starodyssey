using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Materials;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public interface ITexturedMaterial
    {
        void LoadTextures(MaterialDescriptor materialDescriptor);
    }

    public abstract class TexturedMaterial : AbstractMaterial, ITexturedMaterial
    {
        protected Texture diffuse;

        public Texture Diffuse
        {
            get { return diffuse; }
            set { diffuse = value; }
        }


        protected override void OnDisposing()
        {
            if (diffuse != null)
                diffuse.Dispose();
        }


        public virtual void LoadTextures(MaterialDescriptor materialDescriptor)
        {
            diffuse = TextureManager.LoadTexture(materialDescriptor[TextureType.Diffuse].TextureFilename);
        }
    }
}