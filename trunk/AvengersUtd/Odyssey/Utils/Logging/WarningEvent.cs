using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Properties;

namespace AvengersUtd.Odyssey.Utils.Logging
{
    public class WarningEvent : AbstractLogEvent
    {
        public static WarningEvent ThreadAborted = new WarningEvent(Game.EngineTag, Resources.ERR_Thread_Aborted,
                                                                    EventCode.ThreadAbort);

        protected WarningEvent(string source, string format, EventCode eventCode) :base(source, eventCode, TraceEventType.Warning, format)
        {
        }
    }
}
