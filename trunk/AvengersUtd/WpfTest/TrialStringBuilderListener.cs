using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using WpfTest;

namespace WpfTest
{
    public class TrialStringBuilderListener : System.Diagnostics.TraceListener, IStopAndGo
    {
        private static string logTag = "{0},{1},";
        string fileName;
        StringBuilder sb;
        System.IO.StreamWriter traceWriter;

        public TrialStringBuilderListener(string fileNamePrefix)
        {
            sb = new StringBuilder();
            fileName = fileNamePrefix;
            traceWriter = new StreamWriter(GenerateFilename(), true);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            TraceEvent(eventCache, source, eventType, id, "No additional information.");
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            WriteLine(string.Format("{0:HH:mm:ss.fff},{1}", DateTime.Now, message, id));
        }

        public override void Write(string value)
        {
            sb.Append(value);
        }

        public override void WriteLine(string value)
        {
            sb.AppendLine(value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                traceWriter.Flush();
            }
        }

        private string GenerateFilename()
        {
            DateTime timeStamp = System.DateTime.Today;

            return Path.Combine(Path.GetDirectoryName(fileName),
               "P" + Test.Participant.ToString("D2") + "T" + Test.Count.ToString("D2") +
               Path.GetFileNameWithoutExtension(fileName) + "_" + timeStamp.ToString("yyyyMMdd") + Path.GetExtension(fileName));
        }

        public void StopAndGo()
        {
            traceWriter.Write(sb.ToString());
            traceWriter.Flush();
            traceWriter.Close();
            traceWriter.Dispose();
            sb.Clear();
            traceWriter = new StreamWriter(GenerateFilename(), true);
        }

    }
}
