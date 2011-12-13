using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Log
{
    public enum EventCode : int
    {
        LogMessage = 10001,
        ArgumentException = 90001,
        ArgumentNull,
        UnhandledException = 90000
    }

    public abstract class AbstractLogEvent
    {
        private readonly TraceSource ts;

        private readonly string source;
        private readonly EventCode code;
        private readonly int id;
        private readonly string format;
        private readonly TraceEventType eventType;

        public int Id
        {
            get { return id; }
        }

        public TraceEventType EventType
        {
            get { return eventType; }
        }

        protected string Source
        {
            get { return source; }
        }

        protected string Format
        {
            get { return format; }
        }

        protected EventCode Code
        {
            get { return code; }
        }

        protected TraceSource TraceSource
        {
            get { return ts; }
        }

        protected AbstractLogEvent(string source, EventCode code, TraceEventType eventType, string format)
        {
            this.source = source;
            this.code = code;
            this.format = format;
            this.eventType = eventType;
            id = (int)code;
            ts = new TraceSource(this.source);
        }




    }
}
