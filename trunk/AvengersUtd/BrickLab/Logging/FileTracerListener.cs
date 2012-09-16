using System;
using System.IO;
using System.Diagnostics;

namespace AvengersUtd.BrickLab.Logging
{
    public class FileTraceListener : TraceListener
    {
        readonly string fileName;
        DateTime timeStamp;
        readonly StreamWriter traceWriter;

        public FileTraceListener(string fileNamePrefix)
        {
            // Pass in the path of the logfile (ie. C:\Logs\MyAppLog.log)
            // The logfile will actually be created with a yyyymmdd format appended to the filename
            fileName = fileNamePrefix;
            traceWriter = new StreamWriter(GenerateFilename(), true) {AutoFlush = true};
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
            string filename = fileName + '_' + timeStamp.ToString("yyyyMMdd") + ".log";

           

            return Path.Combine(Global.CurrentDir, filename);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //traceWriter.Flush();
                traceWriter.Close();
            }
        }

    }
}
