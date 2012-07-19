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
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using AvengersUtd.Odyssey.Utils.Logging;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for GameTask.xaml
    /// </summary>
    public partial class GameTask : SurfaceWindow
    {
        int gazeRadius;
        Dictionary<TouchDevice, Point> touchPoints;
        List<Dot> targets;
        TrackerWrapper tracker;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GameTask()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
            Init();
        }

        void Init()
        {
            gazeRadius = (int)CrossHair.Width / 2;
           
            touchPoints = new Dictionary<TouchDevice, Point>();
            Loaded += new RoutedEventHandler(GameTask_Loaded);
            Canvas.TouchDown += new EventHandler<TouchEventArgs>(Canvas_TouchDown);
            Canvas.TouchMove += new EventHandler<TouchEventArgs>(Canvas_TouchMove);
            LostTouchCapture += new EventHandler<TouchEventArgs>(Canvas_LostTouchCapture);
            bConnect.Click += (sender, e) => { tracker.Connect(); };
            bStart.Click += (sender, e) => { tracker.StartTracking(); };
            bNew.Click += (sender, e) => { NewSession(); };

            NewSession();
        }

        void NewSession()
        {
            int radius = 32;
            int maxDistance = 256;
            Indicator.Fill = Brushes.Red;

            if (targets != null && targets.Count > 0)
            {
                foreach (Dot d in targets)
                    Canvas.Children.Remove(d);
                targets.Clear();
            }

            Point center = GeoHelper.ChooseRandomPointOnCircle(new Point(960, 540), 256);
            Point[] points = GeoHelper.ChooseTrianglePoints(center, maxDistance, radius);
            Dot target1 = new Dot() { Fill = Brushes.Red, Radius = radius, Center=points[0]};
            Dot target2 = new Dot() { Fill = Brushes.Blue, Radius = radius, Center = points[1]};
            Dot target3 = new Dot() { Fill = Brushes.Green, Radius = radius, Center =points[2]};

            targets = new List<Dot>() { target1, target2, target3 };
            Canvas.Children.Add(target1);
            Canvas.Children.Add(target2);
            Canvas.Children.Add(target3);
        }

        void Canvas_LostTouchCapture(object sender, TouchEventArgs e)
        {
            touchPoints.Remove(e.TouchDevice);
        }

        void Canvas_TouchMove(object sender, TouchEventArgs e)
        {
            touchPoints[e.TouchDevice] = e.GetTouchPoint(Canvas).Position;
        }

        void Canvas_TouchDown(object sender, TouchEventArgs e)
        {
            TouchDevice touchDevice = e.TouchDevice;
            Point location = e.GetTouchPoint(this).Position;
            Canvas.CaptureTouch(touchDevice);
            //touchDevice.Capture(Canvas);
            touchPoints.Add(touchDevice, location);
        }

        void GameTask_Loaded(object sender, RoutedEventArgs e)
        {
            WpfTextTraceListener.SetTextOutput(Status);
            WindowState = WindowState.Maximized;
            tracker = new TrackerWrapper();
            tracker.SetWindow(this);
            tracker.StartBrowsing();
            tracker.GazeDataReceived += new EventHandler<GazeEventArgs>(tracker_GazeDataReceived);
        }

        void tracker_GazeDataReceived(object sender, GazeEventArgs e)
        {
            TranslateTransform transform = (TranslateTransform)CrossHair.RenderTransform;
            transform.X = e.GazePoint.X - gazeRadius;
            transform.Y = e.GazePoint.Y - gazeRadius;

            //if (knotPoints.Count < 2)
            //    return;

            if (e.GazePoint.X < 0 || e.GazePoint.X > 1920 || e.GazePoint.Y < 0 || e.GazePoint.Y > 1080)
            {
                LogEvent.Engine.Write(string.Format("Rejected GP({0:f2},{1:f2} -> Outside screen bounds", e.GazePoint.X, e.GazePoint.Y));
                return;
            }

            if (touchPoints.Count < 2)
                return;

            Point gazeLocation = new Point(e.GazePoint.X, e.GazePoint.Y);

            bool result = false;
            foreach (Dot d in targets)
            {
                Point[] pointArray = touchPoints.Values.ToArray();
                result = d.IntersectsWith(pointArray[0]) || d.IntersectsWith(pointArray[1]) || d.IntersectsWith(gazeLocation);
                if (!result)
                    return;
            }

            Indicator.Fill = Brushes.Green;
            LogEvent.Engine.Write(string.Format("GP({0:f2},{1:f2}", e.GazePoint.X, e.GazePoint.Y));

        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
            tracker.DisconnectTracker();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }
    }
}