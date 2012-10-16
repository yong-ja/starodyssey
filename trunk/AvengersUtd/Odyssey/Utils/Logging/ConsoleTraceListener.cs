using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;

namespace AvengersUtd.Odyssey.Utils.Logging
{
    public class ConsoleTraceListener : TraceListener
    {
        private const string LogTag = "[{0:HH:mm:ss.fff]}:\t{1}({2})";
        TraceEventType eventType;
        int id;
        string source;
        TextWriter tw;

        public ConsoleTraceListener(string name) : base(name)
        {
            tw = System.Console.Out;
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            this.source = source;
            this.eventType = eventType;
            this.id = id;
            TraceEvent(eventCache, source, eventType, id, "No additional information.");
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            string logEntry = string.Format(LogTag, DateTime.Now, eventType != TraceEventType.Information ? eventType.ToString() : string.Empty, source);
            WriteLine(string.Format("{0} {1}", logEntry, message,id));
        }

        /// <summary>
        /// Writes the specified message to the console.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            Debugger.Log(id, source, message);
        }

        /// <summary>
        /// Writes a message to the console, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            Debugger.Log(id, source, message + "\n");
            //base.WriteLine(message);
        }
        
        static string GetCode(TraceEventType eventType, int eventCode)
        {
            switch (eventType)
            {
                default:
                    return string.Empty;
                    
                case TraceEventType.Warning:
                    return string.Format(" Warning #{0:000000}\n", eventCode);

                case TraceEventType.Critical:
                    return string.Format(" Critical Failure #{0:000000}\n", eventCode);

                case TraceEventType.Error:
                    return string.Format(" Error #{0:000000}\n", eventCode);

            }
        }
        
    }
}
