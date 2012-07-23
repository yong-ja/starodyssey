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
        public static TrackerEvent BoxSessionStart = new TrackerEvent("TrackerDesc", "START: Session {0}, Width:{1:f2} Depth:{2:f2} Height {3:f2}");
        public static TrackerEvent BoxSessionEnd = new TrackerEvent("TrackerDesc", "END: Session {0}, Time:{1:f3}");
        public static TrackerEvent ArrowLock = new TrackerEvent("TrackerDesc", "{0},{1},Locked");
        public static TrackerEvent ArrowDwell = new TrackerEvent("TrackerDesc", "{0},Dwelling");
        public static TrackerEvent ArrowIntersection = new TrackerEvent("TrackerDesc", "{0},Intersected,{1}");
        public static TrackerEvent ArrowMoveStart = new TrackerEvent("TrackerDesc", "{0},GazeMovement");
        public static TrackerEvent ArrowDeselection = new TrackerEvent("TrackerDesc", "{0},Dselected,{1}");

        protected TrackerEvent(string source, string format)
            : base(source, format)
        { }
    }
}
