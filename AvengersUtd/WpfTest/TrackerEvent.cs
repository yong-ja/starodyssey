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
        public static TrackerEvent BoxSessionStart = new TrackerEvent("TrackerDesc", "START: BoxSession {0}, Width:{1:f2} Depth:{2:f2} Height {3:f2}");
        public static TrackerEvent BoxSessionEnd = new TrackerEvent("TrackerDesc", "END: BoxSession {0}, Time:{1:f3}");
        public static TrackerEvent ArrowLock = new TrackerEvent("TrackerDesc", "{0},{1},Locked");
        public static TrackerEvent ArrowDwell = new TrackerEvent("TrackerDesc", "{0},Dwelling");
        public static TrackerEvent ArrowIntersection = new TrackerEvent("TrackerDesc", "{0},Intersected,{1}");
        public static TrackerEvent ArrowMoveStart = new TrackerEvent("TrackerDesc", "{0},GazeMovement");
        public static TrackerEvent ArrowDeselection = new TrackerEvent("TrackerDesc", "{0},Deselected,{1}");

        public static TrackerEvent BezierSessionStart = new TrackerEvent("TrackerDesc", "START: BezierSession {0}, Radius:{1:f2}, Rotation:{2:f2}, RefPoints{3}");
        public static TrackerEvent BezierSessionEnd = new TrackerEvent("TrackerDesc", "End:BezierSession {0}, Time:{1:f3}");
        public static TrackerEvent PointSessionStart= new TrackerEvent("TrackerDesc", "Start:PointSession {0}, Size:{1:f2}, Distance:{2:f2}");
        public static TrackerEvent PointSessionEnd = new TrackerEvent("TrackerDesc", "End:PointSession {0}, Time:{1:f3}");
        public static TrackerEvent PointIntersection = new TrackerEvent("TrackerDesc", "Point {0} intersected by {1}");
        
        protected TrackerEvent(string source, string format)
            : base(source, format)
        { }
    }
}
