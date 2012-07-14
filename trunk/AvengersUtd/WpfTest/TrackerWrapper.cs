using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobii.Eyetracking.Sdk;
using Tobii.Eyetracking.Sdk.Time;
using AvengersUtd.Odyssey.Utils.Logging;
using Tobii.Eyetracking.Sdk.Exceptions;
using System.Windows;
using AvengersUtd.Odyssey;
using SlimDX;
using System.Diagnostics.Contracts;

namespace WpfTest
{
    public class TrackerWrapper
    {
        private readonly EyetrackerBrowser trackerBrowser;
        private readonly Clock clock;
        Window window;
        IEyetracker tracker;
        EyetrackerInfo trackerInfo;
        private string connectionName;
        private SyncManager syncManager;
        public bool IsTracking { get; private set;}

        public Vector2 GazePoint { get; private set; }

        public event EventHandler<GazeEventArgs> GazeDataReceived;

        protected void OnGazeDataReceived(object sender, GazeEventArgs e)
        {
            if (GazeDataReceived != null)
                GazeDataReceived(sender, e);
        }

        public TrackerWrapper()
        {
            // Initialize Tobii SDK eyetracking library
            Library.Init();
            clock = new Clock();
            trackerBrowser = new EyetrackerBrowser();
            trackerBrowser.EyetrackerFound += EyetrackerFound;
            trackerBrowser.EyetrackerUpdated += EyetrackerUpdated;
            trackerBrowser.EyetrackerRemoved += EyetrackerRemoved;
        }

        public void SetWindow(Window window)
        {
            this.window = window;
        }

        public void StartBrowsing()
        {
            LogEvent.Engine.Write("Tracker: Start Browsing");
            trackerBrowser.Start();
        }

        public void StopBrowsing()
        {
            LogEvent.Engine.Write("Tracker: Stop Browsing");
            trackerBrowser.Stop();
        }

        public void StartTracking()
        {
            LogEvent.Engine.Write("Tracker: Tracking Started");
            tracker.StartTracking();
            IsTracking = true;
        }

        private void EyetrackerFound(object sender, EyetrackerInfoEventArgs e)
        {
            // When an eyetracker is found on the network
            trackerInfo = e.EyetrackerInfo;
            LogEvent.Engine.Write(string.Format("Found {0}.", trackerInfo.Model));
        }

        private void EyetrackerRemoved(object sender, EyetrackerInfoEventArgs e)
        {
            // When an eyetracker disappears from the network we remove it from the listview
            if (tracker != null)
                DisconnectTracker();
        }

        private void EyetrackerUpdated(object sender, EyetrackerInfoEventArgs e)
        {
            // When an eyetracker is updated we simply create a new 
            // listviewitem and replace the old one

        }

        public void DisconnectTracker()
        {
            if (tracker != null)
            {
                tracker.GazeDataReceived -= tracker_GazeDataReceived;
                tracker.Dispose();
                //_connectionName = string.Empty;
                //_isTracking = false;

                syncManager.Dispose();
            }
        }

        private void HandleConnectionError(object sender, ConnectionErrorEventArgs e)
        {
            // If the connection goes down we dispose 
            // the IAsyncEyetracker instance. This will release 
            // all resources held by the connection
            DisconnectTracker();
        }

        public void Connect()
        {
            ConnectToTracker(trackerInfo);
        }

        private void ConnectToTracker(EyetrackerInfo info)
        {
            try
            {
                LogEvent.Engine.Write("Tracker: Connecting to " + info.Model);
                tracker = EyetrackerFactory.CreateEyetracker(info);
                tracker.ConnectionError += HandleConnectionError;
                connectionName = info.ProductId;

                syncManager = new SyncManager(clock, info);

                tracker.GazeDataReceived += tracker_GazeDataReceived;
                tracker.FramerateChanged += tracker_FramerateChanged;
                LogEvent.Engine.Write(string.Format("Connected to {0} FrameRate {1}.", info.Model, tracker.GetFramerate()));
            }
            catch (EyetrackerException ee)
            {
                if (ee.ErrorCode == 0x20000402)
                {
                    LogEvent.Engine.Write("Failed to upgrade protocol. " +
                        "This probably means that the firmware needs" +
                        " to be upgraded to a version that supports the new sdk.");
                }
                else
                {
                    LogEvent.Engine.Write("Eyetracker responded with error " + ee);
                }

                DisconnectTracker();
            }
            catch (Exception)
            {
                LogEvent.Engine.Write("Could not connect to eyetracker.");
                DisconnectTracker();
            }
        }

        private void tracker_GazeDataReceived(object sender, GazeDataEventArgs e)
        {
            // Send the gaze data to the track status control.
            GazeDataItem gd = e.GazeDataItem;
            Point2D leftGaze = e.GazeDataItem.LeftGazePoint2D;
            Point2D rightGaze = e.GazeDataItem.RightGazePoint2D;

            if (leftGaze.X == -1)
                leftGaze = rightGaze;
            if (rightGaze.X == -1)
                rightGaze = leftGaze;

            Point gazePoint = new Point((leftGaze.X + rightGaze.X) / 2, (leftGaze.Y + rightGaze.Y) / 2);
            Point screenPoint = new Point(gazePoint.X * SystemParameters.PrimaryScreenWidth, gazePoint.Y * SystemParameters.PrimaryScreenHeight);
            Point clientPoint = window.PointFromScreen(screenPoint);

            GazePoint = new Vector2((float)clientPoint.X, (float)clientPoint.Y);

            OnGazeDataReceived(this, new GazeEventArgs(GazePoint));
        }

        private void tracker_FramerateChanged(object sender, FramerateChangedEventArgs e)
        {
            LogEvent.Engine.Write(string.Format("Framerate changed to {0}.", e.Framerate));
        }

    }
}
