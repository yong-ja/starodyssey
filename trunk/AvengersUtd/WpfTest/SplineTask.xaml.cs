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
using System.Windows.Shapes;
using AvengersUtd.Odyssey.Utils.Logging;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for SplineTask.xaml
    /// </summary>
    public partial class SplineTask : Window
    {
        Dictionary<TouchDevice, Ellipse> knotPoints;

        public SplineTask()
        {
            knotPoints = new Dictionary<TouchDevice, Ellipse>();
            InitializeComponent();

            Canvas.Children.Add(BuildEndPoint(UserCurve.Points[0]));
            for (int i=1; i < UserCurve.Points.Count-1; i++)
            {
                Point p = UserCurve.Points[i];
                Canvas.Children.Add(BuildKnotPoint(p));
            }
            Canvas.Children.Add(BuildEndPoint(UserCurve.Points[4]));
            Loaded += new RoutedEventHandler(SplineTask_Loaded);
        }

        void SplineTask_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        Ellipse BuildKnotPoint(Point location)
        {
            const int radius = 16;
            Ellipse ellipse = new Ellipse
            {
                RenderTransform = new TranslateTransform(location.X - radius/2, location.Y-radius/2),
                Fill = Brushes.DarkBlue,
                Width = radius,
                Height = radius
            };
            ellipse.TouchDown += new EventHandler<TouchEventArgs>(ellipse_TouchDown);
            ellipse.TouchMove += new EventHandler<TouchEventArgs>(ellipse_TouchMove);
            ellipse.LostTouchCapture += new EventHandler<TouchEventArgs>(ellipse_LostTouchCapture);

            return ellipse;
        }

        void ellipse_LostTouchCapture(object sender, TouchEventArgs e)
        {
            knotPoints.Remove(e.TouchDevice);
        }

        void ellipse_TouchMove(object sender, TouchEventArgs e)
        {
            Ellipse ellipse = knotPoints[e.TouchDevice];
            Point newLocation = e.GetTouchPoint(this).Position;
            ellipse.RenderTransform = new TranslateTransform(newLocation.X, newLocation.Y);
        }

        void ellipse_TouchDown(object sender, TouchEventArgs e)
        {
            TouchDevice touchDevice = e.TouchDevice;
            Ellipse ellipse = (Ellipse)sender;
            
            touchDevice.Capture(ellipse);
            knotPoints.Add(touchDevice, ellipse);
        }

        Rectangle BuildEndPoint(Point location)
        {
            const int width=16;
            Rectangle rectangle = new Rectangle()
            {
                RenderTransform = new TranslateTransform(location.X - width/2, location.Y-width/2),
                Fill = Brushes.Black,
                Width = width,
                Height = width
            };
            return rectangle;
        }


    }
}
