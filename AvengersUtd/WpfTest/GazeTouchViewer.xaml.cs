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
        int gazeRadius;
        TrackerWrapper tracker;

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
            Loaded += new RoutedEventHandler(GazeTouchViewer_Loaded);
            //TouchMove += new EventHandler<TouchEventArgs>(GazeTouchViewer_TouchMove);
            
            bConnect.TouchUp += (sender, e) => { tracker.Connect(); };
            bStart.TouchUp += (sender, e) => { tracker.StartTracking(); };
        }

        //void GazeTouchViewer_TouchMove(object sender, TouchEventArgs e)
        //{
            
        //}

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

            bool leftValid = e.LeftValid == 4;
            bool rightValid = e.RightValid == 4;
            Brush fillColor;
            if (leftValid && rightValid)
                fillColor = Brushes.Green;
            else if (leftValid && !rightValid)
                fillColor = Brushes.Yellow;
            else if (rightValid && !leftValid)
                fillColor = Brushes.Orange;
            else
            { 
                fillColor = Brushes.Red;
                //return; 
            }

            HitTestResult result = VisualTreeHelper.HitTest(Canvas, new Point(e.GazePoint.X, e.GazePoint.Y));

            if (result != null)
                return;

            Ellipse gazePoint = new Ellipse()
            {
                Width = gazeRadius,
                Height = gazeRadius,
                Fill = fillColor
            };
            LogEvent.Engine.Write(string.Format("GP({0:f2},{1:f2} L:{2} R:{3}", e.GazePoint.X, e.GazePoint.Y, e.LeftValid, e.RightValid));
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