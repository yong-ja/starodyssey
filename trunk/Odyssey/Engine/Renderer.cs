using System;
using SlimDX.Direct3D9;


namespace AvengersUtd.Odyssey.Engine
{
    /// <summary>
    /// Descrizione di riepilogo per Renderer.
    /// </summary>
    public abstract class Renderer : IDisposable
    {
        protected bool disposed;
        protected QuaternionCam qCam;
        protected Device device;

        public QuaternionCam Camera
        {
            get { return qCam; }
        }

        public Device Device
        {
            get { return device; }
        }

        public Renderer()
        {
            device = Game.Device;
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