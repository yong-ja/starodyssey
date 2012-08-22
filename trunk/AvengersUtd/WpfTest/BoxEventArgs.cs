using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfTest
{
    public class BoxEventArgs : EventArgs
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public double Duration
        {
            get
            {
                return (EndTime - StartTime).TotalMilliseconds / 1000d;
            }
        }

        public BoxEventArgs(DateTime start, DateTime end)
        {
            StartTime = start;
            EndTime = end;
        }
    }
}
