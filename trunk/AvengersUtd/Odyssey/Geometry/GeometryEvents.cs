using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Log;

namespace AvengersUtd.Odyssey.Geometry
{
    public static class GeometryEvents
    {
        public static readonly LogEvent LogEvent = new LogEvent("Geometry", EventCode.LogMessage, "{0}");

        public static readonly ErrorEvent GeometryVerticesException = new ErrorEvent("Geometry", EventCode.ArgumentException,
                                                                            "{0} has {1} vertices, expected {2}");
    }
}
