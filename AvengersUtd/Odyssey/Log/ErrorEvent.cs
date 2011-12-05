using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Log
{
    public class ErrorEvent : AbstractLogEvent
    {
        public ErrorEvent(string source, EventCode code, string format) : base(source, code, System.Diagnostics.TraceEventType.Error, format)
        {
        }

        public void RaiseArgumentException(params object[] data)
        {
            Log(data);
            throw new ArgumentException(string.Format(Format, data));
        }

    }
}
