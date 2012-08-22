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
using System.IO;

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

        //public void SetData(DateTime startTime,
        //    List<DateTime> gazeEvents, List<DateTime> touch1Events, List<DateTime> touch2Events,
        //    DateTime endTime)
        //{
        //    TimeSpan duration = endTime - startTime;

        //    double y = 64;
        //    foreach (DateTime t in gazeEvents)
        //    {
        //        TimeSpan ts = t - startTime;
        //        double  x = (ts.TotalMilliseconds / duration.TotalMilliseconds) * 512;
        //        Dot dot = BuildControlPoint(new Point(x, y), Brushes.Red);
        //        Canvas.Children.Add(dot);
        //    }

        //    y = 128;
        //    foreach (DateTime t in touch1Events)
        //    {
        //        TimeSpan ts = t - startTime;
        //        double x = (ts.TotalMilliseconds / duration.TotalMilliseconds) * 512;
        //        Dot dot = BuildControlPoint(new Point(x, y), Brushes.Blue);
        //        Canvas.Children.Add(dot);
        //    }

        //    y = 192;
        //    foreach (DateTime t in touch2Events)
        //    {
        //        TimeSpan ts = t - startTime;
        //        double x = (ts.TotalMilliseconds / duration.TotalMilliseconds) * 512;
        //        Dot dot = BuildControlPoint(new Point(x, y), Brushes.Green);
        //        Canvas.Children.Add(dot);
        //    }
            
        //}

      

        public void SetData(DateTime startTime, DateTime endTime, List<InputEvent> events)
        {
            TimeSpan duration = endTime - startTime;

            double x, y;
            foreach (InputEvent eventData in events)
            {
                DateTime ts = eventData.TimeStamp;
                TimeSpan t = ts - startTime;
                float[] progress = eventData.Progress;
                x = (t.TotalMilliseconds / duration.TotalMilliseconds) * 512;

                Color eyeColor = Colors.Black.Lerp(Colors.Red, progress[0]);
                Brush eyeBrush = new SolidColorBrush(eyeColor);
                Canvas.Children.Add(BuildControlPoint(new Point(x, 64), eyeBrush));

                Color leftColor = Colors.Black.Lerp(Colors.Blue, progress[1]);
                Brush leftBrush = new SolidColorBrush(eyeColor);
                
                Canvas.Children.Add(BuildControlPoint(new Point(x, 128), leftBrush));

                Color rightColor = Colors.Black.Lerp(Colors.Green, progress[2]);
                Brush rightBrush = new SolidColorBrush(eyeColor);
                
                Canvas.Children.Add(BuildControlPoint(new Point(x, 192), rightBrush));
            }
        }

        public Dot BuildControlPoint(Point location, Brush color)
        {
            Dot knotPoint = new Dot() { Center = location, Fill = color, Radius =  4 };
            return knotPoint;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            RenderTargetBitmap targetBitmap =
                new RenderTargetBitmap((int)Canvas.ActualWidth,
                           (int)Canvas.ActualHeight,
                           96d, 96d,
                           PixelFormats.Default);
            targetBitmap.Render(Canvas);

            // add the RenderTargetBitmap to a Bitmapencoder
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            
            encoder.Frames.Add(BitmapFrame.Create(targetBitmap));

            DateTime timeStamp = System.DateTime.Today;
            string filePrefix = "BoxTask.png";
            string filename =  System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filePrefix),
               System.IO.Path.GetFileNameWithoutExtension(filePrefix) + "_" +
               timeStamp.ToString("yyyyMMdd") + '_' + BoxRenderer.Session + System.IO.Path.GetExtension(filePrefix));
            // save file to disk
            FileStream fs = File.Open(filename, FileMode.OpenOrCreate);
            encoder.Save(fs);
        }
    }
}
