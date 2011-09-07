using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.Settings;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.StarOdyssey.Properties;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using MapFlags = SlimDX.Direct3D11.MapFlags;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class ImageRenderer : Renderer
    {
        private string diffuseMapKey;

        public ImageRenderer(DeviceContext11 deviceContext11) : base(deviceContext11)
        {
        }

          [StructLayout(LayoutKind.Sequential)]
        struct NvStereoImageHeader
        {
            uint    dwSignature;
            uint    dwWidth;
            uint    dwHeight;
            uint    dwBPP;
            uint    dwFlags;

            public NvStereoImageHeader(uint dwSignature, uint dwWidth, uint dwHeight, uint dwBpp, uint dwFlags)
            {
                this.dwSignature = dwSignature;
                this.dwWidth = dwWidth;
                this.dwHeight = dwHeight;
                dwBPP = dwBpp;
                this.dwFlags = dwFlags;
            }
        } 

        private RenderCommand rCommand;
        private RenderableNode rNode;
        private ImageMaterial iMat;

        public override void Init()
        {
            TexturedPolygon quad = TexturedPolygon.CreateTexturedQuad(Vector3.Zero, 1920, 1080);
            ImageLoadInformation info = new ImageLoadInformation()
            {
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Read,
                FilterFlags = FilterFlags.None,
                Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm,
                MipFilterFlags = FilterFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Staging,
                MipLevels = 1
            };
            Texture2D tex = Texture2D.FromFile(Game.Context.Device, "medusa.jpg",  info);
           
            ShaderResourceView srv = new ShaderResourceView(Game.Context.Device,  Make3D(tex));
            quad.DiffuseMapResource = srv;
            rNode = new RenderableNode(quad);
            iMat = new ImageMaterial();
            FixedNode fNode = new FixedNode();
            fNode.Init();
            quad.PositionV3 = Layout.OrthographicTransform(Vector2.Zero, 100, new Size(1920,1080));

            RenderableCollection rCol = new RenderableCollection(iMat.RenderableCollectionDescription);
            MaterialNode mMat = new MaterialNode(iMat);
            rCol.Add(rNode);

            fNode.AppendChild(mMat);
            mMat.AppendChild(rNode);

            rCommand = new RenderCommand(mMat, rCol);
            rCommand.Init();
            DeviceContext.Immediate.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            Scene.Tree.RootNode.AppendChild(fNode);
            Scene.BuildRenderScene();
        }

        byte[] data = new byte[] {0x4e, 0x56, 0x33, 0x44,   //NVSTEREO_IMAGE_SIGNATURE         = 0x4433564e;
0x00, 0x0F, 0x00, 0x00,   //Screen width * 2 = 1920*2 = 3840 = 0x00000F00;
0x38, 0x04, 0x00, 0x00,   //Screen height = 1080             = 0x00000438;
0x04, 0x00, 0x00, 0x00,   //dwBPP = 32                       = 0x00000020;
0x02, 0x00, 0x00, 0x00};  //dwFlags = SIH_SCALE_TO_FIT       = 0x00000002

        Texture2D Make3D(Texture2D tex)
        {
            NvStereoImageHeader header = new NvStereoImageHeader(0x4433564e, 3840, 1080, 4, 0x00000002);
           // DataBox box = DeviceContext.Immediate.MapSubresource(tex, 0,
           //     tex.Description.Width * tex.Description.Height * 4, MapMode.Read, MapFlags.None);
           // Console.WriteLine(box.RowPitch);
           // int size = tex.Description.Width * tex.Description.Height * 8;
           // byte[] buffer = new byte[size/2];
           // int val = box.Data.ReadRange<byte>(buffer, 0, size/2);
           // DeviceContext.Immediate.UnmapSubresource(tex, 0);
            Texture2DDescription desc = new Texture2DDescription()
                                             {
                                                 ArraySize = 1,
                                                 Width = 3840,
                                                 Height = 1081,
                                                 BindFlags = BindFlags.ShaderResource,
                                                 CpuAccessFlags = CpuAccessFlags.Write,
                                                 Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm,
                                                 OptionFlags = ResourceOptionFlags.None,
                                                 Usage = ResourceUsage.Default,
                                                 MipLevels = 1,
                                                 SampleDescription = new SampleDescription(1, 0)
                                             };

            ResourceRegion stereoSrcBox = new ResourceRegion();

            stereoSrcBox.Front = 0;
            stereoSrcBox.Back = 1;
            stereoSrcBox.Top = 0;
            stereoSrcBox.Bottom = 1080;
            stereoSrcBox.Left = 0;
            stereoSrcBox.Right = 3840;
 
            Texture2D outp = new Texture2D(Game.Context.Device, desc);

            DeviceContext.Immediate.CopySubresourceRegion(tex, 0, stereoSrcBox, outp, 0, 0, 0, 0);
            //DeviceContext.Immediate.CopyResource(tex, outp);
            DataBox box = DeviceContext.Immediate.MapSubresource(outp, 0,
                outp.Description.Width * outp.Description.Height * 4, MapMode.WriteNoOverwrite, MapFlags.None);
            //var val = box.Data.ReadByte();
            box.Data.Seek(tex.Description.Width * tex.Description.Height * 4, SeekOrigin.Begin);

            box.Data.Write(data, 0, data.Length);
            DeviceContext.Immediate.UnmapSubresource(outp, 0);

          //box.Data.Write(buffer, 0, buffer.Length);
           
            //DeviceContext.Immediate.CopySubresourceRegion(tex, 0, stereoSrcBox, outp, 0, 0, 0, 0);
            ////return Texture2D.FromMemory(Game.Context.Device, buffer);
            //Texture2D.ToFile(Game.Context.Immediate, outp, ImageFileFormat.Bmp, "prova.bmp");
            return outp;
        }

        static byte[] StructureToByteArray(object obj)
        {

            int len = Marshal.SizeOf(obj);

            byte[] arr = new byte[len];

            IntPtr ptr = Marshal.AllocHGlobal(len);

            Marshal.StructureToPtr(obj, ptr, true);

            Marshal.Copy(ptr, arr, 0, len);

            Marshal.FreeHGlobal(ptr);

            return arr;

        }

        public override void Render()
        {
            Scene.Display();
        }

        public override void ProcessInput()
        {
            //throw new NotImplementedException();
        }
    }
}
