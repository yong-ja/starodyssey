using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using AvengersUtd.Odyssey.Properties;

namespace AvengersUtd.Odyssey.Log
{
    public class CriticalEvent : AbstractLogEvent
    {
        public static CriticalEvent UnhandledEvent = new CriticalEvent(Game.EngineTag, Resources.ERR_UnhandledException, EventCode.UnhandledException);

        protected CriticalEvent(string source,  string format, EventCode eventCode) : base(source, eventCode, TraceEventType.Critical, format)
        {
        }

        public void LogError(TraceData data, Exception ex)
        {
            Contract.Requires(ex != null);
            
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.AppendLine(string.Format(Format, ex.Message));
            sbMessage.AppendLine("Error in " + data.MethodSignature);
            TraceSource.TraceEvent(TraceEventType.Critical, Id, sbMessage.ToString());
        }

        



    }
}
