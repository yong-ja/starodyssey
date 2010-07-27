using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SlimDX.Direct3D11;
using Gdi = System.Drawing;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Text
{
    public static class TextManager
    {
        
        public static Texture2D DrawText(string text)
        {
            //Our return value.
            Texture2D t;

            using (Gdi.Font font = new Gdi.Font("Arial", 12))
            {
                //The size of the rendered string.
                Gdi.SizeF strsize;

                //Create a dummy GDI objects so we can figure out the size of the string...
                using (Gdi.Bitmap dummyimage = new Gdi.Bitmap(1, 1))
                {
                    using (Gdi.Graphics dummygraphics = Gdi.Graphics.FromImage(dummyimage))
                    {
                        //Measure the string...
                        strsize = dummygraphics.MeasureString(text, font);
                    }
                }

                //Now create the REAL Drawing objects to render the text with.
                using (Gdi.Bitmap image = new Gdi.Bitmap((int)strsize.Width, (int)strsize.Height, Gdi.Imaging.PixelFormat.Format32bppArgb))
                {
                    using (Gdi.Graphics graphics = Gdi.Graphics.FromImage(image))
                    {
                        //Fill the rect with a transparent color.
                        graphics.Clear(Gdi.Color.FromArgb(0));

                        //Draw the text
                        using (Gdi.Brush brush = new Gdi.SolidBrush(Gdi.Color.FromArgb(255, 0, 255, 0)))
                        {
                            graphics.TextRenderingHint = Gdi.Text.TextRenderingHint.AntiAliasGridFit;
                            graphics.SmoothingMode = Gdi.Drawing2D.SmoothingMode.AntiAlias;
                            graphics.DrawString(text, font, brush, new Gdi.PointF(0, 0));
                        }

                        t = ConvertFromBitmap(image);
                    }
                }
            }
            Texture2D.ToFile(RenderForm11.Device., t, ImageFileFormat.Jpg, "Prova2.jpg");
            return t;
        }

        static Texture2D ConvertFromBitmap(Bitmap image)
        {
            System.Drawing.Imaging.BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
										System.Drawing.Imaging.ImageLockMode.ReadWrite, image.PixelFormat);
				IntPtr src = data.Scan0;
				int bytes = data.Stride * image.Height;
				byte[] dest = new byte[bytes];

				Marshal.Copy(src, dest, 0, bytes);
				image.UnlockBits(data);

				SlimDX.DXGI.SampleDescription sampleDesc = new SlimDX.DXGI.SampleDescription();
				sampleDesc.Count = 1;
				sampleDesc.Quality = 0;

            Texture2DDescription texDesc = new Texture2DDescription()
                                               {
                                                   ArraySize = 1,
                                                   MipLevels = 1,
                                                   SampleDescription = sampleDesc,
                                                   Format = SlimDX.DXGI.Format.R8_UInt,
                                                   CpuAccessFlags = CpuAccessFlags.Write,
                                                   BindFlags = BindFlags.ShaderResource,
                                                   Usage = ResourceUsage.Dynamic,
                                                   Height = image.Height,
                                                   Width = image.Width
                                               };

                return new Texture2D(RenderForm11.Device, texDesc);
            
        }
    }
}
