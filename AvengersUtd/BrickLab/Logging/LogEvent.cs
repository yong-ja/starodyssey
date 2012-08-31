using System.Diagnostics;

namespace AvengersUtd.BrickLab.Logging
{
    public class LogEvent : AbstractLogEvent
    {
        public static LogEvent Network = new LogEvent("Network", "{0}");

        public LogEvent(string source, string format) : base(source, EventCode.LogMessage, TraceEventType.Information, format)
        {}

    }
}
