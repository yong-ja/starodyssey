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
using AvengersUtd.Odyssey.Geometry;
using Ellipse = System.Windows.Shapes.Ellipse;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for SplineTask.xaml
    /// </summary>
    public partial class SplineTask : Window
    {
        const int radius = 16;
        Dictionary<TouchDevice, Ellipse> knotPoints;
        List<Ellipse> ellipses;

        public SplineTask()
        {
            knotPoints = new Dictionary<TouchDevice, Ellipse>();
            ellipses = new List<Ellipse>();
            InitializeComponent();

            Canvas.Children.Add(BuildEndPoint(UserCurve.Points[0]));
            for (int i=1; i < UserCurve.Points.Count-1; i++)
            {
                Point p = UserCurve.Points[i];
                Ellipse knotPoint = BuildKnotPoint(p);
                knotPoint.Tag = i;
                Canvas.Children.Add(knotPoint);
                ellipses.Add(knotPoint);
            }
            Canvas.Children.Add(BuildEndPoint(UserCurve.Points[4]));
            Loaded += new RoutedEventHandler(SplineTask_Loaded);
            TouchDown += new EventHandler<TouchEventArgs>(ellipse_TouchDown);
            TouchMove += new EventHandler<TouchEventArgs>(ellipse_TouchMove);
            LostTouchCapture += new EventHandler<TouchEventArgs>(ellipse_LostTouchCapture);
        }

        void SplineTask_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }



        void ellipse_LostTouchCapture(object sender, TouchEventArgs e)
        {
            knotPoints.Remove(e.TouchDevice);
        }

        void ellipse_TouchMove(object sender, TouchEventArgs e)
        {
            if (!knotPoints.ContainsKey(e.TouchDevice))
                return;
            Ellipse ellipse = knotPoints[e.TouchDevice];
            Point newLocation = e.GetTouchPoint(this).Position;
            int index = (int)ellipse.Tag;
            UserCurve.Points[index] = newLocation;
            ellipse.RenderTransform = new TranslateTransform(newLocation.X-radius/2, newLocation.Y-radius/2);

        }

        void ellipse_TouchDown(object sender, TouchEventArgs e)
        {
            TouchDevice touchDevice = e.TouchDevice;
            Point location = e.GetTouchPoint(this).Position;
            Ellipse ellipse = FindKnotPoint(location);
            if (ellipse == null)
                return;
            
            touchDevice.Capture(ellipse);
            knotPoints.Add(touchDevice, ellipse);
        }

        Ellipse FindKnotPoint(Point location)
        {
            foreach (Ellipse e in ellipses)
            {
                TranslateTransform transform = (TranslateTransform)e.RenderTransform;
                Vector2D ellipseCenter = new Vector2D(transform.X, transform.Y);
                Circle c = new Circle(ellipseCenter, 2*(int)radius);
                bool test = Intersection.CirclePointTest(c, new Vector2D(location.X, location.Y));
                if (test)
                    return e;
            }
            return null;
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

        Ellipse BuildKnotPoint(Point location)
        {

            Ellipse ellipse = new Ellipse
            {
                RenderTransform = new TranslateTransform(location.X - radius / 2, location.Y - radius / 2),
                Fill = Brushes.DarkBlue,
                Width = radius,
                Height = radius
            };
            return ellipse;
        }


    }
}
