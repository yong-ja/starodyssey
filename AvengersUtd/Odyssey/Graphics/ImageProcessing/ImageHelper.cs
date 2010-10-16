using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey.Graphics.ImageProcessing
{
    public static class ImageHelper
    {
        public static Texture2D TextureFromBitmap(Bitmap image)
        {
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                             ImageLockMode.ReadWrite, image.PixelFormat);
            int bytes = data.Stride*image.Height;
            DataStream stream = new DataStream(bytes, true, true);
            stream.WriteRange(data.Scan0, bytes);
            stream.Position = 0;
            DataRectangle dRect = new DataRectangle(data.Stride, stream);

            Texture2DDescription texDesc = new Texture2DDescription
                                               {
                                                   ArraySize = 1,
                                                   MipLevels = 1,
                                                   SampleDescription = new SampleDescription(1, 0),
                                                   Format = Format.B8G8R8A8_UNorm,
                                                   CpuAccessFlags = CpuAccessFlags.None,
                                                   BindFlags = BindFlags.ShaderResource,
                                                   Usage = ResourceUsage.Immutable,
                                                   Height = image.Height,
                                                   Width = image.Width
                                               };

            image.UnlockBits(data);
            image.Dispose();
            Texture2D texture = new Texture2D(Game.Context.Device, texDesc, dRect);
            stream.Dispose();
            return texture;
        }

        public static Bitmap BitmapFromTexture(Texture2D texture)
        {
            MemoryStream mem = new MemoryStream();
            Result result= Texture2D.ToStream(Game.Context.Immediate, texture, ImageFileFormat.Bmp, mem);

            if (result.IsFailure)
                throw Error.InvalidOperation(result.Description);

            return (Bitmap)Image.FromStream(mem);
        }

        public static void SaveBitmapToDisk(string filename, Bitmap image)
        {
            if (string.IsNullOrEmpty(filename))
                return;

            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);

            FileStream fs = new FileStream(filename, FileMode.CreateNew);
            ms.Seek(0, SeekOrigin.Begin);
            fs.Write(ms.GetBuffer(), 0, (int) ms.Length);
        }

    }
}