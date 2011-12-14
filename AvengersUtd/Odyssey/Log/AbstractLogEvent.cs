using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Log
{

    public enum EventCode : int
    {
        LogMessage = 10001,
        ArgumentException = 80001,
        ArgumentNull,
        UnhandledException = 90000,
        CriticalFault,
        ThreadAbort
    }

    public abstract class AbstractLogEvent
    {
        private readonly TraceSource ts;

        private readonly int id;

        public int Id
        {
            get { return id; }
        }

        public EventCode EventCode { get; private set; }

        public TraceEventType EventType { get; private set; }

        protected string Source { get; private set; }

        protected string Format { get; private set; }

        protected TraceSource TraceSource
        {
            get { return ts; }
        }

        protected AbstractLogEvent(string source, EventCode eventCode, TraceEventType eventType, string format)
        {
            Contract.Requires(!string.IsNullOrEmpty(source));

            Source = source;
            Format = format;
            EventCode = eventCode;
            EventType = eventType;
            id = (int)eventCode;

            ts = new TraceSource(this.Source);
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
