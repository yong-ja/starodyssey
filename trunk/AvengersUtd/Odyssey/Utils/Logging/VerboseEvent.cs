using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Properties;
using System.Diagnostics;

namespace AvengersUtd.Odyssey.Utils.Logging
{
    public class VerboseEvent : AbstractLogEvent
    {
        public static VerboseEvent InstanceParameterCreation = new VerboseEvent(Game.GraphicsTag, Resources.INFO_Graphics_InstanceParameterCreation);
        public static VerboseEvent InstanceParameterSetting = new VerboseEvent(Game.RenderingTag, Resources.INFO_Rendering_ApplyInstanceParameter);
        public static VerboseEvent DynamicParameterSetting = new VerboseEvent(Game.RenderingTag, Resources.INFO_Rendering_ApplyInstanceParameter);

        public VerboseEvent(string source)
            : this(source, Resources.ERR_NoInformation)
        {}

        public VerboseEvent(string source, string format) : base(source, EventCode.VerboseMessage, TraceEventType.Verbose, format)
        {}
    }
}
