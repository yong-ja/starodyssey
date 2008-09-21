using System;
using SlimDX.Direct3D9;


namespace AvengersUtd.Odyssey
{
    /// <summary>
    /// Descrizione di riepilogo per Renderer.
    /// </summary>
    public abstract class Renderer : IDisposable
    {
        protected bool disposed;
        protected Device device;

        protected Device Device
        {
            get { return device; }
        }

        public Renderer()
        {
            device = Game.Device;
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