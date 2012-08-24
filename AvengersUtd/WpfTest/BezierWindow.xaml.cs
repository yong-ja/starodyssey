using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Surface;
using Microsoft.Surface.Presentation.Controls;
using AvengersUtd.Odyssey.Utils.Logging;
using AvengersUtd.Odyssey.Geometry;
using System.Timers;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for SplineWindow.xaml
    /// </summary>
    public partial class BezierWindow : SurfaceWindow
    {
        private Timer completionTimer;
        private DateTime prevTime;
        private const double Threshold = 32;
        Dictionary<TouchDevice, IDot> knotPoints;
        List<IDot> userDots;
        List<IDot> refDots;
        private Stopwatch stopwatch;
        private TextBlock tComplete;
        
        const double RadiusSize = 4 * Dot.DefaultRadius;
        int gazeRadius;
        //int index=20;
        int eyeIndex;
        Marker endPoint;
        TrackerWrapper tracker;
        bool gazeOn;

        private DateTime startTime;

        private readonly Point leftDot = new Point(256, 640);
        private readonly Point middleDot = new Point(960,640);
        private readonly Point rightDot = new Point(1664, 640);

        private readonly List<int[]> cpConditions = new List<int[]>
        {
            new[] { 0, 1, 2},
            new[] { 0, 2, 1},
            new[] { 2, 0, 1}
            //new[] { 0, 1,2},
            //new[] {0 , 2,1},
            //new[] {1, 0, 2},
            //new[] {1, 2, 0},
            //new[] { 2, 0, 1},
            //new[] {2, 1, 0}
        };

        private List<Point[]> userCurves;
        private List<Point[]> refCurves;

        readonly bool[] gazeActivated = new[] { true, false};

        private readonly List<int[]> conditions = new List<int[]>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BezierWindow()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
            InitConditions();
            Init();
            //NewSession();
            
        }

        void InitConditions()
        {
            // Curve 1 short
            Point c0p0 = CirclePoint(leftDot, 96, 315);
            Point c0p1 = CirclePoint(middleDot, 96, 225);
            Point c0p2 = CirclePoint(rightDot, 96, 315);

            //Curve 2 short
            Point c1p0 = CirclePoint(leftDot, 96, 45);
            Point c1p1 = CirclePoint(middleDot, 96, 225);
            Point c1p2 = CirclePoint(rightDot, 96, 135);

            //Curve 3 Long
            Point c2p0 = CirclePoint(leftDot, 224, 135);
            Point c2p1 = CirclePoint(middleDot, 224, 315);
            Point c2p2 = CirclePoint(rightDot, 224, 225);

            //Curve 4 Long
            Point c3p0 = CirclePoint(leftDot, 224, 225);
            Point c3p1 = CirclePoint(middleDot, 224, 45);
            Point c3p2 = CirclePoint(rightDot, 224, 315);

            userCurves = new List<Point[]>
            {
                new Point[] { c0p0, c0p1, c0p2},
                new Point[] { c1p0, c1p1, c1p2},
                new Point[] { c2p0, c2p1, c2p2},
                new Point[] { c3p0, c3p1, c3p2}
            };

            // Curve 1 short
            Point rC0p0 = CirclePoint(leftDot, 128, 90);
            Point rC0p1 = CirclePoint(middleDot, 128, 90);
            Point rC0p2 = CirclePoint(rightDot, 128, 90);

            //Curve 2 short
            Point rC1p0 = CirclePoint(leftDot, 128, 180);
            Point rC1p1 = CirclePoint(middleDot, 128, 90);
            Point rC1p2 = CirclePoint(rightDot, 128, 270);

            //Curve 3 Long
            Point rC2p0 = CirclePoint(leftDot, 256, 270);
            Point rC2p1 = CirclePoint(middleDot, 256, 180);
            Point rC2p2 = CirclePoint(rightDot, 256, 90);

            //Curve 4 Long
            Point rC3p0 = CirclePoint(leftDot, 256, 90);
            Point rC3p1 = CirclePoint(middleDot, 256, 270);
            Point rC3p2 = CirclePoint(rightDot, 256, 90);

            refCurves = new List<Point[]>
            {
                new Point[] { rC0p0, rC0p1, rC0p2},
                //new Point[] { rC1p0, rC1p1, rC1p2},
                //new Point[] { rC2p0, rC2p1, rC2p2},
                new Point[] { rC3p0, rC3p1, rC3p2}
            };

                for (int t = 0; t < 2; t++) // Gaze On/off
                            for (int j = 0; j < 3 ; j++) // EP rotation
                                for (int k = 0; k < 4; k++) // Show Ref Points - Repetitions 4
                                    for (int i = 0; i < 2; i++) // Curves
                                    
                        // curve, epr, ref, gaze
            conditions.Add(new int[] {i, j, k, t });
            //Participant, Curve, EPR, Ref, GazeOn, Time
            TrackerEvent.BezierSessionStart.Log("Participant", "Curve", "EPR", "Rep", "GazeOn", "Time", "CvEr");

        }

        static Point CirclePoint(Point center, float radius, float angle)
        {
            double x = center.X + radius * Math.Cos(MathHelper.DegreesToRadians(angle));
            double y = center.Y + radius * Math.Sin(MathHelper.DegreesToRadians(angle));

            return new Point(x,y);
        }


        void NewSession()
        {
            if (Test.Count == conditions.Count)
                return;

            foreach (IDot dot in userDots.Where(dot => Canvas.Children.Contains((UIElement)dot)))
                Canvas.Children.Remove((UIElement)dot);
            userDots.Clear();

            foreach (IDot dot in refDots.Where(dot => Canvas.Children.Contains((UIElement)dot)))
                Canvas.Children.Remove((UIElement)dot);
            refDots.Clear();

            if (endPoint != null)
                Canvas.Children.Remove(endPoint);

            if (Canvas.Children.Contains(tComplete))
                Canvas.Children.Remove(tComplete);

            //
            //lCondition.Visibility = Visibility.Hidden;

            int[] condition = conditions[Test.BezierIndex % conditions.Count];

            Point[] userPoints = userCurves[condition[0]];
            Point[] refPoints = refCurves[condition[0]];
            int[] cpIndex = cpConditions[condition[1]];

            UserCurve.ControlPoint1 = userPoints[cpIndex[0]];
            UserCurve.ControlPoint2 = userPoints[cpIndex[1]];
            UserCurve.EndPoint = userPoints[cpIndex[2]];

            RefCurve.ControlPoint1 = refPoints[cpIndex[0]];
            RefCurve.ControlPoint2 = refPoints[cpIndex[1]];
            RefCurve.EndPoint = refPoints[cpIndex[2]];

            if (cpIndex[0] == 1)
                eyeIndex = 1;
            else if (cpIndex[1] == 1)
                eyeIndex = 2;
            else if (cpIndex[2] == 1)
                eyeIndex = 3;

            gazeOn = gazeActivated[condition[3]];
            

            //TrackerEvent.BezierSessionStart.Log(index, condition[0], condition[1], condition[2], condition[3]);
            //TrackerEvent.BezierPoint.Log("RefStart", RefCurve.StartPoint.X, RefCurve.StartPoint.Y);
            //TrackerEvent.BezierPoint.Log("RefCP1", RefCurve.ControlPoint1.X, RefCurve.ControlPoint1.Y);
            //TrackerEvent.BezierPoint.Log("RefCP2", RefCurve.ControlPoint2.X, RefCurve.ControlPoint2.Y);
            //TrackerEvent.BezierPoint.Log("RefEnd", RefCurve.EndPoint.X, RefCurve.EndPoint.Y);
            //TrackerEvent.BezierPoint.Log("UserStart", UserCurve.StartPoint.X, UserCurve.StartPoint.Y);
            //TrackerEvent.BezierPoint.Log("UserCP1", UserCurve.ControlPoint1.X, UserCurve.ControlPoint1.Y);
            //TrackerEvent.BezierPoint.Log("UserCP2", UserCurve.ControlPoint2.X, UserCurve.ControlPoint2.Y);
            //TrackerEvent.BezierPoint.Log("UserEnd", UserCurve.EndPoint.X, UserCurve.EndPoint.Y);

            prevTime = default(DateTime);
            completionTimer.Start();
            startTime = DateTime.Now;
        }


        void Init()
        {
            Bezier.Dispatcher = Dispatcher;

            completionTimer = new Timer(250);
            completionTimer.Elapsed += completionTimer_Elapsed;
            UserCurve.Visibility = Visibility.Hidden;
            RefCurve.Visibility = Visibility.Hidden;
            
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
            Loaded += SplineTask_Loaded;
#endif
            TouchDown += ellipse_TouchDown;
            TouchMove += ellipse_TouchMove;
            LostTouchCapture += ellipse_LostTouchCapture;

            //bConnect.TouchUp += (sender, e) => tracker.Connect();
            //bStart.TouchUp += (sender, e) => tracker.StartTracking();
            bDots.Click += (sender, e) => ShowRefDots();
            bNew.Click += delegate
            {
                int[] condition = conditions[Test.BezierIndex % conditions.Count];
                NewSession();
                lCondition.Visibility = Visibility.Visible;
                lCondition.Text = gazeOn ? "Manual + Gaze condition" : "Manual only";
                lCondition.Text += "\nTrial: " + (Test.Count + 1);
                
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
                                                          CrossHair.Visibility = gazeOn ? Visibility.Visible : Visibility.Hidden;
#endif
                                                          UserCurve.Visibility = Visibility.Visible;
                                                          RefCurve.Visibility = Visibility.Visible;
                                                          ShowUserDots();
                                                          
                                                          
                                                          //if (condition[2] == 0)
                                                          //    ShowRefDots();
                                                          lCondition.Visibility = Visibility.Hidden;
                                                          ToggleButtons();
                        
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
                               TrackerEvent.Misc.Write(string.Format("BezierSession {0} aborted", Test.BezierIndex));
                               //NewSession();
                           };

        }

        double distance;
        void completionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
             distance = Bezier.FindError(UserCurve.PathGeometry, RefCurve.PathGeometry);
            TrackerEvent.BezierDistance.Log("CvEr", distance);
            bool check = distance < Threshold;

            if (!check)
            {
                prevTime = DateTime.Now;
                return;
            }

            if (prevTime == default(DateTime))
                prevTime = DateTime.Now;
            
            DateTime now = DateTime.Now;
            double delta = (now - prevTime).TotalMilliseconds;

            if (delta <= 333) return;

            TrackerEvent.BezierDistance.Log("CvErFinal", distance);
            Dispatcher.BeginInvoke(new Action( CompleteSession));
        }

        void CompleteSession()
        {
            stopwatch.Stop();
            
            completionTimer.Stop();
            //Dispatcher.BeginInvoke(new Action(delegate
            //{
                ToggleButtons();
                //TrackerEvent.BezierDistance.Log("Start", ComputeDistance(RefCurve.StartPoint, UserCurve.StartPoint));
                //TrackerEvent.BezierDistance.Log("CP1", ComputeDistance(RefCurve.ControlPoint1, UserCurve.ControlPoint1));
                //TrackerEvent.BezierDistance.Log("CP2", ComputeDistance(RefCurve.ControlPoint2, UserCurve.ControlPoint2));
                //TrackerEvent.BezierDistance.Log("End", ComputeDistance(RefCurve.EndPoint, UserCurve.EndPoint));

                    // curve, epr, ref, gaze
                int[] condition = conditions[Test.BezierIndex % conditions.Count ];
                //Participant, Curve, EPR, Ref, GazeOn, Time

                TrackerEvent.BezierSessionEnd.Log(Test.Participant,
                                                  condition[0], condition[1], condition[2], condition[3],
                                                  DateTime.Now - startTime, distance);
                     //stopwatch.ElapsedMilliseconds / 1000d);
                stopwatch.Reset();
            //}));
            
#if TRACKER
            if (gazeOn)
                tracker.StopTracking();
#endif
            UserCurve.Visibility = Visibility.Hidden;
            RefCurve.Visibility = Visibility.Hidden;
            CrossHair.Visibility = Visibility.Hidden;
            Canvas.Children.Add(tComplete);
            lCondition.Visibility = Visibility.Visible;

            Test.Count++;
            if (Test.Count == conditions.Count)
            {
                lCondition.Text = "Thanks, this task is now complete.";
                return;
            }

            if (Test.Count % 12 == 0 && Test.Count > 0)
            {

                //int[] condition = conditions[Test.BezierIndex];
                lCondition.Text = "Please have a break";
            }
            else
                lCondition.Text = string.Empty;

            Test.BezierIndex++;

            foreach (IStopAndGo tl in Trace.Listeners.OfType<IStopAndGo>())
            {
                tl.StopAndGo();
            }

            foreach (ISave tl in Trace.Listeners.OfType<ISave>())
            {
                tl.Save();
            }
            
        }

        void ToggleButtons()
        {
            bNew.Visibility = bNew.IsVisible ? Visibility.Hidden : Visibility.Visible;
            bDots.Visibility = bDots.IsVisible ? Visibility.Hidden : Visibility.Visible;
        }

        private void ShowUserDots()
        {
            Dot cp1 = BuildControlPoint(UserCurve.ControlPoint1, 1, Brushes.Blue, true);
            Dot cp2 = BuildControlPoint(UserCurve.ControlPoint2, 2, Brushes.Red,true);
            Marker cp3 = BuildEndPoint(UserCurve.EndPoint, Brushes.Yellow, true);
            Canvas.Children.Add(cp1);
            Canvas.Children.Add(cp2);
            Canvas.Children.Add(cp3);

            endPoint = BuildEndPoint(UserCurve.StartPoint, Brushes.Black, false);
            userDots.AddRange(new List<IDot> { cp1, cp2, cp3 });

            Canvas.Children.Add(endPoint);
        }

        private void ShowRefDots()
        {
            Dot cp1 = BuildControlPoint(RefCurve.ControlPoint1, 1, Brushes.Cyan, false);
            Dot cp2 = BuildControlPoint(RefCurve.ControlPoint2, 2, Brushes.Purple,false);
            Marker cp3 = BuildEndPoint(RefCurve.EndPoint, Brushes.Orange, false);
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
                TrackerEvent.Gaze.Log(Test.BezierIndex, e.GazePoint.X, e.GazePoint.Y, e.LeftValid, e.RightValid, "GazeNotEngaged");
                return;
            }
            else
                TrackerEvent.Gaze.Log(Test.BezierIndex, e.GazePoint.X, e.GazePoint.Y, e.LeftValid, e.RightValid, "GazeEngaged");

            //int eyeIndex = GetEyeIndex(knotPoints.Values);
            //int[] condition = conditions[Test.BezierIndex % conditions.Count];
            //int cpIndex = condition[1];
            //int[] cpCondition = cpConditions[cpIndex];
            //int eyeIndex = cpCondition.ToList().IndexOf(1);


            Point newLocation = new Point(e.GazePoint.X, e.GazePoint.Y);

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
            tracker.GazeDataReceived += tracker_GazeDataReceived;

        }

        void ellipse_LostTouchCapture(object sender, TouchEventArgs e)
        {
            Point location = e.GetTouchPoint(this).Position;
            knotPoints.Remove(e.TouchDevice);
            TrackerEvent.Touch.Log(Test.BezierIndex % conditions.Count, location.X, location.Y, e.TouchDevice.Id, "TouchUp");
        }

        void ellipse_TouchMove(object sender, TouchEventArgs e)
        {
            Point location = e.GetTouchPoint(this).Position;
            TrackerEvent.Touch.Log(Test.BezierIndex % conditions.Count, location.X, location.Y, e.TouchDevice.Id, "TouchMove");
            if (!knotPoints.ContainsKey(e.TouchDevice))
            {
                return;
            }

            if (gazeOn && knotPoints.Count < 2) return;

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
            TrackerEvent.Touch.Log(Test.BezierIndex % conditions.Count, location.X, location.Y, e.TouchDevice.Id, "TouchDown");

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
                Circle c = new Circle(ellipseCenter, RadiusSize);
                bool test = Intersection.CirclePointTest(c, new Vector2D(location.X, location.Y));
                if (test)
                    return d;
            }
            return null;
        }

        static Marker BuildEndPoint(Point location, Brush color, bool isUser)
        {
             int width = isUser ? 32: 16;
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

        Dot BuildControlPoint(Point location, int index, Brush color, bool isUser)
        {
            Dot knotPoint = new Dot() { Center = location, Fill = color, Radius = isUser? 16 : 8};
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
            {
                tl.Flush();
                tl.Dispose();
            }

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