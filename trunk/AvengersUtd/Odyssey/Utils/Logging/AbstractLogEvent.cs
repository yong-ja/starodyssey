#region Using directives

using System.Diagnostics;
using System.Diagnostics.Contracts;

#endregion

namespace AvengersUtd.Odyssey.Utils.Logging
{
    public enum EventCode : int
    {
        LogMessage = 10001,
        // Warnings
        ThreadAbort = 70001,
        // Exceptions
        ArgumentException = 80001,
        ArgumentNull = 80002,
        // Critical Errors
        CriticalFault = 90001,
        UnhandledException = 90002,
        
        
    }

    public abstract class AbstractLogEvent
    {
        private readonly int id;
        private readonly TraceSource ts;

        protected AbstractLogEvent(string source, EventCode eventCode, TraceEventType eventType, string format)
        {
            Contract.Requires(!string.IsNullOrEmpty(source));

            Source = source;
            Format = format;
            EventCode = eventCode;
            EventType = eventType;
            id = (int) eventCode;

            ts = new TraceSource(Source);
        }

        public int Id
        {
            get { return id; }
        }

        public EventCode EventCode { get; private set; }

        public TraceEventType EventType { get; private set; }

        protected string Source { get; private set; }

        protected internal string Format { get; private set; }

        protected TraceSource TraceSource
        {
            get { return ts; }
        }

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