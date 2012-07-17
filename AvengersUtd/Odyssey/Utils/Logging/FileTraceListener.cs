using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AvengersUtd.Odyssey.Utils.Logging
{
    public class FileTraceListener : System.Diagnostics.TraceListener
    {
        private static string logTag = "{0},{1}";
        string fileName;
        System.DateTime timeStamp;
        System.IO.StreamWriter traceWriter;

        public FileTraceListener(string fileNamePrefix
            )
        {
            // Pass in the path of the logfile (ie. C:\Logs\MyAppLog.log)
            // The logfile will actually be created with a yyyymmdd format appended to the filename
            fileName = fileNamePrefix;
            traceWriter = new StreamWriter(generateFilename(), true);
        }

        public override void Write(string value)
        {
            traceWriter.Write(value);
        }

        public override void WriteLine(string value)
        {
            traceWriter.WriteLine(value);
        }

        private string generateFilename()
        {
            timeStamp = System.DateTime.Today;

            return Path.Combine(Path.GetDirectoryName(fileName),
               Path.GetFileNameWithoutExtension(fileName) + "_" +
               timeStamp.ToString("yyyymmdd") + Path.GetExtension(fileName));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                traceWriter.Flush();
                traceWriter.Close();
            }
        }

    }
}
