using System;
using System.Collections.Generic;
using System.Text;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Settings
{
    public enum ShaderQuality
    {
        ShaderModel1 = 1,
        ShaderModel2 = 2,
        ShaderModel3 = 3
    }

    public enum ShadowQuality
    {
        Low,
        Medium,
        High
    }

    public class VideoSettings
    {

        static Format[] singleChannelFormats = {
                                       Format.R32F, Format.A8R8G8B8, Format.X8R8G8B8
                                   };

        DeviceParameters deviceInfo;
        int screenWidth;
        int screenHeight;
        ShaderQuality shaderQuality;
        ShadowQuality shadowQuality;
        Format bestSingleChannelTextureFormat;
        Format[] supportedTextureFormats;
       
        #region Properties

        public DeviceParameters DeviceInfo
        {
            get { return deviceInfo; }
        }

        public Format BestSingleChannelTextureFormat
        {
            get { return bestSingleChannelTextureFormat; }
        }

        public int ScreenWidth
        {
            get { return screenWidth; }
        }

        public int ScreenHeight
        {
            get { return screenHeight; }
        }

        public ShaderQuality ShaderQuality
        {
            get { return shaderQuality; }
        }

        public string VertexShaderTag
        {
            get
            {
                return string.Format("vs_{0}_0", (int)shaderQuality);
            }
        }

        public string PixelShaderTag
        {
            get
            {
                return string.Format("ps_{0}_0", (int)shaderQuality);
            }
        } 
        #endregion

        public string ShadowAlgorithmTag
        {
            get
            {
                switch (shadowQuality)
                {
                    case ShadowQuality.Low:
                        return "SSM";
                    case ShadowQuality.Medium:
                        return "VSM";
                    case ShadowQuality.High:
                        return "CazzoSM";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public VideoSettings(DeviceParameters deviceInfo, ShaderQuality shaderQuality, int screenWidth, int screenHeight, Format[] supportedTextureFormats)
        {
            this.deviceInfo = deviceInfo;
            this.screenWidth = screenWidth;
            this.shaderQuality = shaderQuality;
            this.screenHeight = screenHeight;
            this.supportedTextureFormats = supportedTextureFormats;

            shadowQuality = ShadowQuality.Low;

            bestSingleChannelTextureFormat = FindBestTextureFormat(singleChannelFormats, supportedTextureFormats);


        }

        public static bool SupportsFormat(Format format, Format[] supportedFormats)
        {
            if (Array.Exists(supportedFormats, f => f == format))
                return true;
            else
                return false;
            
        }

        public static Format FindBestTextureFormat(Format[] preferredformats, Format[] supportedFormats)
        {
            for (int i = 0; i < preferredformats.Length; i++)
            {
                Format preferredFormat = preferredformats[i];
                if (SupportsFormat(preferredFormat, supportedFormats))
                    return preferredFormat;
            }

            // At least X8R8G8B8 should be supported
            throw new InvalidOperationException(Properties.Resources.ERR_Fatal);

        }
    }
}
