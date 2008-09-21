#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.Odyssey.Settings;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using SlimDX.Direct3D9;

#endregion

namespace AvengersUtd.Odyssey
{
    /// <summary>
    /// 
    /// </summary>
    public class RenderForm : Form
    {
        bool fullScreen;
        Device device;
        PresentParameters presentParameters;
        Direct3D direct3D;

        protected readonly Size size = new Size(1024, 768);

        static bool AppStillIdle
        {
            get
            {
                SafeNativeMethods.Message msg;
                return !SafeNativeMethods.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }

        public RenderForm()
        {
            ClientSize = size;
            Application.Idle += ApplicationIdle;

            direct3D = new Direct3D();
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
            Format[] supportedTextureFormats;
            DeviceParameters supportedParams = DeviceCreator.CheckDevice(direct3D, out supportedTextureFormats)[0];

            presentParameters = new PresentParameters
                                    {
                                        AutoDepthStencilFormat = supportedParams.DepthStencilFormat,
                                        EnableAutoDepthStencil = true,
                                        BackBufferFormat = supportedParams.BackBufferFormat,
                                        BackBufferCount = 1,
                                        DeviceWindowHandle = Handle,
                                        PresentationInterval = PresentInterval.Immediate,
                                        SwapEffect = SwapEffect.Discard,
                                        Windowed = (!fullScreen),
                                        Multisample = supportedParams.MultisampleType,
                                        MultisampleQuality = supportedParams.MultisampleQuality-1,
                                        BackBufferHeight = size.Height,
                                        BackBufferWidth = size.Width
                                    };


            

            //We only need to set the Width/Height in full-screen mode
            //if (fullScreen)
            //{
            //    presentParameters.BackBufferHeight = size.Height;
            //    presentParameters.BackBufferWidth = size.Width;
            //}
            //else
            //{
            //    presentParameters.BackBufferFormat = supportedParams.BackBufferFormat;
            //}

            device = new Device(direct3D,
                                0, //Adapter
                                supportedParams.DeviceType, //Device Type
                                Handle, //Render Window
                                supportedParams.CreateFlags, //behaviour flags
                                presentParameters); //PresentParamters

            Capabilities caps = direct3D.GetDeviceCaps(supportedParams.Adapter, supportedParams.DeviceType);

            ShaderQuality shaderQuality = DeviceCreator.GetHighestSupportedShaderModel(caps);
            VideoSettings videoSettings = new VideoSettings(supportedParams,
                                                            shaderQuality,
                                                            size.Width, size.Height, supportedTextureFormats);

            EngineSettings.Video = videoSettings;
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
            DebugManager.SetFrameTimeCounterDelegate(Game.GetFrameTime);


            ////Turn off MDX automatic resize handling
            //device.DeviceResizing += new CancelEventHandler(CancelResize);

            ////Register our event handlers for Lost Devices
            //device.DeviceLost += new EventHandler(OnDeviceLost);
            //device.DeviceReset += new EventHandler(OnDeviceReset);
            //device.Disposing += new EventHandler(OnDispose);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            TextureManager.Dispose();
            EntityManager.Dispose();
            EffectManager.Dispose();
        }
    }
}