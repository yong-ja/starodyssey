using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Collections;
using System.IO;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey.Graphics.Resources
{
    public static class ResourceManager
    {
        static readonly Cache<string, CacheNode<ShaderResourceView>> ResourceCache = new Cache<string, CacheNode<ShaderResourceView>>();
        
        public static void Add(string resourceKey, Texture2D texture)
        {
            Texture2DDescription description = texture.Description;
            int sizeInBytes = BitFromFormat(description.Format) * description.Width * description.Height;
            AddResource(resourceKey, texture, sizeInBytes);
        }

        public static void Add(string resourceKey, Texture3D texture)
        {
            Texture3DDescription description = texture.Description;
            int sizeInBytes = BitFromFormat(description.Format) * description.Width * description.Height;
            AddResource(resourceKey, texture, sizeInBytes);
        }

        static void AddResource(string resourceKey, SlimDX.Direct3D11.Resource resource, int sizeInBytes)
        {
            ShaderResourceView srv = new ShaderResourceView(Game.Context.Device, resource);
            ResourceCache.Add(resourceKey, new CacheNode<ShaderResourceView>(sizeInBytes, srv)); 
        }

        public static void Remove(string textureKey)
        {
            ShaderResourceView srv = ResourceCache.GetValue(textureKey).Object;
            srv.Resource.Dispose();
            srv.Dispose();
            ResourceCache.Remove(textureKey);
        }

        public static bool Contains(string resourceKey)
        {
            return ResourceCache.ContainsKey(resourceKey);
        }

        public static int BitFromFormat(Format textureFormat)
        {
            switch (textureFormat)
            {

                case Format.R1_UNorm:
                    return 1;

                case Format.A8_UNorm:
                case Format.R8_SInt:
                case Format.R8_SNorm:
                case Format.R8_UInt:
                case Format.R8_UNorm:
                case Format.R8_Typeless:
                    return 8;

                case Format.R16_SInt:
                case Format.R16_SNorm:
                case Format.R16_UInt:
                case Format.R16_UNorm:
                case Format.D16_UNorm:
                case Format.R16_Float:
                case Format.R16_Typeless:
                case Format.R8G8_SInt:
                case Format.R8G8_SNorm:
                case Format.R8G8_UInt:
                case Format.R8G8_UNorm:
                case Format.R8G8_Typeless:
                case Format.B5G5R5A1_UNorm:
                case Format.B5G6R5_UNorm:
                    return 16;

                case Format.B8G8R8X8_UNorm_SRGB:
                case Format.B8G8R8X8_Typeless:
                case Format.B8G8R8A8_UNorm_SRGB:
                case Format.B8G8R8A8_Typeless:
                case Format.B8G8R8X8_UNorm:
                case Format.B8G8R8A8_UNorm:
                    return 24;

                case Format.G8R8_G8B8_UNorm:
                case Format.R8G8_B8G8_UNorm:
                case Format.R9G9B9E5_SharedExp:
                case Format.R10G10B10_XR_Bias_A2_UNorm:
                case Format.X24_Typeless_G8_UInt:
                case Format.R24_UNorm_X8_Typeless:
                case Format.D24_UNorm_S8_UInt:
                case Format.R24G8_Typeless:
                case Format.R32_SInt:
                case Format.R32_UInt:
                case Format.R32_Float:
                case Format.D32_Float:
                case Format.R32_Typeless:
                case Format.R16G16_SInt:
                case Format.R16G16_SNorm:
                case Format.R16G16_UInt:
                case Format.R16G16_UNorm:
                case Format.R16G16_Float:
                case Format.R16G16_Typeless:
                case Format.R8G8B8A8_SInt:
                case Format.R8G8B8A8_SNorm:
                case Format.R8G8B8A8_UInt:
                case Format.R8G8B8A8_UNorm_SRGB:
                case Format.R8G8B8A8_UNorm:
                case Format.R8G8B8A8_Typeless:
                case Format.R11G11B10_Float:
                case Format.R10G10B10A2_UInt:
                case Format.R10G10B10A2_UNorm:
                case Format.R10G10B10A2_Typeless:
                    return 32;

                case Format.X32_Typeless_G8X24_UInt:
                case Format.R32_Float_X8X24_Typeless:
                case Format.D32_Float_S8X24_UInt:
                case Format.R32G8X24_Typeless:
                case Format.R32G32_SInt:
                case Format.R32G32_UInt:
                case Format.R32G32_Float:
                case Format.R32G32_Typeless:
                case Format.R16G16B16A16_SInt:
                case Format.R16G16B16A16_SNorm:
                case Format.R16G16B16A16_UInt:
                case Format.R16G16B16A16_UNorm:
                case Format.R16G16B16A16_Float:
                case Format.R16G16B16A16_Typeless:
                    return 64;

                case Format.R32G32B32_SInt:
                case Format.R32G32B32_UInt:
                case Format.R32G32B32_Float:
                case Format.R32G32B32_Typeless:
                    return 96;

                case Format.R32G32B32A32_SInt:
                case Format.R32G32B32A32_UInt:
                case Format.R32G32B32A32_Float:
                case Format.R32G32B32A32_Typeless:
                    return 128;

                case Format.BC7_UNorm_SRGB:
                case Format.BC7_UNorm:
                case Format.BC7_Typeless:
                case Format.BC6_SFloat16:
                case Format.BC6_UFloat16:
                case Format.BC6_Typeless:
                case Format.BC5_SNorm:
                case Format.BC5_UNorm:
                case Format.BC5_Typeless:
                case Format.BC4_SNorm:
                case Format.BC4_UNorm:
                case Format.BC4_Typeless:
                case Format.BC3_UNorm_SRGB:
                case Format.BC3_UNorm:
                case Format.BC3_Typeless:
                case Format.BC2_UNorm_SRGB:
                case Format.BC2_UNorm:
                case Format.BC2_Typeless:
                case Format.BC1_UNorm_SRGB:
                case Format.BC1_UNorm:
                case Format.BC1_Typeless:
                case Format.Unknown:
                    return -1;

                default:
                    throw Error.WrongCase("textureFormat", "BitFromFormat", textureFormat);
            }

        }

        public static Texture2D GetTexture(string resourceKey)
        {
            if (ResourceCache.ContainsKey(resourceKey))
            {
                return (Texture2D)ResourceCache[resourceKey].Object.Resource;
            }

            throw Error.KeyNotFound(resourceKey, "ResourceCache");
        }

        public static ShaderResourceView GetResource(string resourceKey)
        {
            if (ResourceCache.ContainsKey(resourceKey))
            {
                return ResourceCache[resourceKey].Object;
            }

            throw Error.KeyNotFound(resourceKey, "ResourceCache");
        }
        public static ShaderResourceView LoadTexture2DResource(string filename)
        {
            if (ResourceCache.ContainsKey(filename))
            {
                return ResourceCache[filename].Object;
            }
            else
            {
                try
                {
                    Texture2D texture2D = Texture2D.FromFile(Game.Context.Device, filename);
                    Add(filename, texture2D);
                    return ResourceCache[filename].Object;
                }
                catch (InvalidDataException)
                {
                    Error.MessageMissingFile(filename, Properties.Resources.ERR_MissingFile);
                    return null;
                }
            }
        }

        public static ShaderResourceView LoadTexture3DResource(string filename)
        {
            if (ResourceCache.ContainsKey(filename))
            {
                return ResourceCache[filename].Object;
            }
            else
            {
                try
                {
                    Texture3D texture3D = Texture3D.FromFile(Game.Context.Device, filename);
                    Add(filename, texture3D);
                    return ResourceCache[filename].Object;
                }
                catch (InvalidDataException)
                {
                    Error.MessageMissingFile(filename, Properties.Resources.ERR_MissingFile);
                    return null;
                }
            }
        }

        internal static void OnDispose(object sender, EventArgs e)
        {
            if (ResourceCache.IsEmpty)
                return;

            foreach (CacheNode<ShaderResourceView> node in ResourceCache)
            {
                if (!node.Object.Resource.Disposed)
                    node.Object.Resource.Dispose();

                if (!node.Object.Disposed)
                {

                    node.Object.Dispose();
                }
            }
        }

        public static void Dispose()
        {
            OnDispose(null, EventArgs.Empty);
        }
    }
}