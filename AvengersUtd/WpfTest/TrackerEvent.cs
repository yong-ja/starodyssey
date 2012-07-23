using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Utils.Logging;

namespace WpfTest
{
    public class TrackerEvent : LogEvent
    {
        // Session Id, gpX, gpY, valL, valR, GazeActivity
        public static TrackerEvent Gaze = new TrackerEvent("TrackerRaw", "{0},{1:f2},{2:f2},{3},{4},{5})");
        // Session Id, tpX, tpY, tdId, eventType
        public static TrackerEvent Touch = new TrackerEvent("TrackerRaw", "{0},{1:f2},{2:f2},{3},{4})");
        public static TrackerEvent BoxSessionStart = new TrackerEvent("TrackerDesc", "Starting session {0}, Width:{1:f2} Depth:{2:f2} Height {3:f2}");
        public static TrackerEvent BoxSessionEnd = new TrackerEvent("TrackerDesc", "Ending session {0}, Time:{1:f3}");
        public static TrackerEvent ArrowLock = new TrackerEvent("TrackerDesc", "{0} locked by {1}");


        protected TrackerEvent(string source, string format)
            : base(source, format)
        { }
    }
}
