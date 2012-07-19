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
using AvengersUtd.Odyssey.Geometry;
using Ellipse = AvengersUtd.Odyssey.Geometry.Ellipse;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for SplineWindow.xaml
    /// </summary>
    public partial class BezierWindow : SurfaceWindow
    {
        Dictionary<TouchDevice, IDot> knotPoints;
        List<IDot> userDots;
        List<IDot> refDots;
        Point startPoint, endPoint;
        const int radiusSize = 4 * Dot.Radius;
        Point prevEyeLocation;
        int gazeRadius;
        int session;
        bool gazeUpdate;

        TrackerWrapper tracker;
        Random rand;
        int x, y;
        AvengersUtd.Odyssey.Geometry.Ellipse outerEllipse;
        Circle innerCircle;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BezierWindow()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
            Init();
            NewSession();
            
        }


        void NewSession()
        {
            int radius = 512;
            Point startPoint = ChooseRandomPoint();
            Point endPoint = ChoosePointOnCircle(startPoint, radius);
            Point middlePoint1 = new Point(Math.Abs((startPoint.X + endPoint.X)/2), 
                Math.Abs((startPoint.Y +endPoint.Y)/2));

            RefCurve.StartPoint = startPoint;
            RefCurve.EndPoint = endPoint;
            RefCurve.ControlPoint1 = ChoosePointOnCircle(middlePoint1, radius);
            RefCurve.ControlPoint2 = ChoosePointOnCircle(middlePoint1, radius);

            UserCurve.StartPoint = startPoint;
            UserCurve.EndPoint = ChoosePointOnCircle(startPoint, radius);
            Point middlePoint2 = new Point(Math.Abs((startPoint.X + endPoint.X) / 2),
                Math.Abs((startPoint.Y + endPoint.Y) / 2));
            UserCurve.ControlPoint1 = ChoosePointOnCircle(middlePoint2, radius);
            UserCurve.ControlPoint2 = ChoosePointOnCircle(middlePoint2, radius);

            ShowUserDots();

        }

        Point ChoosePointOnCircle(Point center, int radius)
        {
            bool test = false;
            double x = 0, y = 0;
            while (!test)
            {
                double t = rand.NextDouble() * 2;
                x =center.X+ (radius * Math.Cos(t * Math.PI));
                y =center.Y + (radius * Math.Sin(t * Math.PI));
                Vector2D p = new Vector2D(x, y); 
                bool inOuterCircle = Intersection.EllipsePointTest(outerEllipse, p) && p.Y < 1080;
                bool inInnerCircle = Intersection.CirclePointTest(innerCircle, p) && p.Y < 1080;

                test = inOuterCircle && !inInnerCircle;
            }

            return new Point(x, y);
        }

        Point ChooseRandomPoint()
        {
            Vector2D p = new Vector2D();

            bool test = false;
            int index = 0;
            while (!test)
            {
                x = rand.Next(1920);
                y = rand.Next(1080);

                p = new Vector2D(x, y);

                bool inOuterCircle = Intersection.EllipsePointTest(outerEllipse, p) && p.Y < 1080;
                bool inInnerCircle = Intersection.CirclePointTest(innerCircle, p) && p.Y < 1080;

                test = inOuterCircle && !inInnerCircle;
                index++;

                if (index > 100)
                {
                    LogEvent.Engine.Write("More than 100 attempts!");
                    break;
                }
            }

            return new Point(p.X, p.Y);

        }

        void Init()
        {
            rand = new Random();
            outerEllipse = new Ellipse(new Vector2D(960, 1208), 960, 1080);
            innerCircle = new Circle(new Vector2D(960, 1080), 256);

            bool test = Intersection.CirclePointTest(innerCircle, new Vector2D(960, 820));

            knotPoints = new Dictionary<TouchDevice, IDot>();
            userDots = new List<IDot>();
            refDots = new List<IDot>();
            //Loaded += new RoutedEventHandler(SplineTask_Loaded);
            TouchDown += new EventHandler<TouchEventArgs>(ellipse_TouchDown);
            TouchMove += new EventHandler<TouchEventArgs>(ellipse_TouchMove);
            LostTouchCapture += new EventHandler<TouchEventArgs>(ellipse_LostTouchCapture);

            bConnect.TouchUp += (sender, e) => {tracker.Connect();};
            bStart.TouchUp += (sender, e) => { tracker.StartTracking(); };
            bNew.TouchUp += (sender, e) => { NewSession(); };
            bDots.TouchUp += (sender, e) => { ShowRefDots(); };
            

            //Ellipse ellipseRadius = new System.Windows.Shapes.Ellipse()
            //{
            //    Width = 2 * radiusSize,
            //    Height = 2 * radiusSize,
            //    Stroke = Brushes.Black,
            //    RenderTransform = new TranslateTransform(8, 1070)
            //};
            //Canvas.Children.Add(ellipseRadius);
        }

        private void ShowUserDots()
        {
            Dot cp1 = BuildControlPoint(UserCurve.ControlPoint1, 1, Brushes.Blue);
            Dot cp2 = BuildControlPoint(UserCurve.ControlPoint2, 2, Brushes.Red);
            Marker cp3 = BuildEndPoint(UserCurve.EndPoint, Brushes.Yellow);
            Canvas.Children.Add(cp1);
            Canvas.Children.Add(cp2);
            Canvas.Children.Add(cp3);

            userDots.AddRange(new List<IDot> { cp1, cp2, cp3 });

            Canvas.Children.Add(BuildEndPoint(UserCurve.StartPoint, Brushes.Black));
            endPoint = cp3.Center;
        }

        private void ShowRefDots()
        {
            Dot cp1 = BuildControlPoint(RefCurve.ControlPoint1, 1, Brushes.Cyan);
            Dot cp2 = BuildControlPoint(RefCurve.ControlPoint2, 2, Brushes.Purple);
            Marker cp3 = BuildEndPoint(RefCurve.EndPoint, Brushes.Orange);
            Canvas.Children.Add(cp1);
            Canvas.Children.Add(cp2);
            Canvas.Children.Add(cp3);

            refDots.AddRange(new List<IDot> { cp1, cp2, cp3 });

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

            //Vector delta = Point.Subtract(newLocation, prevEyeLocation);
            //if (delta.LengthSquared < 16 * 16 && gazeUpdate)
            //{
            //    LogEvent.Engine.Write(string.Format("Rejected GP({0:f2},{1:f2} -> Too close", e.GazePoint.X, e.GazePoint.Y));
            //    return;
            //}
            //else gazeUpdate = false;

            //if (!gazeUpdate)
            //{
                LogEvent.Engine.Write(string.Format("GP({0:f2},{1:f2}", e.GazePoint.X, e.GazePoint.Y));
                userDots[eyeIndex - 1].Center = newLocation;
                UserCurve.SetPoint(newLocation, eyeIndex);
                gazeUpdate = true;
            //}

        }

        static int GetEyeIndex(IEnumerable<IDot> dots)
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
            WpfTextTraceListener.SetTextOutput(Status);
            WindowState = WindowState.Maximized;
            tracker = new TrackerWrapper();
            tracker.SetWindow(this);
            tracker.StartBrowsing();
            tracker.GazeDataReceived += new EventHandler<GazeEventArgs>(tracker_GazeDataReceived);
            
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
            IDot dot = knotPoints[e.TouchDevice];
            Point newLocation = e.GetTouchPoint(this).Position;
            Point newCenter = new Point(newLocation.X , newLocation.Y );
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
            IDot dot = FindKnotPoint(location);
            LogEvent.Engine.Write(string.Format("TouchDown [{0}] {1}", e.TouchDevice.Id,
                dot == null ? "No intersection" : "Found dot #" + (int)dot.Tag));

            if (dot == null)
                return;
            Canvas.CaptureTouch(touchDevice);
            //touchDevice.Capture(Canvas);
            knotPoints.Add(touchDevice, dot);
        }

        IDot FindKnotPoint(Point location)
        {
            foreach (IDot d in userDots)
            {
                AvengersUtd.Odyssey.Geometry.Vector2D ellipseCenter = new AvengersUtd.Odyssey.Geometry.Vector2D(d.Center.X, d.Center.Y);
                AvengersUtd.Odyssey.Geometry.Circle c = new AvengersUtd.Odyssey.Geometry.Circle(ellipseCenter, radiusSize);
                bool test = AvengersUtd.Odyssey.Geometry.Intersection.CirclePointTest(c, new AvengersUtd.Odyssey.Geometry.Vector2D(location.X, location.Y));
                if (test)
                    return d;
            }
            return null;
        }

        Marker BuildEndPoint(Point location, Brush color)
        {
            const int width = 16;
            Marker marker = new Marker()
            {
                Center = location,
                RenderTransform = new TranslateTransform(location.X - width / 2, location.Y - width / 2),
                Fill = color,
                Width = width,
                Height = width,
                Tag = 3
            };
            return marker;
        }

        Dot BuildControlPoint(Point location, int index, Brush color)
        {
            Dot knotPoint = new Dot() { Center = location, Fill = color};
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