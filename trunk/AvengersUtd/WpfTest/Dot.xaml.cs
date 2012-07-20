using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AvengersUtd.Odyssey.Geometry;

namespace WpfTest
{
    public partial class Dot : UserControl, IDot
    {
        public const double DefaultRadius = 8;
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center",
                typeof(Point),
                typeof(Dot),
                new PropertyMetadata(new Point(), OnCenterChanged));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius",
                typeof(double),
                typeof(Dot),
                new PropertyMetadata(DefaultRadius, OnRadiusChanged));

        public Dot()
        {
            InitializeComponent();
        }

        public Point Center
        {
            set { SetValue(CenterProperty, value); }
            get { return (Point)GetValue(CenterProperty); }
        }

        public double Radius
        {
            set { SetValue(RadiusProperty, value); }
            get { return (double)GetValue(RadiusProperty); }
        }


        static void OnCenterChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as Dot).ellipseGeo.Center = (Point)args.NewValue;
        }

        static void OnRadiusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Dot dot = (Dot)obj;
            dot.ellipseGeo.RadiusX = (double)args.NewValue;
            dot.ellipseGeo.RadiusY = (double)args.NewValue;
        }



        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(Dot),
        new PropertyMetadata(OnFillChanged));

        public Brush Fill
        {
            set { SetValue(FillProperty, value); }
            get { return (Brush)GetValue(FillProperty); }

        }

        private static void OnFillChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Dot dot = sender as Dot;
            if (dot == null)
                return;

            dot.Path.Fill = (Brush)e.NewValue;
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(Dot),
        new PropertyMetadata(OnStrokeChanged));

        public Brush Stroke
        {
            set { SetValue(StrokeProperty, value); }
            get { return (Brush)GetValue(StrokeProperty); }

        }

        private static void OnStrokeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Dot dot = sender as Dot;
            if (dot == null)
                return;

            dot.Path.Stroke = (Brush)e.NewValue;
        }

        public bool IntersectsWith(Point p)
        {
            return Intersection.CirclePointTest(new Vector2D(Center.X, Center.Y), (float)Radius, new Vector2D(p.X, p.Y));
        }
    }


    public interface IDot
    {
        Point Center { get; set; }
        object Tag { get; set; }
    }
}
