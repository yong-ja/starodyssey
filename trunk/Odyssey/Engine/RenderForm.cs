#region Using directives

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using SlimDX.Direct3D9;

#endregion

namespace AvengersUtd.Odyssey
{
    /// <summary>
    /// 
    /// </summary>
    public class StarOdysseyEngine : Form
    {
        bool fullScreen;
        Device device;
        PresentParameters presentParameters;

        protected readonly Size size = new Size(1024, 768);

        private static bool AppStillIdle
        {
            get
            {
                SafeNativeMethods.Message msg;
                return !SafeNativeMethods.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }

        public StarOdysseyEngine()
        {
            ClientSize = size;
            Application.Idle += ApplicationIdle;
            CreateDevice();
            InitDevice();

        }

        void ApplicationIdle(object sender, EventArgs e)
        {
            while (AppStillIdle)
            {
                Game.Loop();
            }
        }




      
        /// <summary>
        /// Initializes our PresentParamters class
        /// </summary>
        void CreateDevice()
        {
            //DeviceParameters supportedParams = CheckDevice();
            DeviceParameters supportedParams = new DeviceParameters(0, "Test", DeviceType.Hardware,
                Format.X8B8G8R8, Format.X8B8G8R8, true, Format.D32, MultisampleType.FourSamples);
            Capabilities caps = Direct3D.GetDeviceCaps(supportedParams.Adapter, supportedParams.DeviceType);
            CreateFlags vertexProcessing;

#if (!OdysseyUIDemo)
            Version requiredVersion = new Version(2, 0);

            if (caps.VertexShaderVersion < requiredVersion ||
                caps.PixelShaderVersion < requiredVersion)
                MessageBox.Show("Your hardware device does not support shaders version 2.0!",
                                "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif

            if ((caps.DeviceCaps & DeviceCaps.HWTransformAndLight) == 0)
            {
                vertexProcessing = CreateFlags.HardwareVertexProcessing;
            }
            else
            {
                vertexProcessing = CreateFlags.SoftwareVertexProcessing;
            }


            presentParameters = new PresentParameters();


            presentParameters.EnableAutoDepthStencil = true;
            presentParameters.AutoDepthStencilFormat = Format.D16;
            //presentParameters.AutoDepthStencilFormat = supportedParams.DepthStencilFormat;

            //1 Back buffer for double-buffering
            presentParameters.BackBufferCount = 1;

            //Set our Window as the Device Window
            presentParameters.DeviceWindowHandle = Handle;

            //Do not wait for VSync
            presentParameters.PresentationInterval = PresentInterval.Immediate;

            //Discard old frames for better performance
            presentParameters.SwapEffect = SwapEffect.Discard;

            //Set Windowed vs. Full-screen
            presentParameters.Windowed = !fullScreen;

            presentParameters.Multisample = supportedParams.MultiSampleType;
            //presentParameters.MultiSample = MultiSampleType.None;

            //We only need to set the Width/Height in full-screen mode
            //if (fullScreen)
            //{
            //    presentParameters.BackBufferHeight = size.Height;
            //    presentParameters.BackBufferWidth = size.Width;
            //}
            //else
            //{
            presentParameters.BackBufferHeight = size.Height;
            presentParameters.BackBufferWidth = size.Width;
            //    presentParameters.BackBufferFormat = supportedParams.BackBufferFormat;
            //}

            device = new Device(0, //Adapter
                                supportedParams.DeviceType, //Device Type
                                Handle, //Render Window
                                CreateFlags.HardwareVertexProcessing, //behaviour flags
                                presentParameters); //PresentParamters

            //Settings.DeviceInfo = supportedParams;
        }

        
        /// <summary>
        /// GoWindowed - Overrides function in base class to set up present parameters
        /// for the new mode and then call Reset to change modes.
        /// </summary>
        void GoWindowed()
        {
            CreateDevice();

            //We don't want to call this the first time through since the device
            //isn't set up yet.
            if (device != null)
            {
                device.Reset(presentParameters);
            }
        }

        /// <summary>
        /// GoFullscreen - Overrides function in base class to set up present parameters
        /// for the new mode and then call Reset to change modes.
        /// </summary>
        void GoFullscreen()
        {
            //base.GoFullscreen();

            CreateDevice();

            //We don't want to call this the first time through since the device
            //isn't set up yet.
            if (device != null)
            {
                device.Reset(presentParameters);
            }
        }



        protected void InitDevice()
        {
            device.SetRenderState(RenderState.ZEnable, true);
            device.SetRenderState(RenderState.Lighting, false);
            device.SetRenderState(RenderState.FillMode, FillMode.Solid);

            device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Point);
            device.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Linear);
            device.SetSamplerState(0, SamplerState.MipFilter, TextureFilter.Linear);


            Game.Device = device;
            OdysseyUI.Device = device;
            Global.FormOwner = this;

            //DebugManager.UpdateStaticStats(Settings.DeviceInfo.ToString());
            DebugManager.SetFrameTimeCounterDelegate(
                new FrameTimeCounter(Game.GetFrameTime));


            ////Turn off MDX automatic resize handling
            //device.DeviceResizing += new CancelEventHandler(CancelResize);

            ////Register our event handlers for Lost Devices
            //device.DeviceLost += new EventHandler(OnDeviceLost);
            //device.DeviceReset += new EventHandler(OnDeviceReset);
            //device.Disposing += new EventHandler(OnDispose);
        }
    }
}