using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace WpfTest
{
    public class Target : Image
    {
        public System.Windows.Point Location
        {
            get
            {
                TranslateTransform transform = this.RenderTransform as TranslateTransform;
                if (transform == null)
                    return new Point(0, 0);
                else return new Point(transform.X, transform.Y);
            }
        }

        public Vector Velocity
        {
            get;
            set;
        }

        public bool IsPinnedLeft
        { get; set; }

        public bool IsPinnedRight
        { get; set; }

        public bool IntersectsWith(Point location)
        {
            Rect rect = new Rect(Location.X, Location.Y, Width, Height);
            return rect.Contains(location);
        }

        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("CenterProperty", typeof(Point), typeof(Target), new PropertyMetadata(OnCenterChanged));

        public Point Center
        {
            set { SetValue(CenterProperty, value); }
            get { return (Point)GetValue(CenterProperty); }
        }

        private static void OnCenterChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Target target = sender as Target;
            if (target == null)
                return;

            Point newCenter = (Point)e.NewValue;
            target.RenderTransform = new TranslateTransform(newCenter.X, newCenter.Y);
        }
    }
}
