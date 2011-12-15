using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Utils.Logging
{
    public class ConsoleTraceListener : TraceListener
    {
        private const string LogTag = "[{0:HH:mm:ss.fff]} ({1}):{2}";

        

        public ConsoleTraceListener(string name) : base(name)
        {
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            TraceEvent(eventCache, source, eventType, id, "No additional information.");
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            string logEntry = string.Format(LogTag, DateTime.Now, source, GetCode(eventType, id));
            WriteLine(string.Format("{0} {1}", logEntry, message));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            string logEntry = string.Format(LogTag, DateTime.Now, source, GetCode(eventType, id));
            WriteLine(string.Format("{0} {1}", logEntry, string.Format(format, args)));
        }

        /// <summary>
        /// Writes the specified message to the console.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            Console.Write(message);
        }

        /// <summary>
        /// Writes a message to the console, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        
        static string GetCode(TraceEventType eventType, int eventCode)
        {
            switch (eventType)
            {
                default:
                    return string.Empty;
                    
                case TraceEventType.Warning:
                    return string.Format("Warning #{0:000000} -", eventCode);

                case TraceEventType.Critical:
                    return string.Format("Critical Failure #{0:000000} -", eventCode);

                case TraceEventType.Error:
                    return string.Format("Error #{0:000000} -", eventCode);

            }
        }
        
    }
}
