using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AvengersUtd.Odyssey.Properties;

namespace AvengersUtd.Odyssey.Log
{
    public class LogEvent : AbstractLogEvent
    {

        public static LogEvent EngineEvent = new LogEvent(Game.EngineTag, EventCode.LogMessage);

        public LogEvent(string source, EventCode code) : this(source, code, Resources.ERR_NoInformation)
        {}

        public LogEvent(string source, EventCode code, string format) : base(source, code, TraceEventType.Information, format)
        {}

        public void Log()
        {
            Log(Format);
        }

        public void Log(string message)
        {
            TraceSource.TraceEvent(EventType, Id, message);
        }

        public void Log(params object[] args)
        {
            TraceSource.TraceEvent(EventType, Id, string.Format(Format, args));
        }
    }
}
