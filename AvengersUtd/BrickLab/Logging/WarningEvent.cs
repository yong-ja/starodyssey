using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AvengersUtd.BrickLab.Logging
{
    public class WarningEvent : AbstractLogEvent
    {
        public static WarningEvent FileDoesNotExist = new WarningEvent("System", "File [{0}] does not exist", EventCode.FileDoesNotExist);

        public WarningEvent(string source, string format, EventCode code) : base(source, code, TraceEventType.Warning, format)
        {}
    }
}
