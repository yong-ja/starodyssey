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
    /// Interaction logic for SplineWindow.xaml
    /// </summary>
    public partial class BezierWindow : SurfaceWindow
    {
        Dictionary<TouchDevice, Dot> knotPoints;
        List<Dot> dots;
        Point startPoint, endPoint;
        const int radiusSize = 4 * Dot.Radius;
        Point prevEyeLocation;
        int gazeRadius;

        TrackerWrapper tracker;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public BezierWindow()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
            Init();
        }

        void Init()
        {
            gazeRadius = (int)CrossHair.Width / 2;
            
            
            knotPoints = new Dictionary<TouchDevice, Dot>();
            dots = new List<Dot>();
            InitializeComponent();
            startPoint = UserCurve.StartPoint;
            Canvas.Children.Add(BuildEndPoint(UserCurve.StartPoint));
            Dot cp1=  BuildControlPoint(UserCurve.ControlPoint1, 1);
            Dot cp2 = BuildControlPoint(UserCurve.ControlPoint2, 2);
            Dot cp3 = BuildControlPoint(UserCurve.EndPoint, 3);
            Canvas.Children.Add(cp1);
            Canvas.Children.Add(cp2);
            Canvas.Children.Add(cp3);

            dots.AddRange(new List<Dot>{cp1, cp2, cp3});

            endPoint = cp3.Center;
            Loaded += new RoutedEventHandler(SplineTask_Loaded);
            TouchDown += new EventHandler<TouchEventArgs>(ellipse_TouchDown);
            TouchMove += new EventHandler<TouchEventArgs>(ellipse_TouchMove);
            LostTouchCapture += new EventHandler<TouchEventArgs>(ellipse_LostTouchCapture);

            bConnect.TouchUp += (sender, e) => {tracker.Connect();};
            bStart.TouchUp += (sender, e) => { tracker.StartTracking(); };
            

            Ellipse ellipseRadius = new System.Windows.Shapes.Ellipse()
            {
                Width = 2 * radiusSize,
                Height = 2 * radiusSize,
                Stroke = Brushes.Black,
                RenderTransform = new TranslateTransform(8, 1070)
            };
            Canvas.Children.Add(ellipseRadius);
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

            if (knotPoints.Count < 2)
                return;

            int eyeIndex = GetEyeIndex(knotPoints.Values);

            Point newLocation = new Point(e.GazePoint.X, e.GazePoint.Y);
            prevEyeLocation = newLocation;

            Vector delta = Point.Subtract(newLocation, prevEyeLocation);
            if (delta.LengthSquared < 16 * 16)
            {
                LogEvent.Engine.Write(string.Format("Rejected GP({0:f2},{1:f2} -> Too close", e.GazePoint.X, e.GazePoint.Y));
                return;
            }
            LogEvent.Engine.Write(string.Format("GP({0:f2},{1:f2}", e.GazePoint.X, e.GazePoint.Y));
            dots[eyeIndex - 1].Center = newLocation;

            //UserCurve.Points[eyeIndex] = newLocation;

        }

        static int GetEyeIndex(IEnumerable<Dot> dots)
        {
            List<int> indices = new List<int>() { 1, 2, 3 };
            foreach (Dot d in dots)
            {
                int index = (int)d.Tag;
                if (indices.Contains(index))
                    indices.Remove(index);
            }
            if (indices.Count != 1)
            {
                LogEvent.Engine.Write("Error: indices exclusion failed");
                return -1;
            }
            return indices[0];
        }

        void SplineTask_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            tracker = new TrackerWrapper();
            tracker.SetWindow(this);
            tracker.StartBrowsing();
            tracker.GazeDataReceived += new EventHandler<GazeEventArgs>(tracker_GazeDataReceived);
            WpfTextTraceListener.SetTextOutput(Status);
        }



        void ellipse_LostTouchCapture(object sender, TouchEventArgs e)
        {
            knotPoints.Remove(e.TouchDevice);
            LogEvent.Engine.Write(string.Format("TouchDown [{0}]", e.TouchDevice.Id));
        }

        void ellipse_TouchMove(object sender, TouchEventArgs e)
        {
            if (!knotPoints.ContainsKey(e.TouchDevice))
            {
                LogEvent.Engine.Write("TouchMove - No intersection");
                return;
            }
            Dot dot = knotPoints[e.TouchDevice];
            Point newLocation = e.GetTouchPoint(this).Position;
            Point newCenter = new Point(newLocation.X - Dot.Radius, newLocation.Y - Dot.Radius);
            dot.Center = newCenter;

            UserCurve.SetPoint(newLocation, (int)dot.Tag);
            Status.Text = string.Format("TouchMove P[{0},{1}]", newLocation.X, newLocation.Y);
        }


        //void SynchronizeDotsWithPoints()
        //{
        //    PointCollection points = UserCurve.Points;
        //    UserCurve.Points = null;
        //    points.Clear();
        //    points.Add(startPoint);
        //    foreach (UIElement child in Canvas.Children)
        //    {
        //        Dot dot = child as Dot;
        //        if (dot != null)
        //            points.Add(dot.Center);
        //    }
        //    points.Add(endPoint);

        //    UserCurve.Points = points;
        //}


        void ellipse_TouchDown(object sender, TouchEventArgs e)
        {
            TouchDevice touchDevice = e.TouchDevice;
            Point location = e.GetTouchPoint(this).Position;
            Dot dot = FindKnotPoint(location);
            LogEvent.Engine.Write(string.Format("TouchDown [{0}] {1}", e.TouchDevice.Id,
                dot == null ? "No intersection" : "Found dot #" + (int)dot.Tag));

            if (dot == null)
                return;
            Canvas.CaptureTouch(touchDevice);
            //touchDevice.Capture(Canvas);
            knotPoints.Add(touchDevice, dot);
        }

        Dot FindKnotPoint(Point location)
        {
            foreach (Dot d in dots)
            {
                AvengersUtd.Odyssey.Geometry.Vector2D ellipseCenter = new AvengersUtd.Odyssey.Geometry.Vector2D(d.Center.X, d.Center.Y);
                AvengersUtd.Odyssey.Geometry.Circle c = new AvengersUtd.Odyssey.Geometry.Circle(ellipseCenter, radiusSize);
                bool test = AvengersUtd.Odyssey.Geometry.Intersection.CirclePointTest(c, new AvengersUtd.Odyssey.Geometry.Vector2D(location.X, location.Y));
                if (test)
                    return d;
            }
            return null;
        }

        Rectangle BuildEndPoint(Point location)
        {
            const int width = 16;
            Rectangle rectangle = new Rectangle()
            {
                RenderTransform = new TranslateTransform(location.X - width / 2, location.Y - width / 2),
                Fill = Brushes.Black,
                Width = width,
                Height = width
            };
            return rectangle;
        }

        Dot BuildControlPoint(Point location, int index)
        {
            Dot knotPoint = new Dot() { Center = location };
            knotPoint.Tag = index;
            return knotPoint;
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

        #region Surface Window Logic

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