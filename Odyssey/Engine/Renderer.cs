using System;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Engine;


namespace AvengersUtd.Odyssey
{
    /// <summary>
    /// Descrizione di riepilogo per Renderer.
    /// </summary>
    public abstract class Renderer : IDisposable
    {
        protected bool disposed;
        protected QuaternionCam qCam;
        protected Device device;
        LightManager lightManager;

        public QuaternionCam Camera
        {
            get { return qCam; }
        }

        public LightManager LightManager
        {
            get { return lightManager; }
        }

        public Device Device
        {
            get { return device; }
        }

        public Renderer()
        {
            device = Game.Device;
            lightManager = new LightManager();
            qCam = new QuaternionCam();
        }

        public void Reset()
        {
        }

        public abstract void Init();
        public abstract void Render();
        public abstract void ProcessInput();

        public abstract void Dispose();
    }
}