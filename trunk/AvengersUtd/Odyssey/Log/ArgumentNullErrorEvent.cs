using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AvengersUtd.Odyssey.Properties;

namespace AvengersUtd.Odyssey.Log
{
    public class ArgumentNullErrorEvent : ErrorEvent
    {
        public ArgumentNullErrorEvent() : base(Game.EngineTag, EventCode.ArgumentNull, Resources.ERR_ArgumentNull)
        {
        }

        public override void RaiseException(TraceData data, params object[] args)
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.AppendLine("Error in " + data.MethodSignature);
            sbMessage.AppendLine(string.Format(Format, args[0]));

            ArgumentNullException anEx = new ArgumentNullException((string)args[0],sbMessage.ToString());
            throw anEx;
        }
    }
}
