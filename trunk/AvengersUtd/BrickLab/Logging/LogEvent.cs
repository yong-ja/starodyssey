using System.Diagnostics;

namespace AvengersUtd.BrickLab.Logging
{
    public class LogEvent : AbstractLogEvent
    {
        public static LogEvent System = new LogEvent("System", "{0}");
        public static LogEvent Network = new LogEvent("Network", "{0}");

        public static LogEvent OrderChanged = new LogEvent("Network", "Order[{0}] is different from the local copy");

        public LogEvent(string source, string format) : base(source, EventCode.LogMessage, TraceEventType.Information, format)
        {}

    }
}
