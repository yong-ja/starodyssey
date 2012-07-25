using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AvengersUtd.Odyssey.Geometry;

namespace WpfTest
{
    public partial class Marker : UserControl,IDot
    {
        public const int Radius = 8;
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center",
                typeof(Point),
                typeof(Marker),
                new PropertyMetadata(new Point(), OnCenterChanged));

        public Marker()
        {
            InitializeComponent();
        }

        public Point Center
        {
            set { SetValue(CenterProperty, value); }
            get { return (Point)GetValue(CenterProperty); }
        }

        static void OnCenterChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Point p = (Point)args.NewValue;
            Marker marker = (Marker)obj;
            marker.RenderTransform = new TranslateTransform(p.X - marker.Rectangle.Width/2, p.Y - marker.Rectangle.Height / 2);
        }
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(Marker),
        new PropertyMetadata(OnFillChanged));

        public Brush Fill
        {
            set { SetValue(FillProperty, value); }
            get { return (Brush)GetValue(FillProperty); }

        }

        private static void OnFillChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Marker marker = sender as Marker;
            if (marker == null)
                return;

            marker.Rectangle.Fill = (Brush)e.NewValue;
        }

        public static readonly DependencyProperty SideProperty =
    DependencyProperty.Register("Side", typeof(double), typeof(Marker),
new PropertyMetadata(OnSideChanged));

        public double Side
        {
            set { SetValue(SideProperty, value); }
            get { return (double)GetValue(SideProperty); }

        }

        private static void OnSideChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Marker marker = sender as Marker;
            if (marker == null)
                return;

            marker.Rectangle.Width = marker.Rectangle.Height = ((double)e.NewValue);
            marker.RenderTransform = new TranslateTransform(marker.Center.X - marker.Rectangle.Width / 2, 
                marker.Center.Y - marker.Rectangle.Height / 2);
        }

        public bool IntersectsWith(Point p)
        {
            double radius = Math.Max(32, Radius);
            return Intersection.CirclePointTest(new Vector2D(Center.X, Center.Y), (float)radius, new Vector2D(p.X, p.Y));
        }
    }
}
