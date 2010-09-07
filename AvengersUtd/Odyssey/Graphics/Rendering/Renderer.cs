using System;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    /// <summary>
    /// Descrizione di riepilogo per Renderer.
    /// </summary>
    public abstract class Renderer : IDisposable
    {
        public static Color4 ClearColor { get; set; }

        private bool disposed;
        private readonly DeviceContext11 deviceContext;

        //LightManager lightManager;
        public Hud Hud { get; set; }
        public QuaternionCam Camera { get; private set; }
        public SceneManager Scene { get; private set; }

        //public LightManager LightManager
        //{
        //    get { return lightManager; }
        //}

        public DeviceContext11 DeviceContext
        {
            get { return deviceContext; }
        }

        static Renderer()
        {
            ClearColor = new Color4(1.0f, 0.0f, 0.0f, 0.0f);
        }

        protected Renderer(DeviceContext11 deviceContext11)
        {
            deviceContext = deviceContext11;
            deviceContext.DeviceDisposing += OnDisposing;
            deviceContext.DeviceResize += OnDeviceResize;
            deviceContext.DeviceSuspend += OnDeviceSuspend;
            deviceContext.DeviceResume += OnDeviceResume;
            Scene = new SceneManager();
            Camera = new QuaternionCam();
        }

        public void Reset()
        {
        }

        public abstract void Init();
        public abstract void Render();
        public abstract void ProcessInput();

        public void Begin()
        {
            deviceContext.Immediate.OutputMerger.SetTargets(deviceContext.DepthStencilView, deviceContext.RenderTargetView);
            deviceContext.Immediate.Rasterizer.SetViewports(new Viewport(0, 0, deviceContext.Settings.ScreenWidth, deviceContext.Settings.ScreenHeight, 0.0f, 1.0f));
            deviceContext.Immediate.ClearRenderTargetView(deviceContext.RenderTargetView, ClearColor);
            deviceContext.Immediate.ClearDepthStencilView(deviceContext.DepthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
        }

        public void Present()
        {
            deviceContext.SwapChain.Present(0, PresentFlags.None);
        }

        protected virtual void OnDeviceResize(object sender,ResizeEventArgs e)
        {
            Camera.Reset();
            Scene.Tree.Reset();
            Hud.BeginDesign();
            Hud.Size = e.NewSize;
            Hud.EndDesign();
        }

        protected virtual void OnDisposing(object sender, EventArgs e)
        {
            Scene.Dispose();
        }

        protected virtual void OnDeviceSuspend(object sender, EventArgs e)
        {
            //Scene.SuspendUpdates();
            //Hud.IsEnabled = false;
        }

        protected virtual void OnDeviceResume(object sender, EventArgs e)
        {
            Scene.ResumeUpdates();
        }

        #region IDisposable members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose managed components
                    OnDisposing(this, EventArgs.Empty);
                }
            }
            disposed = true;
        }

        ~Renderer()
        {
            Dispose(false);
        } 
        #endregion
    }
}