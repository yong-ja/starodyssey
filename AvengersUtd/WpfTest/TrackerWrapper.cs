using System;
using System.Collections.Generic;
using Tobii.Eyetracking.Sdk;
using Tobii.Eyetracking.Sdk.Time;
using AvengersUtd.Odyssey.Utils.Logging;
using Tobii.Eyetracking.Sdk.Exceptions;
using System.Windows;
using SlimDX;

namespace WpfTest
{
    public class TrackerWrapper
    {
        private readonly EyetrackerBrowser trackerBrowser;
        private readonly Clock clock;
        private Window window;
        private IEyetracker tracker;
        private EyetrackerInfo trackerInfo;
        private SyncManager syncManager;
        private string connectionName;
        private readonly AverageWindow smoother;
        public bool IsTracking { get; private set; }
        public bool IsConnected { get; private set; }

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
            smoother = new AverageWindow(10);
        }

        public void SetWindow(Window hostWindow)
        {
            this.window = hostWindow;
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

        public void StopTracking()
        {
            LogEvent.Engine.Write("Tracker: Tracking Stopped");
            tracker.StopTracking();
            IsTracking = false;
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
                connectionName = string.Empty;
                IsConnected = IsTracking = false;
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
                tracker.SetFramerate(60);
                ListFramerates();
                IsConnected = true;
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
                    LogEvent.Engine.Write("Eyetracker responded with error " + ee);

                DisconnectTracker();
            }
            catch (Exception)
            {
                LogEvent.Engine.Write("Could not connect to eyetracker.");
                DisconnectTracker();
            }
        }

        private void ListFramerates()
        {
            IList<float> framerates = tracker.EnumerateFramerates();
            for (int i = 0; i < framerates.Count; i++)
            {
                float f = framerates[i];
                Console.WriteLine(string.Format("Framerate #[{0}] = {1:f2}", i, f));
            }
        }


        /// <summary>
        /// This is the function that handles the gaze data received from the tracker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tracker_GazeDataReceived(object sender, GazeDataEventArgs e)
        {
            // 'left' and 'right' are the eyes
            Point2D leftGaze = e.GazeDataItem.LeftGazePoint2D;
            Point2D rightGaze = e.GazeDataItem.RightGazePoint2D;

            if (leftGaze.X == -1)
                leftGaze = rightGaze;
            if (rightGaze.X == -1)
                rightGaze = leftGaze;

            Point gazePoint = new Point((leftGaze.X + rightGaze.X)/2, (leftGaze.Y + rightGaze.Y)/2);
            Point screenPoint = new Point(gazePoint.X*SystemParameters.PrimaryScreenWidth,
                                          gazePoint.Y*SystemParameters.PrimaryScreenHeight);
            Point clientPoint = window.PointFromScreen(screenPoint);

            Point smoothedPoint = smoother.Smooth(clientPoint);
            GazePoint = new Vector2((float) smoothedPoint.X, (float) smoothedPoint.Y);

            OnGazeDataReceived(this, new GazeEventArgs(GazePoint, e.GazeDataItem.LeftValidity, e.GazeDataItem.RightValidity));
        }

        private void tracker_FramerateChanged(object sender, FramerateChangedEventArgs e)
        {
            LogEvent.Engine.Write(string.Format("Framerate changed to {0}.", e.Framerate));
        }
    }
}