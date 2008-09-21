using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Settings;

namespace AvengersUtd.Odyssey.Settings
{
    public static class EngineSettings
    {
        static VideoSettings video;

        public static VideoSettings Video
        {
            get { return video; }
            set { video = value; }
        }
    }
}