using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfTest
{
    public partial class Dot : UserControl
    {
        public const int Radius = 8;
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center",
                typeof(Point),
                typeof(Dot),
                new PropertyMetadata(new Point(), OnCenterChanged));

        public Dot()
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
            (obj as Dot).ellipseGeo.Center = (Point)args.NewValue;
        }
    }
}
