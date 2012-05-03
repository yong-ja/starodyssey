#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Utils.Logging;
using AvengersUtd.Odyssey.Settings;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;
using Resources = AvengersUtd.Odyssey.Properties.Resources;

#endregion

namespace AvengersUtd.Odyssey
{
    public class DeviceContext11 : IDisposable
    {
        public const Format DefaultIndexFormat = Format.R16_UInt;

        private readonly Device device;
        private readonly EventHandlerList eventHandlerList;
        private readonly Factory factory;
        private readonly DeviceContext immediate;
        private readonly SwapChain swapChain;
        private bool disposed;

        #region Events

        private static readonly object EventDeviceResize;
        private static readonly object EventDeviceDisposing;
        private static readonly object EventDeviceSuspend;
        private static readonly object EventDeviceResume;

        public event EventHandler<ResizeEventArgs> DeviceResize
        {
            add { eventHandlerList.AddHandler(EventDeviceResize, value); }
            remove { eventHandlerList.RemoveHandler(EventDeviceResize, value); }
        }

        public event EventHandler<EventArgs> DeviceDisposing
        {
            add { eventHandlerList.AddHandler(EventDeviceDisposing, value); }
            remove { eventHandlerList.RemoveHandler(EventDeviceDisposing, value); }
        }

        public event EventHandler<EventArgs> DeviceSuspend
        {
            add { eventHandlerList.AddHandler(EventDeviceSuspend, value); }
            remove { eventHandlerList.RemoveHandler(EventDeviceSuspend, value); }
        }

        public event EventHandler<EventArgs> DeviceResume
        {
            add { eventHandlerList.AddHandler(EventDeviceResume, value); }
            remove { eventHandlerList.RemoveHandler(EventDeviceResume, value); }
        }

        protected virtual void OnDeviceResize(ResizeEventArgs e)
        {
            EventHandler<ResizeEventArgs> handler = (EventHandler<ResizeEventArgs>) eventHandlerList[EventDeviceResize];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnDeviceDisposing(EventArgs e)
        {
            EventHandler<EventArgs> handler = (EventHandler<EventArgs>) eventHandlerList[EventDeviceDisposing];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnDeviceSuspend(EventArgs e)
        {
            EventHandler<EventArgs> handler = (EventHandler<EventArgs>) eventHandlerList[EventDeviceSuspend];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnDeviceResume(EventArgs e)
        {
            EventHandler<EventArgs> handler = (EventHandler<EventArgs>) eventHandlerList[EventDeviceResume];
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Properties

        public DepthStencilView DepthStencilView { get; private set; }

        public RenderTargetView RenderTargetView { get; private set; }

        public DeviceSettings Settings { get; private set; }

        public Device Device
        {
            get { return device; }
        }

        public SwapChain SwapChain
        {
            get { return swapChain; }
        }

        public Factory Factory
        {
            get { return factory; }
        }

        public DeviceContext Immediate
        {
            get { return immediate; }
        }

        #endregion

        static DeviceContext11()
        {
            EventDeviceResize = new object();
            EventDeviceDisposing = new object();
            EventDeviceSuspend = new object();
            EventDeviceResume = new object();
        }

        public DeviceContext11(IntPtr handle, DeviceSettings settings)
        {
            Contract.Requires(handle != IntPtr.Zero);
            Contract.Requires(settings != null);

            Settings = settings;
            LogEvent.Engine.Log(settings.ToString());


            eventHandlerList = new EventHandlerList();
            SwapChainDescription swapChainDesc = new SwapChainDescription
                                                     {
                                                         BufferCount = 1,
                                                         ModeDescription =
                                                             new ModeDescription(Settings.ScreenWidth, Settings.ScreenHeight,
                                                                                 new Rational(120, 1), Settings.Format),
                                                         IsWindowed = true,
                                                         OutputHandle = handle,
                                                         SampleDescription = Settings.SampleDescription,
                                                         SwapEffect = SwapEffect.Discard,
                                                         Usage = Usage.RenderTargetOutput,
                                                     };

            LogEvent.Engine.Log(Resources.INFO_OE_DeviceCreating);
            Device.CreateWithSwapChain(DriverType.Hardware, Settings.CreationFlags, swapChainDesc, out device, out swapChain);

            factory = swapChain.GetParent<Factory>();
            factory.SetWindowAssociation(handle, WindowAssociationFlags.IgnoreAltEnter | WindowAssociationFlags.IgnoreAll);

            immediate = device.ImmediateContext;

            CreateTargets();
            LogEvent.Engine.Log(Resources.INFO_OE_DeviceCreated);
        }

        #region IDisposable Members

        /// <summary>
        ///   Disposes all managed and unmanaged resources of this object.
        /// </summary>
        /// <remarks>
        ///   Be sure to call this method when you don't need this control anymore.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        private void CreateTargets()
        {
            using (Texture2D backBuffer = GetBackBuffer())
            {
                RenderTargetView = new RenderTargetView(device, backBuffer);
            }

            Texture2DDescription depthBufferDesc = new Texture2DDescription
                                                       {
                                                           ArraySize = 1,
                                                           BindFlags = BindFlags.DepthStencil,
                                                           CpuAccessFlags = CpuAccessFlags.None,
                                                           Format = Format.D32_Float,
                                                           Width = Settings.ScreenWidth,
                                                           Height = Settings.ScreenHeight,
                                                           MipLevels = 1,
                                                           OptionFlags = ResourceOptionFlags.None,
                                                           SampleDescription = Settings.SampleDescription,
                                                           Usage = ResourceUsage.Default
                                                       };

            using (Texture2D depthBuffer = new Texture2D(device, depthBufferDesc))
            {
                DepthStencilView = new DepthStencilView(device, depthBuffer);
            }
        }

        public void ResizeDevice(Size newSize, bool fullScreen)
        {
            OnDeviceSuspend(EventArgs.Empty);

            Size previousSize = new Size(Settings.ScreenWidth, Settings.ScreenHeight);
            Settings.ScreenWidth = newSize.Width;
            Settings.ScreenHeight = newSize.Height;
            RenderTargetView.Dispose();
            DepthStencilView.Dispose();
            SwapChainDescription swapChainDesc = swapChain.Description;
            Result result = swapChain.ResizeBuffers(swapChainDesc.BufferCount, newSize.Width, newSize.Height,
                                                    swapChainDesc.ModeDescription.Format, swapChainDesc.Flags);
            //result = swapChain.ResizeTarget(swapChain.Description.ModeDescription);
            CreateTargets();
            OnDeviceResize(new ResizeEventArgs(previousSize, newSize, fullScreen));

            OnDeviceResume(EventArgs.Empty);
        }

        public Texture2D GetBackBuffer()
        {
            return Resource.FromSwapChain<Texture2D>(swapChain, 0);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    OnDeviceDisposing(EventArgs.Empty);

                    RenderTargetView.Dispose();
                    DepthStencilView.Dispose();
                    swapChain.Dispose();
                    factory.Dispose();

                    device.ImmediateContext.ClearState();
                    device.ImmediateContext.Flush();

                    device.Dispose();
                    Game.CurrentRenderer.DebugBuffers();
                }
            }
            disposed = true;
        }

        ~DeviceContext11()
        {
            Dispose(false);
        }
    }
}