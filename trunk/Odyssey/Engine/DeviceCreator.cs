using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Settings;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey
{
    public static class DeviceCreator
    {
        public static ShaderQuality GetHighestSupportedShaderModel(Capabilities caps)
        {
            Version requiredVersion = new Version(2, 0);

            if (caps.VertexShaderVersion < requiredVersion ||
                caps.PixelShaderVersion < requiredVersion)
            {
                MessageBox.Show(Properties.Resources.ERR_ShaderModelNotSupported,
                                Properties.Resources.ERR_Fatal,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Application.Exit();
            }
            else if (caps.VertexShaderVersion > requiredVersion &&
                caps.PixelShaderVersion > requiredVersion)
                return ShaderQuality.ShaderModel3;
            else
                return ShaderQuality.ShaderModel2;

            return ShaderQuality.ShaderModel1;
        }

        public static DeviceParameters[] CheckDevice(Direct3D direct3D, out Format[] supportedFormats)
        {
            List<DeviceParameters> supportedParams = new List<DeviceParameters>();

            List<Format> adapterFormats = new List<Format>();
            DeviceType[] deviceTypes = new DeviceType[]
                {
                    DeviceType.Hardware
                };

            Format[] backBufferFormats = new Format[]
                {
                    Format.A8R8G8B8, 
                    Format.X8R8G8B8,
                    //Format.A2R10G10B10,
                    Format.R5G6B5,
                    Format.R32F,
                    Format.G16R16F,
                    Format.R16F,
                    //Format.A1R5G5B5,
                    //Format.X1R5G5B5
                };

            Format[] depthStencilFormats = new Format[]
                {
                    Format.D32,
                    //Format.D24X4S4,
                    Format.D24S8,
                    //Format.D24X8,
                    Format.D15S1,
                    Format.D16,
                };

            MultisampleType[] multiSampleTypes = new MultisampleType[]
                                                     {
                                                         //MultisampleType.FourSamples,
                                                         //MultisampleType.ThreeSamples,
                                                         //MultisampleType.TwoSamples,
                                                         MultisampleType.None
                                                     };


            bool[] windowModes = new bool[] { true, false };

            // For each adapter
            foreach (AdapterInformation adapter in direct3D.Adapters)
           // for (int i = 0; i < direct3D.AdapterCount; i++)
            {


                foreach (Format format in backBufferFormats)
                {
                    DisplayModeCollection collection = adapter.GetDisplayModes(format);
                    foreach (DisplayMode displayMode in collection)
                        if (!adapterFormats.Contains(displayMode.Format))
                            adapterFormats.Add(displayMode.Format);
                }

                foreach (DeviceType deviceType in deviceTypes)
                {
                    Capabilities caps = direct3D.GetDeviceCaps(adapter.Adapter, deviceType);
                    CreateFlags createFlags;

                    if ((caps.DeviceCaps & DeviceCaps.HWTransformAndLight) == DeviceCaps.HWTransformAndLight)
                    {
                        createFlags = CreateFlags.HardwareVertexProcessing;
                    }
                    else
                    {
                        createFlags = CreateFlags.SoftwareVertexProcessing;
                    }

                    foreach (Format adapterFormat in adapterFormats)
                    {
                        foreach (Format backBufferFormat in backBufferFormats)
                        {

                            foreach (Format depthStencilFormat in depthStencilFormats)
                            {
                                foreach (MultisampleType multisampleType in multiSampleTypes)
                                {
                                    foreach (bool windowMode in windowModes)
                                    {
                                        int multisampleQuality;
                                        if (direct3D.CheckDeviceType(adapter.Adapter,
                                                                     deviceType,
                                                                     adapterFormat,
                                                                     backBufferFormat,
                                                                     windowMode)
                                            &&
                                            (direct3D.CheckDeviceFormat(adapter.Adapter,
                                                                        deviceType,
                                                                        adapterFormat,
                                                                        Usage.DepthStencil,
                                                                        ResourceType.Surface,
                                                                        depthStencilFormat))
                                            &&
                                            (direct3D.CheckDepthStencilMatch(adapter.Adapter,
                                                                             deviceType,
                                                                             adapterFormat,
                                                                             backBufferFormat,
                                                                             depthStencilFormat))
                                            &&
                                            (direct3D.CheckDeviceMultisampleType(adapter.Adapter,
                                                                                 deviceType,
                                                                                 adapterFormat,
                                                                                 windowMode,
                                                                                 multisampleType,
                                                                                 out multisampleQuality))
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
                                                                    multisampleType,
                                                                    createFlags, multisampleQuality));
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            supportedFormats = adapterFormats.ToArray();

            return supportedParams.ToArray();
        }
    }
}
