using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tobii.Eyetracking.Sdk;
using System.Diagnostics.Contracts;
using Tobii.Eyetracking.Sdk.Time;
using Tobii.Eyetracking.Sdk.Exceptions;

namespace EyeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly EyetrackerBrowser trackerBrowser;
        private readonly Clock clock;
        IEyetracker tracker;
        EyetrackerInfo trackerInfo;
        private string connectionName;
        private SyncManager syncManager;

        public MainWindow()
        {
            // Initialize Tobii SDK eyetracking library
            Library.Init();
            InitializeComponent();
            clock = new Clock();
            trackerBrowser = new EyetrackerBrowser();
            trackerBrowser.EyetrackerFound += EyetrackerFound;
            trackerBrowser.EyetrackerUpdated += EyetrackerUpdated;
            trackerBrowser.EyetrackerRemoved += EyetrackerRemoved;
            
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            Contract.Requires(trackerInfo != null);
            // Disconnect existing connection
            DisconnectTracker();

            // Create new connection
            ConnectToTracker(trackerInfo);
            miTracking.IsEnabled = true;
            
        }

        private void Tracking_Click(object sender, RoutedEventArgs e)
        {

            if (miTracking.IsChecked)
                tracker.StartTracking();
            else
                tracker.StopTracking();

        }

        private void EyetrackerFound(object sender, EyetrackerInfoEventArgs e)
        {
            // When an eyetracker is found on the network
            trackerInfo = e.EyetrackerInfo;
            StatusText.Text = string.Format("Found {0}.", trackerInfo.Model);
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

        private void DisconnectTracker()
        {
            if (tracker != null)
            {
                //tracker.GazeDataReceived -= _connectedTracker_GazeDataReceived;
                tracker.Dispose();
                //_connectionName = string.Empty;
                //_isTracking = false;

                //_syncManager.Dispose();
            }
        }

        private void HandleConnectionError(object sender, ConnectionErrorEventArgs e)
        {
            // If the connection goes down we dispose 
            // the IAsyncEyetracker instance. This will release 
            // all resources held by the connection
            DisconnectTracker();
        }

        private void ConnectToTracker(EyetrackerInfo info)
        {
            try
            {
                tracker = EyetrackerFactory.CreateEyetracker(info);
                tracker.ConnectionError += HandleConnectionError;
                connectionName = info.ProductId;

                syncManager = new SyncManager(clock, info);

                tracker.GazeDataReceived += tracker_GazeDataReceived;
                tracker.FramerateChanged += tracker_FramerateChanged;
                StatusText.Text = string.Format("Connected to {0}.", info.Model);
            }
            catch (EyetrackerException ee)
            {
                if (ee.ErrorCode == 0x20000402)
                {
                    MessageBox.Show("Failed to upgrade protocol. " +
                        "This probably means that the firmware needs" +
                        " to be upgraded to a version that supports the new sdk.", "Upgrade Failed", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Eyetracker responded with error " + ee, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                DisconnectTracker();
            }
            catch (Exception)
            {
                MessageBox.Show("Could not connect to eyetracker.", "Connection Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                DisconnectTracker();
            }

        }

        private void tracker_GazeDataReceived(object sender, GazeDataEventArgs e)
        {
            // Send the gaze data to the track status control.
            GazeDataItem gd = e.GazeDataItem;
            Console.WriteLine(string.Format("L({0:f2},{1:f2},{2:f2}) R:({3:f2},{4:f2},{5:f2})",
                e.GazeDataItem.LeftEyePosition3DRelative.X, e.GazeDataItem.LeftEyePosition3DRelative.Y, e.GazeDataItem.LeftEyePosition3DRelative.Z,
                e.GazeDataItem.RightEyePosition3DRelative.X, e.GazeDataItem.RightEyePosition3DRelative.Y, e.GazeDataItem.RightEyePosition3DRelative.Z));
            

            if (syncManager.SyncState.StateFlag == SyncStateFlag.Synchronized)
            {
                Int64 convertedTime = syncManager.RemoteToLocal(gd.TimeStamp);
                Int64 localTime = clock.GetTime();
            }
            else
            {
                StatusText.Text = string.Format("Warning. Sync state is {0}", syncManager.SyncState.StateFlag);
            }
        }

        private void tracker_FramerateChanged(object sender, FramerateChangedEventArgs e)
        {
            StatusText.Text = string.Format("Framerate changed to {0}.",e.Framerate);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            trackerBrowser.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            trackerBrowser.Stop();
        }
    }
}
