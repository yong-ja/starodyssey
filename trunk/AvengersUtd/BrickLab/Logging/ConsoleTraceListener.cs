using System;
using System.Diagnostics;

namespace AvengersUtd.BrickLab.Logging
{
    public class ConsoleTraceListener : TraceListener
    {
        private const string LogTag = "[{0:HH:mm:ss.fff]}:\t{1}({2})";

        public ConsoleTraceListener(string name) : base(name)
        {
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
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
        
        
    }
}
