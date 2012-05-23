using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AvengersUtd.Odyssey.UserInterface.Controls;

namespace AvengersUtd.Odyssey.Utils.Logging
{
    public class LoggerTraceListener : TraceListener
    {
        private static LoggerPanel loggerPanel;
        private const string LogTag = "[{0:HH:mm:ss.fff]}:\t{1}({2})";
        

        public LoggerTraceListener(string name)
            : base(name)
        {
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            TraceEvent(eventCache, source, eventType, id, "No additional information.");
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            string logEntry = string.Format(LogTag, DateTime.Now, eventType != TraceEventType.Information ? eventType.ToString() : string.Empty, source);
            loggerPanel.EnqueueMessage(string.Format("{0} {1}", logEntry, message,id));
            
        }

        /// <summary>
        /// Writes the specified message to the console.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            loggerPanel.EnqueueMessage(message);
        }

        /// <summary>
        /// Writes a message to the console, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            loggerPanel.EnqueueMessage(message);
        }

        public static void SetLoggerPanel(LoggerPanel lPanel)
        {
            loggerPanel = lPanel;
        }

    }
}
