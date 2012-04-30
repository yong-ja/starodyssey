using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using AvengersUtd.Odyssey.Properties;

namespace AvengersUtd.Odyssey.Utils.Logging
{
    public class CriticalEvent : AbstractLogEvent
    {
        public delegate void LogMethod(CriticalEvent logEvent, TraceData data, Exception ex);

        public static CriticalEvent Unhandled = new CriticalEvent(Game.EngineTag, Resources.ERR_UnhandledException, EventCode.UnhandledException);
        public static CriticalEvent MissingFile = new CriticalEvent(Game.EngineTag, Resources.ERR_IO_MissingFile, EventCode.CriticalFault, LogErrorFilename);
        public static CriticalEvent ShaderCompilationError = new CriticalEvent(Game.EngineTag, Resources.ERR_IO_EffectCompilationError, EventCode.CriticalFault, LogErrorFilename);

        LogMethod logMethod;

        protected CriticalEvent(string source,  string format, EventCode eventCode, LogMethod logMethod) 
            : base(source, eventCode, TraceEventType.Critical, format)
        {
            this.logMethod = logMethod;
        }
        
        protected CriticalEvent(string source,  string format, EventCode eventCode) 
            : this(source,format, eventCode, LogError)
        {
            logMethod = LogError;
        }

        internal static void LogError(CriticalEvent logEvent, TraceData data, Exception ex)
        {
            Contract.Requires(ex != null);
            Contract.Requires(logEvent != null);
            
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.Append(string.Format(logEvent.Format, ex.Message));
            sbMessage.Append(string.Format(" [#{0}]", logEvent.Id));
            sbMessage.Append("\n\tError in " + data.MethodSignature);
            logEvent.TraceSource.TraceEvent(TraceEventType.Critical, logEvent.Id, sbMessage.ToString()); 
        }

        internal static void LogErrorFilename(CriticalEvent logEvent, TraceData data, Exception ex)
        {
            Contract.Requires(logEvent != null);

            StringBuilder sbMessage = new StringBuilder();
            sbMessage.Append(string.Format(logEvent.Format, data.GetValue("filename")));
            sbMessage.Append(string.Format(" [#{0}]", logEvent.Id));
            if (ex!= null) {
                sbMessage.Append("\n\tError in " + data.MethodSignature);
                sbMessage.AppendLine(ex.Message);
            }
            logEvent.TraceSource.TraceEvent(TraceEventType.Critical, logEvent.Id, sbMessage.ToString()); 
        }


        public void LogError(TraceData data, Exception ex = null)
        {
            logMethod(this, data, ex);
        }

    }
}
