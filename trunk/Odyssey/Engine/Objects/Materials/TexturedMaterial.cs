using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Materials;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public class TexturedMaterial : AbstractMaterial
    {
        protected Texture diffuse;

        public Texture Diffuse
        {
            get { return diffuse; }
        }

        public override TextureDescriptor TextureDescriptor
        {
            get
            {
                return base.TextureDescriptor;
            }
            set
            {
                base.TextureDescriptor = value;
                diffuse = TextureManager.LoadTexture(textureDescriptor.GetTextureFilename(TextureType.Diffuse));
            }
        }


        public override void Apply()
        {
            Device device = Game.Device;
            device.Material = material;
            device.SetTexture(0, diffuse);
        }

        public override void Dispose()
        {
            if (diffuse != null)
                diffuse.Dispose();
        }

        public override bool Disposed
        {
            get { return diffuse.Disposed; }
        }
    }
}