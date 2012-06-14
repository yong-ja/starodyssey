using System;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey
{
    public interface IDeviceContext
    {
        DepthStencilView DepthStencilView { get; }
        Device Device { get; }
        event EventHandler<EventArgs> DeviceDisposing;
        event EventHandler<AvengersUtd.Odyssey.Graphics.ResizeEventArgs> DeviceResize;
        event EventHandler<EventArgs> DeviceResume;
        event EventHandler<EventArgs> DeviceSuspend;
        Texture2D GetBackBuffer();
        void Dispose();
        DeviceContext Immediate { get; }
        RenderTargetView RenderTargetView { get; }
        void ResizeDevice(System.Drawing.Size newSize, bool fullScreen);
        AvengersUtd.Odyssey.Settings.DeviceSettings Settings { get; }

        void Present();
    }
}
