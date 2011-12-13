using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Properties;

namespace AvengersUtd.Odyssey.Log
{
    public class ArgumentInvalidErrorEvent : ErrorEvent
    {
        public ArgumentInvalidErrorEvent() : base(Game.EngineTag, EventCode.ArgumentException, Resources.ERR_Argument)
        {
        }

        public override void RaiseException(TraceData data, params object[] args)
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.AppendLine(string.Format(Format, args[0], args[1]));
            sbMessage.AppendLine("Error in " + data.MethodSignature);

            ArgumentException aEx = new ArgumentException(sbMessage.ToString(), (string)args[0]);
            throw aEx;
        }
    }
}
