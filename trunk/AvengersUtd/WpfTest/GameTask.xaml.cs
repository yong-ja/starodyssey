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
using System.Threading;
using Timer = System.Timers.Timer;
using AvengersUtd.Odyssey.Geometry;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for GameTask.xaml
    /// </summary>
    public partial class GameTask : SurfaceWindow
    {
        private readonly Point leftDot = new Point(256, 640);
        private readonly Point middleDot = new Point(960, 448);
        private readonly Point rightDot = new Point(1664, 640);
        int gazeRadius;
        bool gazeOn;
        private Timer clock;
        private Stopwatch stopwatch;
        private TextBlock tComplete;
        Dictionary<TouchDevice, Point> touchPoints;
        List<IDot> targets;
        TrackerWrapper tracker;
        Point lastGaze;

        // Radii 8, 64, 128
        // Distances 256, 512, 768

        private float[] sizes = new[] { 4f, 16f, 32f };
        private float[] distances = new[] { 256f, 512f, 768f };

        private readonly List<int[]> conditions = new List<int[]>();
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GameTask()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
            Init();

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < sizes.Length; j++)
                    for (int k = 0; k < distances.Length; k++)
                        conditions.Add(new int[] { i, j, k });
        }

        private static int index;

        void Init()
        {
            clock = new Timer() {Interval = 100};
            stopwatch = new Stopwatch();
            clock.Elapsed += new ElapsedEventHandler(clock_Elapsed);
            tComplete = new TextBlock
            {
                FontSize = 128,
                Foreground = Brushes.Black,
                Text = "Session complete!",
                RenderTransform = new TranslateTransform() { X = 500, Y = 300 }
            };
            gazeRadius = (int)CrossHair.Width / 2;
           
            touchPoints = new Dictionary<TouchDevice, Point>();
#if TRACKER
            Loaded += new RoutedEventHandler(GameTask_Loaded);
#endif
            Canvas.TouchDown += new EventHandler<TouchEventArgs>(Canvas_TouchDown);
            Canvas.TouchMove += new EventHandler<TouchEventArgs>(Canvas_TouchMove);
            LostTouchCapture += new EventHandler<TouchEventArgs>(Canvas_LostTouchCapture);
            //bConnect.Click += (sender, e) => tracker.Connect();
            //bStart.Click += (sender, e) => tracker.StartTracking();
            bNew.Click += delegate
                          {
                              if (Canvas.Children.Contains(tComplete))
                                  Canvas.Children.Remove(tComplete);
#if TRACKER
                              tracker.Connect();
                              tracker.StartTracking();
#endif
                              eyeArea.Visibility = lFingerArea.Visibility = rFingerArea.Visibility = Visibility.Visible;
                              Thread thread = new Thread(CheckPreconditions);
                              thread.SetApartmentState(ApartmentState.STA);
                              thread.Start();
                          };
            Indicator.MouseUp += (sender, e) => CompleteSession();
            eyeArea.Visibility = lFingerArea.Visibility = rFingerArea.Visibility = Visibility.Hidden;
        }

        void StartNew()
        {
            CountDownWpf countdownTimer = new CountDownWpf();
            countdownTimer.Elapsed += delegate
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    eyeArea.Visibility = lFingerArea.Visibility = rFingerArea.Visibility = Visibility.Hidden;
                    


                    ToggleButtons();
                    NewSession();
                    Canvas.Children.Remove(
                        countdownTimer);
                    countdownTimer.Reset();
                    clock.Start();
                    stopwatch.Start();
                }));
            };
            Canvas.Children.Add(countdownTimer);
            countdownTimer.Start();
            
        }

        void CheckPreconditions()
        {
            bool done = false;
            while (!done)
            {
                if (touchPoints.Count != 2)
                {
                    Thread.Sleep(100);
                    continue;
                }

                Point[] pArray = touchPoints.Values.ToArray();
                Point tp1 = pArray[0];
                Point tp2 = pArray[1];
                Point gaze = lastGaze;

                Vector2D vTp1 = new Vector2D(tp1.X, tp1.Y);
                Vector2D vTp2 = new Vector2D(tp2.X, tp2.Y);
                Vector2D vGaze = new Vector2D(gaze.X, gaze.Y);

                //Dispatcher.BeginInvoke(new Action(delegate
                //{
                bool lFinger = Intersection.CirclePointTest(new Vector2D(leftDot.X, leftDot.Y), 64, vTp1);
                bool rFinger = Intersection.CirclePointTest(new Vector2D(rightDot.X, rightDot.Y), 64, vTp2);
                bool eyes = Intersection.CirclePointTest(new Vector2D(middleDot.X, middleDot.Y), 64, vGaze);

                    //bool lFinger = lFingerArea.IntersectsWith(tp1) || lFingerArea.IntersectsWith(tp2);
                    //bool rFinger = rFingerArea.IntersectsWith(tp1) || rFingerArea.IntersectsWith(tp2);
                    //bool eyes = eyeArea.IntersectsWith(gaze);
                    done = lFinger && rFinger && eyes;

                //}));
            }

            Dispatcher.BeginInvoke(new Action(StartNew));
        }

        void CompleteSession()
        {
            clock.Stop();
            stopwatch.Stop();
            TrackerEvent.PointSessionEnd.Log(index, stopwatch.ElapsedMilliseconds / 1000d);
                        index++;
            
            ToggleButtons();
           
            gazeOn = false;
            tComplete.Text = index % 18 == 0 ? "Session complete!\nPlease have a break." : "Session complete!";
            if (index == 45)
                tComplete.Text = "Thanks! This task is now complete";
            Canvas.Children.Add(tComplete);
            
            stopwatch.Reset();
            tracker.StopTracking();

            if (targets != null && targets.Count > 0)
            {
                foreach (UIElement d in targets)
                    Canvas.Children.Remove(d);
                targets.Clear();
            }
            ClockLabel.Visibility = Visibility.Visible;
            touchPoints.Clear();

        }

        void clock_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate
                                              {
                                                  ClockLabel.Text =
                                                      string.Format("{0:000}.{1}", stopwatch.ElapsedMilliseconds/1000,
                                                                    stopwatch.ElapsedMilliseconds%1000);
                                              }));
        }

        void ToggleButtons()
        {
            //bConnect.Visibility = bConnect.IsVisible ? Visibility.Hidden: Visibility.Visible;
            //bStart.Visibility = bStart.IsVisible ? Visibility.Hidden : Visibility.Visible;
            bNew.Visibility = bNew.IsVisible ? Visibility.Hidden : Visibility.Visible;
        }

        void NewSession()
        {
            if (index == conditions.Count)
                return;

            int rIndex = conditions[index][1];
            int dIndex = conditions[index][2];
            float radius = sizes[rIndex];
            float maxDistance = distances[dIndex];
            Indicator.Fill = Brushes.Red;

            if (targets != null && targets.Count > 0)
            {
                foreach (UIElement d in targets)
                    Canvas.Children.Remove(d);
                targets.Clear();
            }

            Point[] points = GeoHelper.ChooseTrianglePoints(new Point(960, 540), maxDistance, radius);
            Dot target1 = new Dot() { Fill = Brushes.Red, Radius = 32, Center = points[0], Tag = 1 };
            Marker target2 = new Marker() { Fill = Brushes.Blue, Side= radius*2, Center = points[1], Tag = 2 };
            Marker target3 = new Marker() { Fill = Brushes.Green, Side = radius* 2, Center = points[2], Tag = 3 };

            targets = new List<IDot>() { target1, target2, target3 };
            Canvas.Children.Add(target1);
            Canvas.Children.Add(target2);
            Canvas.Children.Add(target3);
            gazeOn = true;
            TrackerEvent.PointSessionStart.Log(index, radius, maxDistance);
            ClockLabel.Visibility = Visibility.Hidden;

        }

        void Canvas_LostTouchCapture(object sender, TouchEventArgs e)
        {
            Point location = e.GetTouchPoint(this).Position;
            touchPoints.Remove(e.TouchDevice);
            TrackerEvent.Touch.Log(index, location.X, location.Y, e.TouchDevice.Id, "TouchUp");
        }

        void Canvas_TouchMove(object sender, TouchEventArgs e)
        {
            Point location = e.GetTouchPoint(this).Position;
            touchPoints[e.TouchDevice] = location;
            
            TrackerEvent.Touch.Log(index, location.X, location.Y, e.TouchDevice.Id, "TouchMove");
        }

        void Canvas_TouchDown(object sender, TouchEventArgs e)
        {
            TouchDevice touchDevice = e.TouchDevice;
            Point location = e.GetTouchPoint(this).Position;
            Canvas.CaptureTouch(touchDevice);
            //touchDevice.Capture(Canvas);
            touchPoints.Add(touchDevice, location);

            TrackerEvent.Touch.Log(index, location.X, location.Y, e.TouchDevice.Id, "TouchDown");
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

            lastGaze = new Point(transform.X, transform.Y);

            //if (knotPoints.Count < 2)
            //    return;

            if (e.GazePoint.X < 0 || e.GazePoint.X > 1920 || e.GazePoint.Y < 0 || e.GazePoint.Y > 1080)
            {
                return;
            }

            if (!gazeOn)
                return;

            if (touchPoints.Count < 2)
            {
                TrackerEvent.Gaze.Log(index, e.GazePoint.X, e.GazePoint.Y, e.LeftValid, e.RightValid, "GazeNotEngaged");
                return;
            }
            else
                TrackerEvent.Gaze.Log(index, e.GazePoint.X, e.GazePoint.Y, e.LeftValid, e.RightValid, "GazeEngaged");

            Point gazeLocation = new Point(e.GazePoint.X, e.GazePoint.Y);

            bool result = false;
            foreach (IDot d in targets)
            {
                Point[] pointArray = touchPoints.Values.ToArray();
                
                bool intersect1= d.IntersectsWith(pointArray[0]);
                bool intersect2= d.IntersectsWith(pointArray[1]);
                bool intersect3= d.IntersectsWith(gazeLocation);
                result = intersect1 || intersect2 || intersect3;
                if (!result)
                    return;
            }

            Indicator.Fill = Brushes.Green;
            CompleteSession();

        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            foreach (TraceListener tl in System.Diagnostics.Trace.Listeners)
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