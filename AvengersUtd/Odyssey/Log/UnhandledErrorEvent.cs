using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Properties;

namespace AvengersUtd.Odyssey.Log
{
    public class UnhandledErrorEvent : ErrorEvent
    {
        public UnhandledErrorEvent()
            : base(Game.EngineTag, EventCode.UnhandledException, Resources.ERR_UnhandledException)
        {
        }

        public override void RaiseException(TraceData data, params object[] args)
        {
            StringBuilder sbMessage = new StringBuilder();
            Exception ex = (Exception)args[0];
            sbMessage.AppendLine(string.Format(Format, ex.GetType().Name));
            sbMessage.AppendLine("Error in " + data.MethodSignature);

            throw ex;
        }
    }
}
