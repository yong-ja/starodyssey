using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using AvengersUtd.Odyssey.Properties;

namespace AvengersUtd.Odyssey.Log
{
    public abstract class ErrorEvent : AbstractLogEvent
    {
        public static ErrorEvent ArgumentNull = new ArgumentNullErrorEvent();
        public static ErrorEvent ArgumentInvalid = new ArgumentInvalidErrorEvent();
        //public static ErrorEvent ArgumentNull = new ErrorEvent(Game.EngineTag, EventCode.ArgumentNull, Resources.ERR_ArgumentNull);
        //public static ErrorEvent ArgumentInvalid = new ErrorEvent(Game.EngineTag, EventCode.ArgumentException, Resources.ERR_Argument);

        //internal static ErrorEvent UnhandledException = new ErrorEvent(Game.EngineTag, EventCode.UnhandledException,
        //                                                             Resources.ERR_UnhandledException);

        protected ErrorEvent(string source, EventCode code, string format) : base(source, code, TraceEventType.Error, format)
        {
        }

        public abstract void RaiseException(TraceData data, params object[] args);

        public static void ReportException(Type type, MethodBase method, Exception ex, params object[] args)
        {
            ParameterInfo[] parms = method.GetParameters();
            object[] namevalues = new object[2 * parms.Length];

            string msg = string.Format("Error in {0}.{1}(", type.Name, method.Name);
            for (int i = 0, j = 0; i < parms.Length; i++, j += 2)
            {
                msg += "{" + j + "}={" + (j + 1) + "}, ";
                namevalues[j] = parms[i].Name;
                if (i < args.Length) namevalues[j + 1] = args[i];
            }
            msg += "exception=" + ex.Message + ")";
           // UnhandledException.Log(msg);
        }



    }
}
