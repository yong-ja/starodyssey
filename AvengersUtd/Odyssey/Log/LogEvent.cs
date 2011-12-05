using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Log
{
    public class LogEvent : AbstractLogEvent
    {
        public LogEvent(string source, EventCode code) : this(source,code, string.Empty)
        {}

        public LogEvent(string source, EventCode code, string format) : base(source, code, TraceEventType.Verbose, format)
        {}
    }
}
