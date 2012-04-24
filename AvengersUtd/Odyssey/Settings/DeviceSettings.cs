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
        private static readonly Factory Factory;
        public int AdapterOrdinal { get; set; }

        public string AdapterName
        {
            get { return Factory.GetAdapter(AdapterOrdinal).Description.Description; }
        }

        public DeviceCreationFlags CreationFlags { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public Format Format { get; set; }
        public Size ScreenSize { get { return new Size(ScreenWidth, ScreenHeight);} }
        public SampleDescription SampleDescription { get; set; }
        public bool IsStereo { get; set; }

        static DeviceSettings()
        {
            Factory = new Factory();
        }

        public float AspectRatio
        {
            get { return ScreenWidth/ (float)ScreenHeight; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Adapter[{0}]: {1}\n", AdapterOrdinal, AdapterName);
            sb.AppendFormat("\tResolution: {0}x{1} Format: {2} M{3}Q{4}\n", ScreenWidth, ScreenHeight, Format, SampleDescription.Count, SampleDescription.Quality);
            return sb.ToString();
        }
        
    }
}
