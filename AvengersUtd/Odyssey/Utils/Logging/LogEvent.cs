using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Properties;

namespace AvengersUtd.Odyssey.Utils.Logging
{
    public class LogEvent : AbstractLogEvent
    {
        public static LogEvent Engine = new LogEvent(Game.EngineTag, "{0}");
        public static LogEvent UserInterface = new LogEvent(Game.UITag, "{0}");
        public static LogEvent Network = new LogEvent(Game.NetworkTag, "{0}");

        public LogEvent(string source) : this(source, Resources.ERR_NoInformation)
        {}

        public LogEvent(string source, string format) : base(source, EventCode.LogMessage, TraceEventType.Information, format)
        {}

    }
}
