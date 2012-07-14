using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Controls;
using System.Diagnostics.Contracts;

namespace AvengersUtd.Odyssey.Utils.Logging
{
    public class WpfTextTraceListener : TraceListener
    {
        private const string LogTag = "[{0:HH:mm:ss.fff]}:\t{1}({2})";
        private static TextBlock output;

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            TraceEvent(eventCache, source, eventType, id, "No additional information."); 
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            string logEntry = string.Format(LogTag, DateTime.Now, eventType != TraceEventType.Information ? eventType.ToString() : string.Empty, source);
            WriteLine(string.Format("{0} {1}", logEntry, message, id));
        }

        /// <summary>
        /// Writes the specified message to the console.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            if (output == null)
            {
                Console.Write("Warning: no WPF output control defined\n" + message);
                return;
            }
            output.Text += message;
        }

        /// <summary>
        /// Writes a message to the console, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            if (output == null)
            {
                Console.Write("Warning: no WPF output control defined\n" + message);
                return;
            }
            output.Text = message;
        }

        public static void SetTextOutput(TextBlock textControl)
        {
            Contract.Requires(textControl != null);
            output = textControl;
        }
        
    }
}
