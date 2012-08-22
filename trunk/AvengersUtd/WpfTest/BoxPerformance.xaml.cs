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

            double x;

            TrackerEvent.BoxData.Write(string.Format("Time, X, Y, Z, {0:hh:mm:ss.fff}, {1:hh:mm:ss.fff}",
                startTime, endTime));

            foreach (InputEvent eventData in events)
            {
                DateTime ts = eventData.TimeStamp;
                TimeSpan t = ts - startTime;
                float[] progress = eventData.Progress;
                x = (t.TotalMilliseconds / duration.TotalMilliseconds) * 512;


                Color eyeColor = progress[0] == 0 ? Colors.Transparent :
                    Color.FromArgb(255, (byte)(progress[0] * 255), 0, 0);
                Brush eyeBrush = new SolidColorBrush(eyeColor);
                Canvas.Children.Add(BuildControlPoint(new Point(x, 64), eyeBrush));

                Color leftColor = progress[1] == 0 ? Colors.Transparent : Color.FromArgb(255, 0, (byte)(progress[1] * 255), 0);
                Brush leftBrush = new SolidColorBrush(leftColor);
                
                Canvas.Children.Add(BuildControlPoint(new Point(x, 128), leftBrush));

                Color rightColor = progress[2] == 0 ? Colors.Transparent : Color.FromArgb(255, 0, 0, (byte)(progress[2] * 255));
                Brush rightBrush = new SolidColorBrush(rightColor);
                
                Canvas.Children.Add(BuildControlPoint(new Point(x, 192), rightBrush));

                TrackerEvent.BoxData.Log(t.ToString("ss.fff"), progress[0], progress[1], progress[2]);
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
               timeStamp.ToString("yyyyMMdd") + '_' + Test.Count + System.IO.Path.GetExtension(filePrefix));
            // save file to disk
            FileStream fs = File.Open(filename, FileMode.OpenOrCreate);
            encoder.Save(fs);
        }
    }
}
