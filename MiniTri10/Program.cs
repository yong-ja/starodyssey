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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using SlimDX.Direct3D10;
using SlimDX.DXGI;
using SlimDX.Windows;
using SlimDX.D3DCompiler;
using Buffer = SlimDX.Direct3D10.Buffer;
using Device = SlimDX.Direct3D10.Device;
using Resource = SlimDX.Direct3D10.Resource;

namespace MiniTri
{
    internal static class Program
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct NvStereoImageHeader
        {
            private uint dwSignature;
            private uint dwWidth;
            private uint dwHeight;
            private uint dwBPP;
            private uint dwFlags;

            public NvStereoImageHeader(uint dwSignature, uint dwWidth, uint dwHeight, uint dwBpp, uint dwFlags)
            {
                this.dwSignature = dwSignature;
                this.dwWidth = dwWidth;
                this.dwHeight = dwHeight;
                dwBPP = dwBpp;
                this.dwFlags = dwFlags;
            }
        }

        //private RenderCommand rCommand;
        //private RenderableNode rNode;
        //private ImageMaterial iMat;

        // The NVSTEREO header.
        private static byte[] data = new byte[]
                                         {
                                             0x4e, 0x56, 0x33, 0x44, //NVSTEREO_IMAGE_SIGNATURE         = 0x4433564e;
                                             0x00, 0x0F, 0x00, 0x00, //Screen width * 2 = 1920*2 = 3840 = 0x00000F00;
                                             0x38, 0x04, 0x00, 0x00, //Screen height = 1080             = 0x00000438;
                                             0x20, 0x00, 0x00, 0x00, //dwBPP = 32                       = 0x00000020;
                                             0x03, 0x00, 0x00, 0x00
                                         }; //dwFlags = SIH_SCALE_TO_FIT       = 0x00000002

        private static Size size = new Size(1920, 1080);

        private static Texture2D Make3D(Texture2D stereoTexture)
        {

            NvStereoImageHeader header = new NvStereoImageHeader(0x4433564e, 3840, 1080, 4, 0x00000002);

            // stereoTexture contains a stereo image with the left eye image on the left half 
            // and the right eye image on the right half
            // this staging texture will have an extra row to contain the stereo signature
            Texture2DDescription stagingDesc = new Texture2DDescription()
                                                   {
                                                       ArraySize = 1,
                                                       Width = 2*size.Width,
                                                       Height = size.Height + 1,
                                                       BindFlags = BindFlags.None,
                                                       CpuAccessFlags = CpuAccessFlags.Write,
                                                       Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm,
                                                       OptionFlags = ResourceOptionFlags.None,
                                                       Usage = ResourceUsage.Staging,
                                                       MipLevels = 1,
                                                       SampleDescription = new SampleDescription(1, 0)
                                                   };
            Texture2D staging = new Texture2D(device, stagingDesc);

            // Identify the source texture region to copy (all of it)
            ResourceRegion stereoSrcBox = new ResourceRegion
                                              {Front = 0, Back = 1, Top = 0, Bottom = size.Height, Left = 0, Right = 2*size.Width};
            // Copy it to the staging texture
            device.CopySubresourceRegion(stereoTexture, 0, stereoSrcBox, staging, 0, 0, 0, 0);

            // Open the staging texture for reading
            DataRectangle box = staging.Map(0, MapMode.Write, SlimDX.Direct3D10.MapFlags.None);
            // Go to the last row
            //box.Data.Seek(stereoTexture.Description.Width*stereoTexture.Description.Height*4, System.IO.SeekOrigin.Begin);
            box.Data.Seek(box.Pitch*1080, System.IO.SeekOrigin.Begin);
            // Write the NVSTEREO header
            box.Data.Write(data, 0, data.Length);
            staging.Unmap(0);

            // Create the final stereoized texture
            Texture2DDescription finalDesc = new Texture2DDescription()
                                                 {
                                                     ArraySize = 1,
                                                     Width = 2 * size.Width,
                                                     Height = size.Height + 1,
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
            device.CopyResource(staging, final);
            staging.Dispose();
            return final;
        }

        private static byte[] StructureToByteArray(object obj)
        {
            int len = Marshal.SizeOf(obj);

            byte[] arr = new byte[len];

            IntPtr ptr = Marshal.AllocHGlobal(len);

            Marshal.StructureToPtr(obj, ptr, true);

            Marshal.Copy(ptr, arr, 0, len);

            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        private static Device device;
        private static SwapChain swapChain;
        private static RenderTargetView renderView,rv;
        private static Texture2D backBuffer,rtTex, sourceTexture, stereoizedTexture;
        private static ShaderResourceView srv;
        private static Factory factory;
        private static Effect effect;

        [STAThread]
        private static void Main()
        {
            // Device creation
            var form = new RenderForm("Stereo test")
                           {
                               ClientSize = size,
                               //FormBorderStyle = System.Windows.Forms.FormBorderStyle.None,
                               //WindowState = FormWindowState.Maximized
                           };

            form.KeyDown += new KeyEventHandler(form_KeyDown);
           // form.Resize += new EventHandler(form_Resize);

            ModeDescription mDesc = new ModeDescription(form.ClientSize.Width, form.ClientSize.Height,
                                                        new Rational(120000, 1000), Format.R8G8B8A8_UNorm);
            mDesc.ScanlineOrdering = DisplayModeScanlineOrdering.Progressive;
            mDesc.Scaling = DisplayModeScaling.Unspecified;

            var desc = new SwapChainDescription()
                           {
                               BufferCount = 1,
                               ModeDescription = mDesc,
                                   Flags = SwapChainFlags.AllowModeSwitch,
                               IsWindowed = false,
                               OutputHandle = form.Handle,
                               SampleDescription = new SampleDescription(1, 0),
                               SwapEffect = SwapEffect.Discard,
                               Usage = Usage.RenderTargetOutput
                           };

            Device.CreateWithSwapChain(null, DriverType.Hardware, DeviceCreationFlags.Debug, desc, out device,
                                       out swapChain);

            //Stops Alt+enter from causing fullscreen skrewiness.
            factory = swapChain.GetParent<Factory>();
            factory.SetWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll);

            backBuffer = Resource.FromSwapChain<Texture2D>(swapChain, 0);
            renderView = new RenderTargetView(device, backBuffer);

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

            // Make texture 3D
            sourceTexture = Texture2D.FromFile(device, "medusa.jpg", info);
            ImageLoadInformation info2 = new ImageLoadInformation()
                                            {
                                                BindFlags = BindFlags.ShaderResource,
                                                CpuAccessFlags = CpuAccessFlags.None,
                                                FilterFlags = FilterFlags.None,
                                                Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm,
                                                MipFilterFlags = FilterFlags.None,
                                                OptionFlags = ResourceOptionFlags.None,
                                                Usage = ResourceUsage.Default,
                                                MipLevels = 1
                                            };
            Texture2D tShader = Texture2D.FromFile(device, "medusa.jpg", info2);
            srv = new ShaderResourceView(device, tShader);
            //ResizeDevice(new Size(1920, 1080), true);
            // Create a quad that fills the whole screen

            BuildQuad();
            // Create world view (ortho) projection matrices
            QuaternionCam qCam = new QuaternionCam();

            // Load effect from file. It is a basic effect that renders a full screen quad through 
            // an ortho projectio=n matrix
            effect = Effect.FromFile(device, "Texture.fx", "fx_4_0", ShaderFlags.Debug, EffectFlags.None);
            EffectTechnique technique = effect.GetTechniqueByIndex(0);
            EffectPass pass = technique.GetPassByIndex(0);
            InputLayout layout = new InputLayout(device, pass.Description.Signature, new[]
                                                                                         {
                                                                                             new InputElement(
                                                                                                 "POSITION", 0,
                                                                                                 Format.
                                                                                                     R32G32B32A32_Float,
                                                                                                 0, 0),
                                                                                             new InputElement(
                                                                                                 "TEXCOORD", 0,
                                                                                                 Format.
                                                                                                     R32G32_Float,
                                                                                                 16, 0)
                                                                                         });
            effect.GetVariableByName("mWorld").AsMatrix().SetMatrix(
                Matrix.Translation(Layout.OrthographicTransform(Vector2.Zero, 99, size)));
            effect.GetVariableByName("mView").AsMatrix().SetMatrix(qCam.View);
            effect.GetVariableByName("mProjection").AsMatrix().SetMatrix(qCam.OrthoProjection);
            effect.GetVariableByName("tDiffuse").AsResource().SetResource(srv);

            // Set RT and Viewports
            device.OutputMerger.SetTargets(renderView);
            device.Rasterizer.SetViewports(new Viewport(0, 0, size.Width, size.Height, 0.0f, 1.0f));

            // Create solid rasterizer state
            RasterizerStateDescription rDesc = new RasterizerStateDescription()
                                                   {
                                                       CullMode = CullMode.None,
                                                       IsDepthClipEnabled = true,
                                                       FillMode = FillMode.Solid,
                                                       IsAntialiasedLineEnabled = false,
                                                       IsFrontCounterclockwise = true,
                                                       //IsMultisampleEnabled = true,
                                                   };
            RasterizerState rState = RasterizerState.FromDescription(device, rDesc);
            device.Rasterizer.State = rState;

            Texture2DDescription rtDesc = new Texture2DDescription
                                              {
                                                  ArraySize = 1,
                                                  Width = size.Width,
                                                  Height = size.Height,
                                                  BindFlags = BindFlags.RenderTarget,
                                                  CpuAccessFlags = CpuAccessFlags.None,
                                                  Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm,
                                                  OptionFlags = ResourceOptionFlags.None,
                                                  Usage = ResourceUsage.Default,
                                                  MipLevels = 1,
                                                  SampleDescription = new SampleDescription(1, 0)
                                              };
            rtTex = new Texture2D(device, rtDesc);

            rv = new RenderTargetView(device, rtTex);


            stereoizedTexture = Make3D(sourceTexture);
            //ResizeDevice(new Size(1920, 1080), true);
            Console.WriteLine(form.ClientSize);
            // Main Loop
MessagePump.Run(form, () =>
    {
        device.ClearRenderTargetView(renderView, Color.Cyan);

        //device.InputAssembler.SetInputLayout(layout);
        //device.InputAssembler.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
        //device.OutputMerger.SetTargets(rv);
        //device.InputAssembler.SetVertexBuffers(0,
        //                                new VertexBufferBinding(vertices, 24, 0));
        //device.InputAssembler.SetIndexBuffer(indices, Format.R16_UInt, 0);
        //for (int i = 0; i < technique.Description.PassCount; ++i)
        //{
        //    // Render the full screen quad
        //    pass.Apply();
        //    device.DrawIndexed(6, 0, 0);
        //}
        ResourceRegion stereoSrcBox = new ResourceRegion { Front = 0, Back = 1, Top = 0, Bottom = size.Height, Left = 0, Right = size.Width };
        device.CopySubresourceRegion(stereoizedTexture, 0, stereoSrcBox, backBuffer, 0, 0, 0, 0);
        //device.CopyResource(rv.Resource, backBuffer);
                    
        swapChain.Present(0, PresentFlags.None);
    });

            // Dispose resources
            vertices.Dispose();
            layout.Dispose();
            effect.Dispose();
            renderView.Dispose();
            backBuffer.Dispose();
            device.Dispose();
            swapChain.Dispose();

            rState.Dispose();
            stereoizedTexture.Dispose();
            sourceTexture.Dispose();
            indices.Dispose();
            srv.Dispose();
        }

        static void form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.Enter)
                ResizeDevice(size, !swapChain.IsFullScreen);
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

        public static void ResizeDevice(Size newSize, bool fullScreen)
        {
            //OnDeviceSuspend(EventArgs.Empty);

            backBuffer.Dispose();
            renderView.Dispose();
            vertices.Dispose();
            indices.Dispose();
            rv.Dispose();
            device.ClearState();
            
            SwapChainDescription swapChainDesc = swapChain.Description;
            Output output = factory.GetAdapter(0).GetOutput(0);
            
            var modes = output.GetDisplayModeList(Format.R8G8B8A8_UNorm, DisplayModeEnumerationFlags.Scaling | DisplayModeEnumerationFlags.Interlaced);

            ModeDescription desc2 = modes[modes.Count - 1];
            ModeDescription result2;
            output.GetClosestMatchingMode(device, desc2, out result2);
            //swapChain.SetFullScreenState(true, null);

            Result result;
            
            //
           

            swapChain.ResizeTarget(new ModeDescription(result2.Width, result2.Height, result2.RefreshRate, result2.Format));
            swapChain.SetFullScreenState(fullScreen, null);
            result = swapChain.ResizeBuffers(swapChainDesc.BufferCount, result2.Width, result2.Height,
                result2.Format, SwapChainFlags.AllowModeSwitch);
            backBuffer = Resource.FromSwapChain<Texture2D>(swapChain, 0);

            renderView = new RenderTargetView(device, backBuffer);
            rv = new RenderTargetView(device, rtTex);
            //Console.WriteLine(device.Rasterizer.State.ToString());
            RasterizerStateDescription rDesc = new RasterizerStateDescription()
            {
                CullMode = CullMode.None,
                IsDepthClipEnabled = true,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsFrontCounterclockwise = true,
                //IsMultisampleEnabled = true,
            };
            RasterizerState rState = RasterizerState.FromDescription(device, rDesc);
            device.Rasterizer.State = rState;
            device.Rasterizer.SetViewports(new Viewport(0, 0, size.Width, size.Height, 0.0f, 1.0f));
            //stereoizedTexture = Make3D(sourceTexture);
            //srv.Dispose();
            //srv = new ShaderResourceView(device, stereoizedTexture);

            //effect.GetVariableByName("tDiffuse").AsResource().SetResource(srv);
            device.OutputMerger.SetTargets(renderView);
            BuildQuad();
            //OnDeviceResize(new ResizeEventArgs(previousSize, newSize, fullScreen));

            //OnDeviceResume(EventArgs.Empty);
            Console.WriteLine("here");
        }

        static private Buffer vertices, indices;
        static void BuildQuad()
        {
            ushort[] idx;
            TexturedVertex[] quad = CreateTexturedQuad(Vector3.Zero, size.Width, size.Height, out idx);

            // fill vertex and index buffers
            DataStream stream = new DataStream(4 * 24, true, true);
            stream.WriteRange(quad);
            stream.Position = 0;

            vertices = new SlimDX.Direct3D10.Buffer(device, stream, new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags =
                    ResourceOptionFlags.None,
                SizeInBytes = 4 * 24,
                Usage = ResourceUsage.Default
            });
            stream.Close();

            stream = new DataStream(6 * sizeof(ushort), true, true);
            stream.WriteRange(idx);
            stream.Position = 0;
            indices = new SlimDX.Direct3D10.Buffer(device, stream, new BufferDescription()
            {
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = 6 * sizeof(ushort),
                Usage = ResourceUsage.Default
            });
        }

        
    }
}