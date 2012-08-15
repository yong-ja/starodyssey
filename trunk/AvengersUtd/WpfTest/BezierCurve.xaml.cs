#region Using directives

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace WpfTest
{
    /// <summary>
    ///   Interaction logic for BezierCurve.xaml
    /// </summary>
    public partial class BezierCurve : UserControl
    {
        #region Dependency properties
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point), typeof(BezierCurve),
                                        new PropertyMetadata(OnStartPointChanged));

        public static readonly DependencyProperty ControlPoint1Property =
            DependencyProperty.Register("ControlPoint1", typeof(Point), typeof(BezierCurve),
                                        new PropertyMetadata(OnControlPoint1Changed));

        public static readonly DependencyProperty ControlPoint2Property =
            DependencyProperty.Register("ControlPoint2", typeof(Point), typeof(BezierCurve),
                                        new PropertyMetadata(OnControlPoint2Changed));

        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(Point), typeof(BezierCurve),
                                        new PropertyMetadata(OnEndPointChanged));

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(BezierCurve),
                                        new PropertyMetadata(OnStrokeChanged)); 
        #endregion

        public BezierCurve()
        {
            InitializeComponent();
        }

        public Point StartPoint
        {
            set { SetValue(StartPointProperty, value); }
            get { return (Point) GetValue(StartPointProperty); }
        }

        public Point ControlPoint1
        {
            set { SetValue(ControlPoint1Property, value); }
            get { return (Point) GetValue(ControlPoint1Property); }
        }

        public Point ControlPoint2
        {
            set { SetValue(ControlPoint2Property, value); }
            get { return (Point) GetValue(ControlPoint2Property); }
        }

        public Point EndPoint
        {
            set { SetValue(EndPointProperty, value); }
            get { return (Point) GetValue(EndPointProperty); }
        }

        public Brush Stroke
        {
            set { SetValue(StrokeProperty, value); }
            get { return (Brush) GetValue(StrokeProperty); }
        }

        public void SetPoint(Point p, int index)
        {
            switch (index)
            {
                case 1:
                    ControlPoint1 = p;
                    break;
                case 2:
                    ControlPoint2 = p;
                    break;
                case 3:
                    EndPoint = p;
                    break;
            }
        }

        private static void OnStartPointChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BezierCurve curve = sender as BezierCurve;
            if (curve == null)
                return;

            curve.PathFigure.StartPoint = (Point) e.NewValue;
        }

        private static void OnControlPoint1Changed(object sender, DependencyPropertyChangedEventArgs e)
        {
            BezierCurve curve = sender as BezierCurve;
            if (curve == null)
                return;

            curve.Segment.Point1 = (Point) e.NewValue;
        }

        private static void OnControlPoint2Changed(object sender, DependencyPropertyChangedEventArgs e)
        {
            BezierCurve curve = sender as BezierCurve;
            if (curve == null)
                return;

            curve.Segment.Point2 = (Point) e.NewValue;
        }

        private static void OnEndPointChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BezierCurve curve = sender as BezierCurve;
            if (curve == null)
                return;

            curve.Segment.Point3 = (Point) e.NewValue;
        }

        private static void OnStrokeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BezierCurve curve = sender as BezierCurve;
            if (curve == null)
                return;

            curve.Path.Stroke = (Brush) e.NewValue;
        }
    }
}