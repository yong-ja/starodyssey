using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Settings;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using System.Runtime.InteropServices;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    
    public class Stereo
    {
        // The NVSTEREO header.
        private static readonly byte[] data = new byte[]
                                         {
                                             0x4e, 0x56, 0x33, 0x44, //NVSTEREO_IMAGE_SIGNATURE         = 0x4433564e;
                                             0x00, 0x0F, 0x00, 0x00, //Screen width * 2 = 1920*2 = 3840 = 0x00000F00;
                                             0x38, 0x04, 0x00, 0x00, //Screen height = 1080             = 0x00000438;
                                             0x20, 0x00, 0x00, 0x00, //dwBPP = 32                       = 0x00000020;
                                             0x03, 0x00, 0x00, 0x00
                                         }; //dwFlags = SIH_SCALE_TO_FIT       = 0x00000002

        private static readonly NvStereoImageHeader nvHeader = new NvStereoImageHeader(0x4433564e, 3840, 1080, 32, 0x00000002);

        static Texture2D staging;

        public static Texture2D Make3D(Texture2D leftTexture, Texture2D rightTexture)
        {
            SlimDX.Direct3D11.Device device = Game.Context.Device;
            Size screenSize = Game.Context.Settings.ScreenSize;
            Texture2DDescription stagingDesc = new Texture2DDescription()
            {
                ArraySize = 1,
                Width = 2 * screenSize.Width,
                Height = screenSize.Height + 1,
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Write,
                Format = Format.R8G8B8A8_UNorm,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Staging,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0)
            };
            if (staging == null)
                staging = new Texture2D(device, stagingDesc);
            ResourceRegion stereoSrcBoxLeft = new ResourceRegion
            {
                Front = 0,
                Back = 1,
                Top = 0,
                Bottom = screenSize.Height,
                Left = 0,
                Right = screenSize.Width
            };
            ResourceRegion stereoSrcBoxRight = new ResourceRegion
            {
                Front = 0,
                Back = 1,
                Top = 0,
                Bottom = screenSize.Height,
                Left = screenSize.Width,
                Right = 2*screenSize.Width
            };
            device.ImmediateContext.CopySubresourceRegion(leftTexture, 0, stereoSrcBoxLeft, staging, 0, 0, 0, 0);
            device.ImmediateContext.CopySubresourceRegion(rightTexture, 0, stereoSrcBoxLeft, staging, 0, screenSize.Width, 0, 0);
            DataBox box = device.ImmediateContext.MapSubresource(staging, 0, MapMode.Write, SlimDX.Direct3D11.MapFlags.None);
   
            box.Data.Seek(box.RowPitch * screenSize.Height, System.IO.SeekOrigin.Begin);
            box.Data.Write(nvHeader.ToByteArray(), 0, data.Length);
            device.ImmediateContext.UnmapSubresource(staging, 0);

            // Create the final stereoized texture
            Texture2DDescription finalDesc = new Texture2DDescription()
            {
                ArraySize = 1,
                Width = 2 * screenSize.Width,
                Height = screenSize.Height + 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Dynamic,
                MipLevels = 1, 
                SampleDescription = new SampleDescription(1, 0), 
            };

            // Copy the staging texture on a new texture to be used as a shader resource
            Texture2D final = new Texture2D(device, finalDesc);
            device.ImmediateContext.CopyResource(staging, final);
            //staging.Dispose();
            return final;
        }


        public static Texture2D Make3D(Texture2D stereoTexture)
        {
            SlimDX.Direct3D11.Device device = Game.Context.Device;
            Size screenSize = Game.Context.Settings.ScreenSize;

            //NvStereoImageHeader header = new NvStereoImageHeader(0x4433564e, 3840, 1080, 4, 0x00000002);

            // stereoTexture contains a stereo image with the left eye image on the left half 
            // and the right eye image on the right half
            // this staging texture will have an extra row to contain the stereo signature
            Texture2DDescription stagingDesc = new Texture2DDescription()
            {
                ArraySize = 1,
                Width = 2 * screenSize.Width,
                Height = screenSize.Height + 1,
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Write,
                Format = Format.R8G8B8A8_UNorm,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Staging,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0)
            };
            Texture2D staging = new Texture2D(device, stagingDesc);

            // Identify the source texture region to copy (all of it)
            ResourceRegion stereoSrcBox = new ResourceRegion { Front = 0, Back = 1, Top = 0, Bottom = screenSize.Height, Left = 0, Right = 2 * screenSize.Width };
            // Copy it to the staging texture
            
            device.ImmediateContext.CopySubresourceRegion(stereoTexture, 0, stereoSrcBox, staging, 0, 0, 0, 0);

            // Open the staging texture for reading
            DataBox box = device.ImmediateContext.MapSubresource(staging, 0, MapMode.Write, SlimDX.Direct3D11.MapFlags.None);
            //DataRectangle box = staging.Map(0, MapMode.Write, SlimDX.Direct3D10.MapFlags.None);
            // Go to the last row

            box.Data.Seek(box.RowPitch*screenSize.Height, System.IO.SeekOrigin.Begin);
            //box.Data.Seek(box.Pitch * 1080, System.IO.SeekOrigin.Begin);
            // Write the NVSTEREO header
            box.Data.Write(data, 0, data.Length);
            device.ImmediateContext.UnmapSubresource(staging, 0);
            //staging.Unmap(0);

            // Create the final stereoized texture
            Texture2DDescription finalDesc = new Texture2DDescription()
            {
                ArraySize = 1,
                Width = 2 * screenSize.Width,
                Height = screenSize.Height + 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Dynamic,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0)
            };

            // Copy the staging texture on a new texture to be used as a shader resource
            Texture2D final = new Texture2D(device, finalDesc);
            device.ImmediateContext.CopyResource(staging, final);
            staging.Dispose();
            return final;
        }
    }
}
