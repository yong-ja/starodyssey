using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey.Settings
{
    public class DeviceSettings
    {
        public int AdapterOrdinal { get; set; }
        public DeviceCreationFlags CreationFlags { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public Size ScreenSize { get { return new Size(ScreenWidth, ScreenHeight);} }
        public SampleDescription SampleDescription { get; set; }

        public float AspectRatio
        {
            get { return ScreenWidth/ (float)ScreenHeight; }
        }
        
    }
}
