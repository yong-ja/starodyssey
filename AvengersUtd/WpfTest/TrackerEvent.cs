﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Utils.Logging;

namespace WpfTest
{
    public class TrackerEvent : LogEvent
    {
        // Session Id, gpX, gpY, valL, valR
        public static TrackerEvent Gaze = new TrackerEvent("Tracker", "{0},{1:f2},{2:f2},{3},{4})");
        // Session Id, tpX, tpY, tdId, eventType
        public static TrackerEvent Touch = new TrackerEvent("Tracker", "{0},{1:f2},{2:f2},{3},{4})");

        protected TrackerEvent(string source, string format)
            : base(source, format)
        { }
    }
}
