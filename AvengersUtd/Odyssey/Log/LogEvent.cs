﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Properties;

namespace AvengersUtd.Odyssey.Log
{
    public class LogEvent : AbstractLogEvent
    {

        public static LogEvent Engine = new LogEvent(Game.EngineTag);
        //public static LogEvent ObjectDisposing = new LogEvent(Game.EngineTag, Resources.INFO_OE_Disposing);
        public static LogEvent ObjectDisposed = new LogEvent(Game.EngineTag, Resources.INFO_OE_Disposed);

        public LogEvent(string source) : this(source, Resources.ERR_NoInformation)
        {}

        public LogEvent(string source, string format) : base(source, EventCode.LogMessage, TraceEventType.Information, format)
        {}




    }
}
