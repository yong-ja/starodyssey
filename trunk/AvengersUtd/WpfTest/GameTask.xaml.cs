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
        List<Target> targets;
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
            targets = new List<Target>() { Invader1, Invader2, Invader3 };
            touchPoints = new Dictionary<TouchDevice, Point>();
            Loaded += new RoutedEventHandler(GameTask_Loaded);
            Canvas.TouchDown += new EventHandler<TouchEventArgs>(Canvas_TouchDown);
            Canvas.TouchMove += new EventHandler<TouchEventArgs>(Canvas_TouchMove);
            Canvas.LostTouchCapture += new EventHandler<TouchEventArgs>(Canvas_LostTouchCapture);
            bConnect.TouchUp += (sender, e) => { tracker.Connect(); };
            bStart.TouchUp += (sender, e) => { tracker.StartTracking(); };


            //Ellipse ellipseRadius = new System.Windows.Shapes.Ellipse()
            //{
            //    Width = 2 * radiusSize,
            //    Height = 2 * radiusSize,
            //    Stroke = Brushes.Black,
            //    RenderTransform = new TranslateTransform(8, 1070)
            //};
            //Canvas.Children.Add(ellipseRadius);
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
            foreach (Target t in targets)
            {
                Point[] pointArray = touchPoints.Values.ToArray();
                result = t.IntersectsWith(pointArray[0]) || t.IntersectsWith(pointArray[1]) || t.IntersectsWith(gazeLocation);
                if (!result)
                    return;
            }

            Indicator.Fill = Brushes.Green;

            //prevEyeLocation = newLocation;

            //Vector delta = Point.Subtract(newLocation, prevEyeLocation);
            //if (delta.LengthSquared < 16 * 16)
            //{
            //    LogEvent.Engine.Write(string.Format("Rejected GP({0:f2},{1:f2} -> Too close", e.GazePoint.X, e.GazePoint.Y));
            //    return;
            //}
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