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
using System.Diagnostics;

namespace MultitouchExperiments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<TouchDevice, Ellipse> _Followers = new Dictionary<TouchDevice, Ellipse>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TouchCanvas_TouchDown(object sender, TouchEventArgs e)
        {
            TouchCanvas.CaptureTouch(e.TouchDevice);

            Ellipse follower = new Ellipse();
            follower.Width = follower.Height = 50;
            follower.Fill = Brushes.White;
            follower.Stroke = Brushes.White;

            TouchPoint point = e.GetTouchPoint(TouchCanvas);

            follower.RenderTransform = new TranslateTransform(point.Position.X, point.Position.Y);

            _Followers[e.TouchDevice] = follower;

            TouchCanvas.Children.Add(follower);
        }

        private void TouchCanvas_TouchUp(object sender, TouchEventArgs e)
        {
            TouchCanvas.ReleaseTouchCapture(e.TouchDevice);

            TouchCanvas.Children.Remove(_Followers[e.TouchDevice]);
            _Followers.Remove(e.TouchDevice);
        }

        private void TouchCanvas_TouchMove(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.Captured == TouchCanvas)
            {
                Ellipse follower = _Followers[e.TouchDevice];
                TranslateTransform transform = follower.RenderTransform as TranslateTransform;

                TouchPoint point = e.GetTouchPoint(TouchCanvas);

                transform.X = point.Position.X;
                transform.Y = point.Position.Y;
            }
        }

        private void TouchCanvas_TouchLeave(object sender, TouchEventArgs e)
        {
            //Debug.WriteLine("leave " + e.TouchDevice.Id);
        }

        private void TouchCanvas_TouchEnter(object sender, TouchEventArgs e)
        {
            //Debug.WriteLine("enter " + e.TouchDevice.Id);
        }
    }
}