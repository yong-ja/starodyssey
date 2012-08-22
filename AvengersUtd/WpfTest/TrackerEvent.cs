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
                                                                        
        
                                                                    //Participant, Rep, Axis, Arrow1, Arrow2, Arrow3, Time
        public static TrackerEvent BoxSessionStart = new TrackerEvent("TrackerBoxPerf", "{0}, {1}, {2}, {3}, {4}, {5}, {6}");
        public static TrackerEvent BoxSessionEnd = new TrackerEvent("TrackerBoxPerf", "{0}, {1}, {2}, {3}, {4}, {5}, {6:f3}");
        public static TrackerEvent BoxDataHeader = new TrackerEvent("TrackerBoxData", "{0}, {1:hh:mm:ss.fff}, {2:hh:mm:ss.fff}");
        public static TrackerEvent BoxData = new TrackerEvent("TrackerBoxData", "{0}, {1:f3}, {2:f3}, {3:f3}");

        public static TrackerEvent BoxDefinition = new TrackerEvent("TrackerDesc", "START, BoxSession {0}, Width:{1:f2} Depth:{2:f2} Height: {3:f2}");
        
        public static TrackerEvent ArrowLock = new TrackerEvent("TrackerDesc", "{0},{1},Locked");
        public static TrackerEvent ArrowDwell = new TrackerEvent("TrackerDesc", "{0},Dwelling");
        public static TrackerEvent ArrowIntersection = new TrackerEvent("TrackerDesc", "{0},Intersected,{1}");
        public static TrackerEvent ArrowMoveStart = new TrackerEvent("TrackerDesc", "{0},GazeMovement");
        public static TrackerEvent ArrowDeselection = new TrackerEvent("TrackerDesc", "{0},Deselected,{1}");

        public static TrackerEvent BezierSessionStart = new TrackerEvent("TrackerDesc", "START, BezierSession {0}, ShowRef: {1} GazeOn: {2} C: {3} Er:{4}");
        public static TrackerEvent BezierPoint = new TrackerEvent("TrackerDesc","{0}, ({1:f2},{2:f2})");
        public static TrackerEvent BezierDistance = new TrackerEvent("TrackerDesc", "{0}, distance: {1:f2}");
        public static TrackerEvent BezierSessionEnd = new TrackerEvent("TrackerDesc", "End, BezierSession {0}, Time:{1:f3}");
        public static TrackerEvent PointSessionStart= new TrackerEvent("TrackerDesc", "Start,PointSession {0}, Size:{1:f2}, Distance:{2:f2}");
        public static TrackerEvent PointSessionEnd = new TrackerEvent("TrackerDesc", "End,PointSession {0}, Time:{1:f3}");
        public static TrackerEvent PointIntersection = new TrackerEvent("TrackerDesc", "Point {0} intersected by {1}");
        public static TrackerEvent Misc = new TrackerEvent("TrackerDesc", "{0}");
        
        protected TrackerEvent(string source, string format)
            : base(source, format)
        { }
    }
}
