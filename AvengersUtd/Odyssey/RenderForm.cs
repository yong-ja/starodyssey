using System;
using System.Drawing;
using AvengersUtd.Odyssey.Geometry;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.Windows;
using Device = SlimDX.Direct3D11.Device;

namespace AvengersUtd.Odyssey
{
    public class RenderForm11
    {
        private static Device device;
        static SwapChain swapChain;

        bool fullScreen;
        public static RenderTargetView renderTarget;
        
        Texture2D backBuffer;
        RenderForm form;

        internal RenderTargetView RenderTarget
        {
            get { return renderTarget; }
            set { renderTarget = value; }
        }

        public static Device Device
        {
            get { return device; }
        }

        public static SwapChain SwapChain
        {
            get { return swapChain; }
        }

        public RenderForm Handle
        {
            get { return form; }
        }

        public RenderForm11()
        {
            CreateDevice();
        }

        

        /// <summary>
        /// Initializes our PresentParamters class
        /// </summary>
        void CreateDevice()
        {
            form = new RenderForm()
                                  {
                                      ClientSize = new Size(1024, 768),
                                      Text = "Odyssey 11 demo"
                                  };
            SwapChainDescription desc = new SwapChainDescription()
                                            {
                                                BufferCount = 1,
                                                ModeDescription = new ModeDescription(form.ClientSize.Width, form.ClientSize.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                                                IsWindowed = true,
                                                OutputHandle = form.Handle,
                                                SampleDescription = new SampleDescription(1, 0),
                                                SwapEffect = SwapEffect.Discard,
                                                Usage = Usage.RenderTargetOutput,
                                            };

            FeatureLevel fl = Device.GetSupportedFeatureLevel();
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.Debug, desc, out device, out swapChain);
           
            Factory factory = swapChain.GetParent<Factory>();
            factory.SetWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll);
            backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            renderTarget = new RenderTargetView(device, backBuffer);

            device.ImmediateContext.OutputMerger.SetTargets(renderTarget);
            device.ImmediateContext.Rasterizer.SetViewports(new Viewport(0, 0, form.ClientSize.Width, form.ClientSize.Height, 0.0f, 1.0f));
        }

        public void Dispose()
        {
            renderTarget.Dispose();
            backBuffer.Dispose();
            device.Dispose();
            swapChain.Dispose();
        }

        private Polygon triangle;
    }
}