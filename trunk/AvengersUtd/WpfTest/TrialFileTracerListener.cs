using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using WpfTest;

namespace WpfTest
{

    public interface IStopAndGo
    {
        void StopAndGo();
    }

    public class TrialFileTraceListener : System.Diagnostics.TraceListener, IStopAndGo
    {
        private static string logTag = "{0},{1},";
        string fileName;
        System.DateTime timeStamp;
        System.IO.StreamWriter traceWriter;

        public TrialFileTraceListener(string fileNamePrefix)
        {
            // Pass in the path of the logfile (ie. C:\Logs\MyAppLog.log)
            // The logfile will actually be created with a yyyymmdd format appended to the filename
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
            traceWriter.Write(value);
        }

        public override void WriteLine(string value)
        {
            traceWriter.WriteLine(value);
        }

        private string GenerateFilename()
        {
            timeStamp = System.DateTime.Today;

            return Path.Combine(Path.GetDirectoryName(fileName),
               "P" + Test.Participant.ToString("D2") + "T" + Test.Count.ToString("D2") +
               Path.GetFileNameWithoutExtension(fileName) + "_" + timeStamp.ToString("yyyyMMdd") +  Path.GetExtension(fileName));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                traceWriter.Flush();
                //traceWriter.Close();
            }
        }

        public void StopAndGo()
        {
            traceWriter.Flush();
            traceWriter.Close();
            traceWriter.Dispose();
            traceWriter = new StreamWriter(GenerateFilename(), true);
        }

    }
}
