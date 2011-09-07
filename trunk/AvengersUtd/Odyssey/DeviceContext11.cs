using System;
using System.ComponentModel;
using System.Drawing;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Settings;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;

namespace AvengersUtd.Odyssey
{
    public class DeviceContext11 : IDisposable
    {
        public const Format DefaultIndexFormat = Format.R16_UInt;

        private readonly EventHandlerList eventHandlerList;
        private bool disposed;
        private readonly Device device;
        private readonly DeviceContext immediate;  
        private readonly SwapChain swapChain;
        private readonly Factory factory;
        private RenderTargetView renderTargetView;
        private DepthStencilView depthStencilView;

        #region Events
        private static readonly object EventDeviceResize;
        private static readonly object EventDeviceDisposing;
        private static readonly object EventDeviceSuspend;
        private static readonly object EventDeviceResume;

        public event EventHandler<ResizeEventArgs> DeviceResize
        {
            add { eventHandlerList.AddHandler(EventDeviceResize, value);}
            remove { eventHandlerList.RemoveHandler(EventDeviceResize, value);}
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
            EventHandler<ResizeEventArgs> handler = (EventHandler<ResizeEventArgs>)eventHandlerList[EventDeviceResize];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnDeviceDisposing(EventArgs e)
        {
            EventHandler<EventArgs> handler = (EventHandler<EventArgs>)eventHandlerList[EventDeviceDisposing];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnDeviceSuspend(EventArgs e)
        {
            EventHandler<EventArgs> handler = (EventHandler<EventArgs>)eventHandlerList[EventDeviceSuspend];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnDeviceResume(EventArgs e)
        {
            EventHandler<EventArgs> handler = (EventHandler<EventArgs>)eventHandlerList[EventDeviceResume];
            if (handler != null)
                handler(this, e);
        } 
        #endregion

        #region Properties

        public DepthStencilView DepthStencilView
        {
            get { return depthStencilView; }
        }

        public RenderTargetView RenderTargetView
        {
            get { return renderTargetView; }
        }

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
            if (handle == IntPtr.Zero)
            {
                throw Error.ArgumentInvalid("handle", GetType(),"DeviceContext11_ctor()","Value must be a valid window handle.");
            }
            if (settings == null)
            {
                throw Error.ArgumentNull("settings", GetType(), "DeviceContext11_ctor()");
            }
            eventHandlerList = new EventHandlerList();
            Settings = settings;

            SwapChainDescription swapChainDesc = new SwapChainDescription
                                                     {
                BufferCount = 1,
                ModeDescription = new ModeDescription(Settings.ScreenWidth, Settings.ScreenHeight, new Rational(120, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = handle,
                SampleDescription = Settings.SampleDescription,
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput,
            };

            Device.CreateWithSwapChain(DriverType.Hardware, Settings.CreationFlags, swapChainDesc, out device, out swapChain);

            factory = swapChain.GetParent<Factory>();
            factory.SetWindowAssociation(handle, WindowAssociationFlags.IgnoreAltEnter | WindowAssociationFlags.IgnoreAll);

            immediate = device.ImmediateContext;

            CreateTargets();
        }

        private void CreateTargets()
        {
            using (Texture2D backBuffer = Resource.FromSwapChain<Texture2D>(swapChain, 0))
            {
                renderTargetView = new RenderTargetView(device, backBuffer);
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
                depthStencilView = new DepthStencilView(device, depthBuffer);
            }
        }

        public void ResizeDevice(Size newSize, bool fullScreen)
        {
            OnDeviceSuspend(EventArgs.Empty);

            Size previousSize = new Size(Settings.ScreenWidth, Settings.ScreenHeight);
            Settings.ScreenWidth = newSize.Width;
            Settings.ScreenHeight = newSize.Height;
            renderTargetView.Dispose();
            depthStencilView.Dispose();
            SwapChainDescription swapChainDesc = swapChain.Description;
            Result result = swapChain.ResizeBuffers(swapChainDesc.BufferCount, newSize.Width, newSize.Height,
                swapChainDesc.ModeDescription.Format, swapChainDesc.Flags);
            //result = swapChain.ResizeTarget(swapChain.Description.ModeDescription);
            CreateTargets();
            OnDeviceResize(new ResizeEventArgs(previousSize, newSize, fullScreen));

            OnDeviceResume(EventArgs.Empty);
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes all managed and unmanaged resources of this object.
        /// </summary>
        /// <remarks>Be sure to call this method when you don't need this control anymore.</remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose (bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    OnDeviceDisposing(EventArgs.Empty);

                    renderTargetView.Dispose();
                    depthStencilView.Dispose();
                    swapChain.Dispose();
                    factory.Dispose();
                    
                    device.ImmediateContext.ClearState();
                    device.ImmediateContext.Flush();

                    device.Dispose();
                    BaseMesh<TexturedMeshVertex>.DebugBuffers();
               }

            }
            disposed = true;
        }

        ~DeviceContext11()
        {
            Dispose(false);
        }

        #endregion

        
    }
}
