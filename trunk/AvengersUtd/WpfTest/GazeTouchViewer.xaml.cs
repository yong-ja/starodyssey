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
    /// Interaction logic for GazeTouchViewer.xaml
    /// </summary>
    public partial class GazeTouchViewer : SurfaceWindow
    {
        const int sessionId=0;
        int gazeRadius;
        bool gazeOnly;
        Brush touchBrush;
        Brush eyeBrush;

        Point lastGazePoint;
        TrackerWrapper tracker;
        Dictionary<TouchDevice, Brush> points;
        List<FrameworkElement> hitTestResults;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GazeTouchViewer()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
            Init();
        }

        void Init()
        {
            gazeRadius = (int)CrossHair.Width / 2;
            points = new Dictionary<TouchDevice, Brush>();
            hitTestResults = new List<FrameworkElement>();
            Loaded += new RoutedEventHandler(GazeTouchViewer_Loaded);

            
            bConnect.TouchUp += (sender, e) => { tracker.Connect(); };
            bStart.TouchUp += (sender, e) => { gazeOnly = true; tracker.StartTracking();  };
            bTouch.TouchUp += (sender, e) =>
            {
                gazeOnly = false;
                tracker.StartTracking();
                TouchDown += new EventHandler<TouchEventArgs>(GazeTouchViewer_TouchDown);
                TouchMove += new EventHandler<TouchEventArgs>(GazeTouchViewer_TouchMove);
                LostTouchCapture += new EventHandler<TouchEventArgs>(GazeTouchViewer_LostTouchCapture);
            };
        }

        void GazeTouchViewer_LostTouchCapture(object sender, TouchEventArgs e)
        {
            points.Remove(e.TouchDevice);
            Point p = e.GetTouchPoint(this).Position;
            TrackerEvent.Touch.Log(sessionId, p.X, p.Y, e.TouchDevice.Id, "LostTouch");
        }

        void GazeTouchViewer_TouchDown(object sender, TouchEventArgs e)
        {
            if (points.ContainsKey(e.TouchDevice))
            {
                touchBrush = Brushes.Black;
                return;
            }

            if (points.Count == 0)
                points.Add(e.TouchDevice, Brushes.Blue);
            else
                points.Add(e.TouchDevice, Brushes.Purple);

            Point p = e.GetTouchPoint(this).Position;
            TrackerEvent.Touch.Log(sessionId, p.X, p.Y, e.TouchDevice.Id, "TouchDown");

        }

        void GazeTouchViewer_TouchMove(object sender, TouchEventArgs e)
        {
            if (!points.ContainsKey(e.TouchDevice))
                touchBrush = Brushes.Black;
            else
                touchBrush = points[e.TouchDevice];

            // Clear the contents of the list used for hit test results.
            List<HitTestResult> hitResultsList = new List<HitTestResult>();

            // Set up a callback to receive the hit test result enumeration.
            VisualTreeHelper.HitTest(Canvas, null,
                new HitTestResultCallback(PerformHitTest),
                new PointHitTestParameters(e.GetTouchPoint(Canvas).Position));

            if (hitResultsList.Count > 0)
                return;

            Ellipse gazePoint = new Ellipse()
            {
                Width = gazeRadius,
                Height = gazeRadius,
                Fill = touchBrush,
                Stroke = eyeBrush,
                StrokeThickness = 2

            };
            Point p = e.GetTouchPoint(this).Position;
            gazePoint.RenderTransform = new TranslateTransform(p.X - gazeRadius/2, p.Y - gazeRadius/2);
            Canvas.Children.Add(gazePoint);

            TrackerEvent.Touch.Log(sessionId, p.X, p.Y, e.TouchDevice.Id, "TouchMove");
        }

        // Return the result of the hit test to the callback.
        public HitTestResultBehavior PerformHitTest(HitTestResult result)
        {
            FrameworkElement visual = (FrameworkElement)result.VisualHit;
            // Add the hit test result to the list that will be processed after the enumeration.
            if (visual.IsHitTestVisible)
                hitTestResults.Add(visual);
            else
                return HitTestResultBehavior.Stop;
            // Set the behavior to return visuals at all z-order levels.
            return HitTestResultBehavior.Continue;
        }

        void GazeTouchViewer_Loaded(object sender, RoutedEventArgs e)
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

            bool leftValid = e.LeftValid == 0;
            bool rightValid = e.RightValid == 0;
            if (leftValid && rightValid)
                eyeBrush = Brushes.Green;
            else if (leftValid && !rightValid)
                eyeBrush = Brushes.Yellow;
            else if (rightValid && !leftValid)
                eyeBrush = Brushes.Orange;
            else
            { 
                eyeBrush = Brushes.Red;
                //return; 
            }

            if (!gazeOnly)
                return;

            HitTestResult result = VisualTreeHelper.HitTest(Canvas, new Point(e.GazePoint.X, e.GazePoint.Y));

            if (result != null)
                return;

            Ellipse gazePoint = new Ellipse()
            {
                Width = gazeRadius,
                Height = gazeRadius,
                Fill = eyeBrush
            };
            lastGazePoint = new Point(e.GazePoint.X, e.GazePoint.Y);
            TrackerEvent.Gaze.Log(sessionId,e.GazePoint.X, e.GazePoint.Y, e.LeftValid, e.RightValid);
            gazePoint.RenderTransform = new TranslateTransform(transform.X, transform.Y);
            Canvas.Children.Add(gazePoint);
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

        #region Surface handlers
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
        #endregion
    }
}