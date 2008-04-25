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




        /*
        DeviceParameters CheckDevice()
        {
            ArrayList supportedParams = new ArrayList();
            //DeviceParameters supportedParams = new DeviceParameters();

            DeviceType[] deviceTypes = new DeviceType[]
                {
                    DeviceType.Hardware
                };

            Format[] backBufferFormats = new Format[]
                {
                    //Format.A8R8G8B8, 
                    Format.X8R8G8B8,
                    //Format.A2R10G10B10,
                    Format.R5G6B5,
                    //Format.A1R5G5B5,
                    //Format.X1R5G5B5
                };

            Format[] depthStencilFormats = new Format[]
                {
                    Format.D32,
                    Format.D24X4S4,
                    Format.D24S8,
                    Format.D24X8,
                    Format.D15S1,
                    Format.D16,
                };

            MultiSampleType[] multiSampleTypes = new MultiSampleType[]
                {
                    MultiSampleType.FourSamples,
                    MultiSampleType.TwoSamples,
                    MultiSampleType.None
                };

            bool[] windowModes = new bool[] {true, false};

            // For each adapter
            foreach (AdapterInformation adapter in Direct3D.Adapters)
            {
                ArrayList adapterFormats = new ArrayList();

                // Build the list of adapter formats
                foreach (DisplayMode dispMode in adapter.SupportedDisplayModes)
                {
                    if (!adapterFormats.Contains(dispMode.Format))
                    {
                        adapterFormats.Add(dispMode.Format);
                    }
                }

                foreach (DeviceType deviceType in deviceTypes)
                {
                    foreach (Format adapterFormat in adapterFormats)
                    {
                        foreach (Format backBufferFormat in backBufferFormats)
                        {
                            foreach (bool windowMode in windowModes)
                            {
                                foreach (Format depthStencilFormat in depthStencilFormats)
                                {
                                    foreach (MultiSampleType multiSampleType in multiSampleTypes)
                                    {
                                        if (Direct3D.CheckDeviceType(adapter.Adapter,
                                                                    deviceType,
                                                                    adapterFormat,
                                                                    backBufferFormat,
                                                                    windowMode)
                                            &&
                                            (Direct3D.CheckDeviceFormat(adapter.Adapter, deviceType, adapterFormat,
                                                                       Usage.DepthStencil, ResourceType.Surface,
                                                                       depthStencilFormat))
                                            &&
                                            (Direct3D.CheckDepthStencilMatch(adapter.Adapter, deviceType,
                                                                            adapterFormat, backBufferFormat,
                                                                            depthStencilFormat))
                                            &&
                                            (Direct3D.CheckDeviceMultiSampleType(adapter.Adapter, deviceType,
                                                                                adapterFormat,
                                                                                windowMode, multiSampleType))
                                            )

                                        {
                                            // This depth stencil format is compatible
                                            supportedParams.Add(new DeviceParameters(
                                                                    adapter.Adapter,
                                                                    adapter.Details.Description,
                                                                    deviceType,
                                                                    adapterFormat,
                                                                    backBufferFormat,
                                                                    windowMode,
                                                                    depthStencilFormat,
                                                                    multiSampleType)
                                                );
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return (DeviceParameters) supportedParams[0];
        }
         * */

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

        //void Heartbeat()
        //{
        //    //ResultCode result = device.CheckCooperativeLevelResult();
        //    CooperativeLevel result;

        //    if ((result=device.CheckCooperativeLevel()) == CooperativeLevel.Ok)
        //    {
        //        //Okay to render

        //        try
        //        {
        //            Game.Loop();
        //        }
        //        catch (DeviceLostException)
        //        {
        //            result = CooperativeLevel.DeviceLost;
        //        }
        //        catch (DeviceNotResetException)
        //        {
        //            result = CooperativeLevel.DeviceNotReset;
        //        }
        //    }
        //    if (result == CooperativeLevel.DeviceLost)
        //    {
        //        Thread.Sleep(500); //Can't Reset yet, wait for a bit
        //    }
        //    else if (result == CooperativeLevel.DeviceNotReset)
        //    {
        //        device.Reset(presentParameters);
        //    }
        //}

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

        //public void Run()
        //{
        //    while (Created)
        //    {
        //        if (Focused)
        //        {
        //            Heartbeat();
        //        }
        //        else
        //        {
        //            Thread.Sleep(100);

        //            // Allow the application to handle Windows messages
        //            Application.DoEvents();

        //            // Do the next loop
        //            continue;
        //        }
        //    }
        //}


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