/*
* Copyright (c) 2007-2009 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
using System;
using System.Drawing;
using SlimDX;
using SlimDX.Direct3D10;
using SlimDX.DXGI;
using SlimDX.Windows;
using SlimDX.D3DCompiler;
using Device = SlimDX.Direct3D10.Device;

namespace MiniTri
{
    static class Program
    {

        [StructLayout(LayoutKind.Sequential)]
        struct NvStereoImageHeader
        {
            uint dwSignature;
            uint dwWidth;
            uint dwHeight;
            uint dwBPP;
            uint dwFlags;

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
            Texture2D tex = Texture2D.FromFile(Game.Context.Device, "medusa.jpg", info);

            ShaderResourceView srv = new ShaderResourceView(Game.Context.Device, Make3D(tex));
            quad.DiffuseMapResource = srv;
            rNode = new RenderableNode(quad);
            iMat = new ImageMaterial();
            FixedNode fNode = new FixedNode();
            fNode.Init();
            quad.PositionV3 = Layout.OrthographicTransform(Vector2.Zero, 100, new Size(1920, 1080));

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
0x20, 0x00, 0x00, 0x00,   //dwBPP = 32                       = 0x00000020;
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
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Write,
                Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Staging,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0)
            };

            ResourceRegion stereoSrcBox = new ResourceRegion { Front = 0, Back = 1, Top = 0, Bottom = 1080, Left = 0, Right = 3840 };


            Texture2D outp = new Texture2D(Game.Context.Device, desc);

            DeviceContext.Immediate.CopySubresourceRegion(tex, 0, stereoSrcBox, outp, 0, 0, 0, 0);
            //DeviceContext.Immediate.CopyResource(tex, outp);
            DataBox box = DeviceContext.Immediate.MapSubresource(outp, 0,
                outp.Description.Width * outp.Description.Height * 4, MapMode.Write, MapFlags.None);
            //var val = box.Data.ReadByte();
            box.Data.Seek(tex.Description.Width * tex.Description.Height * 4, SeekOrigin.Begin);

            //box.Data.Write(data, 0, data.Length);
            //byte[] color = BitConverter.GetBytes(Color.White.ToArgb());
            //box.Data.Write(color,0, color.Length);
            //box.Data.Write(data, 0, data.Length);
            byte[] headerData = StructureToByteArray(header);
            box.Data.Write(headerData, 0, headerData.Length);
            //box.Data.Write(color, 0, color.Length);
            DeviceContext.Immediate.UnmapSubresource(outp, 0);
            //box.Data.Write(buffer, 0, buffer.Length);

            //DeviceContext.Immediate.CopySubresourceRegion(tex, 0, stereoSrcBox, outp, 0, 0, 0, 0);
            ////return Texture2D.FromMemory(Game.Context.Device, buffer);
            //Texture2D.ToFile(Game.Context.Immediate, outp, ImageFileFormat.Bmp, "prova.bmp");

            Texture2DDescription finalDesc = new Texture2DDescription()
            {
                ArraySize = 1,
                Width = 3840,
                Height = 1081,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Dynamic,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0)
            };

            Texture2D finalT = new Texture2D(DeviceContext.Device, finalDesc);
            DeviceContext.Immediate.CopyResource(outp, finalT);

            //Texture2D.ToFile(DeviceContext.Immediate, finalT, ImageFileFormat.Bmp, "prova.bmp");


            return finalT;
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

        [STAThread]
        static void Main()
        {
            var form = new RenderForm("SlimDX - MiniTri Direct3D 10 Sample");
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(form.ClientSize.Width, form.ClientSize.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            Device device;
            SwapChain swapChain;
            Device.CreateWithSwapChain(null, DriverType.Hardware, DeviceCreationFlags.Debug, desc, out device, out swapChain);

            //Stops Alt+enter from causing fullscreen skrewiness.
            Factory factory = swapChain.GetParent<Factory>();
            factory.SetWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll);

            Texture2D backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            var renderView = new RenderTargetView(device, backBuffer);
            var effect = Effect.FromFile(device, "MiniTri.fx", "fx_4_0");
            var technique = effect.GetTechniqueByIndex(0);
            var pass = technique.GetPassByIndex(0);
            var layout = new InputLayout(device, pass.Description.Signature, new[] {
                new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0) 
            });

            var stream = new DataStream(3 * 32, true, true);
            stream.WriteRange(new[] {
                new Vector4(0.0f, 0.5f, 0.5f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                new Vector4(0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                new Vector4(-0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
            });
            stream.Position = 0;

            var vertices = new SlimDX.Direct3D10.Buffer(device, stream, new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = 3 * 32,
                Usage = ResourceUsage.Default
            });
            stream.Dispose();

            device.OutputMerger.SetTargets(renderView);
            device.Rasterizer.SetViewports(new Viewport(0, 0, form.ClientSize.Width, form.ClientSize.Height, 0.0f, 1.0f));

            MessagePump.Run(form, () =>
            {
                device.ClearRenderTargetView(renderView, Color.Black);

                device.InputAssembler.SetInputLayout(layout);
                device.InputAssembler.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
                device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 32, 0));

                for (int i = 0; i < technique.Description.PassCount; ++i)
                {
                    pass.Apply();
                    device.Draw(3, 0);
                }

                swapChain.Present(0, PresentFlags.None);
            });

            vertices.Dispose();
            layout.Dispose();
            effect.Dispose();
            renderView.Dispose();
            backBuffer.Dispose();
            device.Dispose();
            swapChain.Dispose();
            //foreach (var item in ObjectTable.Objects)
            //    item.Dispose();
        }

        public static TexturedVertex[] CreateTexturedQuad(Vector3 topLeftVertex, float width, float height,
                                                          out ushort[] indices)
        {
            TexturedVertex[] vertices = new[]
                                            {
                                                new TexturedVertex(
                                                    new Vector4(topLeftVertex.X, topLeftVertex.Y - height,
                                                                topLeftVertex.Z, 1.0f),
                                                    new Vector2(0.0f, 1.0f)),
                                                new TexturedVertex(
                                                    new Vector4(topLeftVertex.X, topLeftVertex.Y,
                                                                topLeftVertex.Z, 1.0f), new Vector2(0.0f, 0.0f)),
                                                new TexturedVertex(
                                                    new Vector4(topLeftVertex.X + width, topLeftVertex.Y,
                                                                topLeftVertex.Z, 1.0f), new Vector2(1.0f, 0.0f)),
                                                new TexturedVertex(
                                                    new Vector4(topLeftVertex.X + width, topLeftVertex.Y - height,
                                                                topLeftVertex.Z, 1.0f), new Vector2(1.0f, 1.0f))
                                            };
            indices = new ushort[]
                          {
                              2, 1, 0,
                              2, 0, 3
                          };

            return vertices;
        } 
    }
}