using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
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
        private Stopwatch stopwatch;
        private TextBlock tComplete;
        
        const double radiusSize = 4 * Dot.DefaultRadius;
        Point prevEyeLocation;
        int gazeRadius;
        int index;
        Marker endPoint;
        TrackerWrapper tracker;
        bool gazeOn;

        private Point leftDot = new Point(256, 640);
        private Point middleDot = new Point(960,448);
        private Point rightDot = new Point(1664, 640);

        private readonly List<int[]> conditions = new List<int[]>
                                           {
                                                new[] {320, 0, 0, 1 },
                                                  new[] {320, 1, 1, 1 },
                                                  new[] {320, 2, 0, 1 },
                                                  new[] {320, 3, 0, 1 },

                                               // minDistance, cpRotation, dotsOnOff
                                               new[] {128, 0, 0, 0 },
                                               new[] {128, 1, 0, 0 },
                                               new[] {128, 2, 0, 0 },
                                               new[] {128, 3, 0, 0 },
                                               new[] {128, 4, 0, 0  },
                                               new[] {128, 5, 0, 0 },
                                               new[] {128, 0, 1, 0 },
                                               new[] {128, 1, 1, 0 },
                                               new[] {128, 2, 1, 0 },
                                               new[] {128, 3, 1, 0 },
                                               new[] {128, 4, 1, 0},
                                               new[] {128, 5, 1, 0},
                                               new[] {256, 0, 0, 0},
                                               new[] {256, 1, 0, 0},
                                               new[] {256, 2, 0, 0},
                                               new[] {256, 3, 0, 0},
                                               new[] {256, 4, 0, 0},
                                               new[] {256, 5, 0, 0},
                                               new[] {256, 0, 1, 0},
                                               new[] {256, 1, 1, 0},
                                               new[] {256, 2, 1, 0},
                                               new[] {256, 3, 1, 0},
                                               new[] {256, 4, 1, 0},
                                               new[] {256, 5, 1, 0},
                                           };

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
            if (index == conditions.Count)
                return;

            foreach (IDot dot in userDots)
                if (Canvas.Children.Contains((UIElement)dot))
                    Canvas.Children.Remove((UIElement)dot);
            userDots.Clear();

            foreach (IDot dot in refDots)
                if (Canvas.Children.Contains((UIElement)dot))
                    Canvas.Children.Remove((UIElement)dot);
            refDots.Clear();

            if (this.endPoint != null)
                Canvas.Children.Remove(this.endPoint);
            int[] condition = conditions[index];
            int radius = condition[0];

            AssignDots(condition[1], radius);

            ShowUserDots();

            if (condition[2] == 0)
                ShowRefDots();

            gazeOn = condition[3] == 0 ? true : false;

            TrackerEvent.BezierSessionStart.Log(index, condition[0], condition[1], condition[2]);
            TrackerEvent.BezierPoint.Log("RefStart", RefCurve.StartPoint.X, RefCurve.StartPoint.Y);
            TrackerEvent.BezierPoint.Log("RefCP1", RefCurve.ControlPoint1.X, RefCurve.ControlPoint1.Y);
            TrackerEvent.BezierPoint.Log("RefCP2", RefCurve.ControlPoint2.X, RefCurve.ControlPoint2.Y);
            TrackerEvent.BezierPoint.Log("RefEnd", RefCurve.EndPoint.X, RefCurve.EndPoint.Y);
            TrackerEvent.BezierPoint.Log("UserStart", UserCurve.StartPoint.X, UserCurve.StartPoint.Y);
            TrackerEvent.BezierPoint.Log("UserCP1", UserCurve.ControlPoint1.X, UserCurve.ControlPoint1.Y);
            TrackerEvent.BezierPoint.Log("UserCP2", UserCurve.ControlPoint2.X, UserCurve.ControlPoint2.Y);
            TrackerEvent.BezierPoint.Log("UserEnd", UserCurve.EndPoint.X, UserCurve.EndPoint.Y);
            TrackerEvent.BezierSessionStart.Write("GazeOn: " + gazeOn.ToString() + "\n");
        }


        void AssignDots(int condition, float radius)
        {
            Point cp1= new Point(), cp2= new Point(), cp3= new Point();
            bool cp1DoTest = false, cp2DoTest = false, cp3DoTest = false;
            switch (condition)
            {
                case 0:
                    cp1 = leftDot;
                    cp2 = middleDot;
                    cp3 = rightDot;
                    cp2DoTest = true;
                    break;
                case 1:
                    cp1 = leftDot;
                    cp2 = rightDot;
                    cp3 = middleDot;
                    cp3DoTest = true;
                    break;
                case 2:
                    cp1 = middleDot;
                    cp2 = leftDot;
                    cp3 = rightDot;
                    cp1DoTest = true;
                    break;
                case 3:
                    cp1 = middleDot;
                    cp1DoTest = true;
                    cp2 = rightDot;
                    cp3 = leftDot;
                    break;
                case 4:
                    cp1 = rightDot;
                    cp2 = middleDot;
                    cp3 = leftDot;
                    cp2DoTest = true;
                    break;
                case 5:
                    cp1 = rightDot;
                    cp2 = leftDot;
                    cp3 = middleDot;
                    cp3DoTest = true;
                    break;
            }
            RefCurve.StartPoint = GeoHelper.ChooseRandomPointWithinBounds(1920, 952); ;
            RefCurve.EndPoint = GeoHelper.ChooseRandomPointInsideCircle(cp3, radius, cp3DoTest);
            RefCurve.ControlPoint1 = GeoHelper.ChooseRandomPointInsideCircle(cp1, radius, cp1DoTest);
            RefCurve.ControlPoint2 = GeoHelper.ChooseRandomPointInsideCircle(cp2, radius, cp2DoTest);

            UserCurve.StartPoint = RefCurve.StartPoint;
            UserCurve.EndPoint = GeoHelper.ChooseRandomPointOnCircle(RefCurve.EndPoint, radius, cp3DoTest);
            UserCurve.ControlPoint1 = GeoHelper.ChooseRandomPointOnCircle(RefCurve.ControlPoint1, radius, cp1DoTest);
            UserCurve.ControlPoint2 = GeoHelper.ChooseRandomPointOnCircle(RefCurve.ControlPoint2, radius, cp2DoTest);
        }
        

        void Init()
        {
            stopwatch = new Stopwatch();
            tComplete = new TextBlock
            {
                FontSize = 128,
                Foreground = Brushes.Black,
                Text = "Session complete!",
                RenderTransform = new TranslateTransform() { X = 500, Y = 300 }
            };
            gazeRadius =(int)( CrossHair.Width / 2);
            
            knotPoints = new Dictionary<TouchDevice, IDot>();
            userDots = new List<IDot>();
            refDots = new List<IDot>();
#if TRACKER
            Loaded += new RoutedEventHandler(SplineTask_Loaded);
#endif
            TouchDown += ellipse_TouchDown;
            TouchMove += ellipse_TouchMove;
            LostTouchCapture += ellipse_LostTouchCapture;

            //bConnect.TouchUp += (sender, e) => tracker.Connect();
            //bStart.TouchUp += (sender, e) => tracker.StartTracking();
            bDots.Click += (sender, e) => ShowRefDots();
            bNew.Click += delegate
            {
                CountDownWpf countdownTimer = new CountDownWpf();
                countdownTimer.Elapsed += delegate
                {
                    Dispatcher.BeginInvoke(new Action(delegate
                                                      {
#if TRACKER
                                                          if (gazeOn)
                                                            {
                                                                tracker.Connect();
                                                                tracker.StartTracking();
                                                          }
#endif
                        ToggleButtons();
                        NewSession();
                        Canvas.Children.Remove(
                            countdownTimer);
                        countdownTimer.Reset();
                        stopwatch.Start();
                    }
                                               ));

                };
                Canvas.Children.Add(countdownTimer);
                countdownTimer.Start();
                if (Canvas.Children.Contains(tComplete))
                    Canvas.Children.Remove(tComplete);

            };

            //test
            bStop.TouchUp += delegate
                           {
                               if (stopwatch.IsRunning)
                                   CompleteSession();
                           };

        }

        static float ComputeDistance(Point p1, Point p2)
        {
            Vector d = Point.Subtract(p1, p2);
            return (float)Math.Abs(d.Length);
        }

        void CompleteSession()
        {
            stopwatch.Stop();
            ToggleButtons();
            TrackerEvent.BezierDistance.Log("Start", ComputeDistance(RefCurve.StartPoint, UserCurve.StartPoint));
            TrackerEvent.BezierDistance.Log("CP1", ComputeDistance(RefCurve.ControlPoint1, UserCurve.ControlPoint1));
            TrackerEvent.BezierDistance.Log("CP2", ComputeDistance(RefCurve.ControlPoint2, UserCurve.ControlPoint2));
            TrackerEvent.BezierDistance.Log("End", ComputeDistance(RefCurve.EndPoint, UserCurve.EndPoint));
            TrackerEvent.BezierSessionEnd.Log(index, stopwatch.ElapsedMilliseconds / 1000d);
            stopwatch.Reset();
            if (gazeOn)
                tracker.StopTracking();
            Canvas.Children.Add(tComplete);
            index++;
        }

        void ToggleButtons()
        {
            bNew.Visibility = bNew.IsVisible ? Visibility.Hidden : Visibility.Visible;
            bDots.Visibility = bDots.IsVisible ? Visibility.Hidden : Visibility.Visible;
        }

        private void ShowUserDots()
        {
            Dot cp1 = BuildControlPoint(UserCurve.ControlPoint1, 1, Brushes.Blue);
            Dot cp2 = BuildControlPoint(UserCurve.ControlPoint2, 2, Brushes.Red);
            Marker cp3 = BuildEndPoint(UserCurve.EndPoint, Brushes.Yellow);
            Canvas.Children.Add(cp1);
            Canvas.Children.Add(cp2);
            Canvas.Children.Add(cp3);

            endPoint = BuildEndPoint(UserCurve.StartPoint, Brushes.Black);
            userDots.AddRange(new List<IDot> { cp1, cp2, cp3 });

            Canvas.Children.Add(endPoint);
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
            {
                TrackerEvent.Gaze.Log(index, e.GazePoint.X, e.GazePoint.Y, e.LeftValid, e.RightValid, "GazeNotEngaged");
                return;
            }
            else
                TrackerEvent.Gaze.Log(index, e.GazePoint.X, e.GazePoint.Y, e.LeftValid, e.RightValid, "GazeEngaged");

            int eyeIndex = GetEyeIndex(knotPoints.Values);

            Point newLocation = new Point(e.GazePoint.X, e.GazePoint.Y);
            prevEyeLocation = newLocation;

            LogEvent.Engine.Write(string.Format("GP({0:f2},{1:f2}", e.GazePoint.X, e.GazePoint.Y));
            userDots[eyeIndex - 1].Center = newLocation;
            UserCurve.SetPoint(newLocation, eyeIndex);
        }

        static int GetEyeIndex(IEnumerable<IDot> dots)
        {
            List<int> indices = new List<int>() { 1, 2, 3 };
            foreach (IDot d in dots)
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
            Point location = e.GetTouchPoint(this).Position;
            knotPoints.Remove(e.TouchDevice);
            TrackerEvent.Touch.Log(index, location.X, location.Y, e.TouchDevice.Id, "TouchUp");
        }

        void ellipse_TouchMove(object sender, TouchEventArgs e)
        {
            Point location = e.GetTouchPoint(this).Position;
            TrackerEvent.Touch.Log(index, location.X, location.Y, e.TouchDevice.Id, "TouchMove");
            if (!knotPoints.ContainsKey(e.TouchDevice))
            {
                return;
            }

            if (knotPoints.Count < 2) return;

            IDot dot = knotPoints[e.TouchDevice];
            Point newLocation = e.GetTouchPoint(this).Position;
            dot.Center = newLocation;

            UserCurve.SetPoint(newLocation, (int)dot.Tag);
            //Status.Text = string.Format("TouchMove P[{0},{1}]", newLocation.X, newLocation.Y);
        }


        void ellipse_TouchDown(object sender, TouchEventArgs e)
        {
            TouchDevice touchDevice = e.TouchDevice;
            Point location = e.GetTouchPoint(this).Position;
            IDot dot = FindKnotPoint(location);
            TrackerEvent.Touch.Log(index, location.X, location.Y, e.TouchDevice.Id, "TouchDown");

            if (dot == null)
                return;
            Canvas.CaptureTouch(touchDevice);
            knotPoints.Add(touchDevice, dot);
        }

        IDot FindKnotPoint(Point location)
        {
            foreach (IDot d in userDots)
            {
                Vector2D ellipseCenter = new Vector2D(d.Center.X, d.Center.Y);
                Circle c = new Circle(ellipseCenter, radiusSize);
                bool test = Intersection.CirclePointTest(c, new Vector2D(location.X, location.Y));
                if (test)
                    return d;
            }
            return null;
        }

        static Marker BuildEndPoint(Point location, Brush color)
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

            foreach (TraceListener tl in Trace.Listeners)
                tl.Dispose();

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
#if TRACKER
            tracker.DisconnectTracker();
#endif
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