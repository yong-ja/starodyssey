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
            int bytes = data.Stride * image.Height;
            DataStream stream = new DataStream(bytes, true, true);
            stream.WriteRange(data.Scan0,bytes);
            stream.Position = 0;
            DataRectangle dRect = new DataRectangle(data.Stride, stream);

            Texture2DDescription texDesc = new Texture2DDescription
                                               {
                                                   ArraySize = 1,
                                                   MipLevels = 1,
                                                   SampleDescription = new SampleDescription(1,0),
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
            int width = texture.Description.Width;
            int height = texture.Description.Height;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(
               width, height,
               System.Drawing.Imaging.PixelFormat.Format32bppArgb
             );

            System.Drawing.Imaging.BitmapData bmpData = image.LockBits(
                           new System.Drawing.Rectangle(0, 0, width, height),
                           System.Drawing.Imaging.ImageLockMode.WriteOnly,
                           System.Drawing.Imaging.PixelFormat.Format32bppArgb
                         );

            IntPtr safePtr = bmpData.Scan0;
            byte[] textureData = new byte[4*width*height];
            System.Runtime.InteropServices.Marshal.Copy(textureData, 0, safePtr, textureData.Length);
            image.UnlockBits(bmpData);
            image.Save("prova.png", ImageFormat.Png);
            return image;
        }

        public static void SaveBitmapToDisk(string filename, Bitmap image)
        {
            if (string.IsNullOrEmpty(filename))
                return;

            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);

            FileStream fs = new FileStream(filename,FileMode.CreateNew);
            ms.Seek(0, SeekOrigin.Begin);
            fs.Write(ms.GetBuffer(),0,(int)ms.Length);
        }
    
//    Public Shared Function InitializeStandaloneImageCopy(ByVal strPathFile As String) As Image
//If strPathFile Is Nothing Then Return Nothing
//If strPathFile.Length <= 0 Then Return Nothing
//If Not FileExists(strPathFile) Then Return Nothing
//Dim fs As New FileStream(strPathFile, FileMode.Open, FileAccess.Read)
//Dim img As Image = Image.FromStream(fs)
//Dim imgClone As Image = CopyToStandaloneBitmap(img)
//img.Dispose()
//img = Nothing
//fs.Close()
//fs = Nothing
//Return imgClone
//End Function
    
    }
}
