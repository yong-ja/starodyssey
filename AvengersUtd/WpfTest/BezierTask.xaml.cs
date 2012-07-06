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
using Microsoft.Surface.Presentation.Controls;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for BezierTask.xaml
    /// </summary>
    public partial class BezierTask : SurfaceWindow
    {
        Dictionary<TouchDevice, int> points;
        QuadraticBezierSegment qbs;
        PathFigure pf;
        int pointsOnScreen;
        int markerRadius = 16;
        Ellipse startMarker, endMarker;
        Rectangle cpMarker;

        public BezierTask()
        {
            points = new Dictionary<TouchDevice, int>();
            InitializeComponent();
            TouchDown += new EventHandler<TouchEventArgs>(BezierTask_TouchDown);
            TouchUp += new EventHandler<TouchEventArgs>(BezierTask_TouchUp);
            TouchMove += new EventHandler<TouchEventArgs>(BezierTask_TouchMove);
            
            startMarker = new Ellipse { Width = markerRadius, Height = markerRadius, Fill = Brushes.Transparent, Stroke = Brushes.Red, StrokeThickness=2};
            endMarker = new Ellipse { Width = markerRadius, Height = markerRadius, Fill = Brushes.Transparent, Stroke = Brushes.Red, StrokeThickness = 2 };
            cpMarker = new Rectangle { Width = markerRadius, Height = markerRadius, Fill = Brushes.Transparent, Stroke = Brushes.Blue, StrokeThickness = 2 };

            DrawLine(Canvas, 50, 50, 712, 512, Colors.Black);
        }

        void BezierTask_TouchMove(object sender, TouchEventArgs e)
        {
            int index = points[e.TouchDevice];
            if (index == 0)
            {
                pf.StartPoint = e.GetTouchPoint(this).Position;
                TranslateTransform transform = (TranslateTransform)startMarker.RenderTransform;
                transform.X = pf.StartPoint.X - markerRadius/2;
                transform.Y = pf.StartPoint.Y - markerRadius / 2;
            }
            else
            {
                qbs.Point2 = e.GetTouchPoint(this).Position;
                TranslateTransform transform = (TranslateTransform)endMarker.RenderTransform;
                transform.X = qbs.Point2.X - markerRadius / 2;
                transform.Y = qbs.Point2.Y - markerRadius / 2;
            }
        }

        void BezierTask_TouchUp(object sender, TouchEventArgs e)
        {
            points.Remove(e.TouchDevice);
            pointsOnScreen--;
        }

        void BezierTask_TouchDown(object sender, TouchEventArgs e)
        {
            points.Add(e.TouchDevice, pointsOnScreen);
            pointsOnScreen++;
        }

        private PathGeometry DrawLine(Canvas canvas, double X1, double Y1, double X2, double Y2, Color color)
        {
            qbs = new QuadraticBezierSegment(new Point(X2, Y1), new Point(X2, Y2), true);

            PathSegmentCollection pscollection = new PathSegmentCollection();
            pscollection.Add(qbs);

            pf = new PathFigure();
            pf.Segments = pscollection;
            pf.StartPoint = new Point(X1, Y1);
            startMarker.RenderTransform = new TranslateTransform(X1 - markerRadius/2, Y1- markerRadius/2);
            endMarker.RenderTransform = new TranslateTransform(X2 - markerRadius / 2, Y2 - markerRadius / 2);
            cpMarker.RenderTransform = new TranslateTransform(X2 - markerRadius / 2, Y1 - markerRadius / 2);

            PathFigureCollection pfcollection = new PathFigureCollection();
            pfcollection.Add(pf);

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = pfcollection;

            Path path = new Path();
            path.Data = pathGeometry;
            path.Stroke = new SolidColorBrush(color);
            path.StrokeThickness = 2;
            canvas.Children.Add(path);

            canvas.Children.Add(startMarker);
            canvas.Children.Add(endMarker);
            canvas.Children.Add(cpMarker);

            return pathGeometry;
        }
    }
}
