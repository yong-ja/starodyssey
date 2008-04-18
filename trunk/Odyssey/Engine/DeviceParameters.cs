using System;
using System.Collections.Generic;
using System.Text;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey
{
    public struct DeviceParameters
    {
        int adapter;
        DeviceType deviceType;
        Format adapterFormat;
        Format backBufferFormat;
        Format depthStencilFormat;
        bool windowed;
        string adapterName;
        MultisampleType multiSampleType;
        //private CreateFlags vertexProcessing;

        public bool Windowed
        {
            get { return windowed; }
        }

        public Format BackBufferFormat
        {
            get { return backBufferFormat; }
        }

        public Format AdapterFormat
        {
            get { return adapterFormat; }
        }

        public string AdapterName
        {
            get { return adapterName; }
        }

        public DeviceType DeviceType
        {
            get { return deviceType; }
        }

        public int Adapter
        {
            get { return adapter; }
        }

        public Format DepthStencilFormat
        {
            get { return depthStencilFormat; }
        }


        public MultisampleType MultiSampleType
        {
            get { return multiSampleType; }
        }

        public DeviceParameters(int adapter,
                                string adapterName,
                                DeviceType deviceType,
                                Format adapterFormat,
                                Format backBufferFormat,
                                bool windowed,
                                Format depthStencilFormat,
                                MultisampleType multiSampleType)
        //CreateFlags vertexProcessing)
        {
            this.adapterName = adapterName;
            this.adapter = adapter;
            this.deviceType = deviceType;
            this.adapterFormat = adapterFormat;
            this.backBufferFormat = backBufferFormat;
            this.windowed = windowed;
            this.depthStencilFormat = depthStencilFormat;
            this.multiSampleType = multiSampleType;
            //this.vertexProcessing = vertexProcessing;
        }

        public override string ToString()
        {
            return adapterName + "Format." +
                   backBufferFormat + " Vertex Processing: " + deviceType; 
                   //+
                   //" at " + Settings.Width + 'x' + Settings.Height;
        }
    }
}
