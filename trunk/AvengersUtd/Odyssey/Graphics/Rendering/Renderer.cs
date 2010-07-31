using System;
using AvengersUtd.Odyssey.Graphics.Rendering;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Device = SlimDX.Direct3D11.Device;

namespace AvengersUtd.Odyssey.Rendering
{
    /// <summary>
    /// Descrizione di riepilogo per Renderer.
    /// </summary>
    public abstract class Renderer : IDisposable
    {
        private bool disposed;
        private Device device;
        private SwapChain swapChain;
        //LightManager lightManager;

        public QuaternionCam Camera { get; private set; }
        public SceneOrganizer Scene { get; private set; }

        //public LightManager LightManager
        //{
        //    get { return lightManager; }
        //}

        public Device Device
        {
            get { return device; }
        }

        public Renderer()
        {
            device = RenderForm11.Device;
            swapChain = RenderForm11.SwapChain;
            //lightManager = new LightManager();
            Scene = new SceneOrganizer();
            Camera = new QuaternionCam();
        }

        public void Reset()
        {
        }

        public abstract void Init();
        public abstract void Render();
        public abstract void ProcessInput();

        public abstract void Dispose();

        public void Present()
        {
            swapChain.Present(0, PresentFlags.None);
        }
    }
}