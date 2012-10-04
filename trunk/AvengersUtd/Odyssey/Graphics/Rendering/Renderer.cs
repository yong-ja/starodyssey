using System;
using System.Drawing;
using AvengersUtd.Odyssey.Utils.Logging;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using System.Collections.Generic;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    /// <summary>
    /// Descrizione di riepilogo per Renderer.
    /// </summary>
    public abstract class Renderer : IDisposable
    {
        public static Color4 ClearColor { get; set; }
        private readonly List<SlimDX.Direct3D11.Buffer> buffers;

        private bool disposed;
        private readonly IDeviceContext deviceContext;

        //LightManager lightManager;
        public Hud Hud { get; set; }
        public ICamera Camera { get; protected set; }
        public SceneManager Scene { get; private set; }
        public bool IsInited { get; protected set; }

        //public LightManager LightManager
        //{
        //    get { return lightManager; }
        //}

        public IDeviceContext DeviceContext
        {
            get { return deviceContext; }
        }

        static Renderer()
        {
            //ClearColor = new Color4(1.0f, 0.0f, 0.0f, 0.0f);
            ClearColor = new Color4(1.0f, 0.25f, 0.25f, 0.25f);
        }

        protected Renderer(IDeviceContext deviceContext)
        {
            this.deviceContext = deviceContext;
            deviceContext.DeviceDisposing += OnDisposing;
            deviceContext.DeviceResize += OnDeviceResize;
            deviceContext.DeviceSuspend += OnDeviceSuspend;
            deviceContext.DeviceResume += OnDeviceResume;
            Scene = new SceneManager();
            Camera = new QuaternionCam();
            buffers = new List<SlimDX.Direct3D11.Buffer>();
            Camera.Reset();
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
            deviceContext.Present();            
        }

        public void RegisterBuffer(SlimDX.Direct3D11.Buffer buffer)
        {
            buffers.Add(buffer);
        }

        public void DebugBuffers()
        {
            LogEvent.Engine.Log("List of registered buffers:");
            foreach (SlimDX.Direct3D11.Buffer buffer in buffers)
            {
                //TODO
                //if (!buffer.Disposed)
                //LogEvent.BufferDisposed.Log((string)buffer.Tag, buffer.Disposed.ToString());
            }
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
            Hud.Dispose();
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
            Hud.IsEnabled = false;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    OnDisposing(this, EventArgs.Empty);
                    // TODO
                    //LogEvent.ObjectDisposed.Log(GetType().Name);
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