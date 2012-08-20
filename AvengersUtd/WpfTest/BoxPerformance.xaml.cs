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

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for BoxPerformance.xaml
    /// </summary>
    public partial class BoxPerformance : Window
    {
        public BoxPerformance()
        {
            InitializeComponent();
        }

        public void SetData(DateTime startTime,
            List<DateTime> gazeEvents, List<DateTime> touch1Events, List<DateTime> touch2Events,
            DateTime endTime)
        {
            TimeSpan duration = endTime - startTime;

            double y = 64;
            foreach (DateTime t in gazeEvents)
            {
                TimeSpan ts = t - startTime;
                double  x = (ts.TotalMilliseconds / duration.TotalMilliseconds) * 512;
                Dot dot = BuildControlPoint(new Point(x, y), Brushes.Red);
                Canvas.Children.Add(dot);
            }

            y = 128;
            foreach (DateTime t in touch1Events)
            {
                TimeSpan ts = t - startTime;
                double x = (ts.TotalMilliseconds / duration.TotalMilliseconds) * 512;
                Dot dot = BuildControlPoint(new Point(x, y), Brushes.Blue);
                Canvas.Children.Add(dot);
            }

            y = 192;
            foreach (DateTime t in touch2Events)
            {
                TimeSpan ts = t - startTime;
                double x = (ts.TotalMilliseconds / duration.TotalMilliseconds) * 512;
                Dot dot = BuildControlPoint(new Point(x, y), Brushes.Green);
                Canvas.Children.Add(dot);
            }
            
        }

        public Dot BuildControlPoint(Point location, Brush color)
        {
            Dot knotPoint = new Dot() { Center = location, Fill = color, Radius =  4 };
            return knotPoint;
        }
    }
}
