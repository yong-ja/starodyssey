using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            marker.RenderTransform = new TranslateTransform(p.X - marker.Width/2, p.Y - marker.Height / 2);
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

    }
}
