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
    }
}
