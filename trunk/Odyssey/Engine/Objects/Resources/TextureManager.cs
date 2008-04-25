using System.Windows.Forms;
using AvengersUtd.Odyssey.Engine;
using AvengersUtd.Utils.Collections;
using SlimDX.Direct3D9;
using System.IO;

namespace AvengersUtd.Odyssey.Resources
{
    public static class TextureManager
    {
        static Cache<string, CacheNode<Texture>> textureCache = new Cache<string, CacheNode<Texture>>();

        public static Texture LoadTexture(string filename)
        {
            if (textureCache.ContainsKey(filename))
            {
                return textureCache[filename].Object;
            }
            else
            {
                Texture texture;
                try
                {
                    texture = Texture.FromFile(Game.Device, filename);
                    SurfaceDescription desc = texture.GetLevelDescription(0);

                    textureCache.Add(filename, new CacheNode<Texture>(32*desc.Width*desc.Height, texture));
                    return texture;
                }
                catch (InvalidDataException ex)
                {
                    MessageBox.Show("You are missing this file:" +
                                    filename);
                    return null;
                }
            }
        }

        public static Texture[] LoadTextures(string[] filenames)
        {
            Texture[] textures = new Texture[filenames.Length];

            for (int i = 0; i < filenames.Length; i++)
            {
                textures[i] = LoadTexture(filenames[i]);
            }
            return textures;
        }

        public static void Dispose()
        {
            foreach (CacheNode<Texture> node in textureCache)
            {
                if (!node.Object.Disposed)
                    node.Object.Dispose();
            }
        }
    }
}