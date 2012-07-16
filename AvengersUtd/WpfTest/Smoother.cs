using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using SlimDX;

namespace WpfTest
{
    /// <summary>
    /// Average window smoothing algorithm. It calculates the smoothed
    /// coordinates as the average in the last N samples
    /// </summary>
    public class AverageWindow
    {
        #region Variables

        private readonly List<Point> data;
        private readonly int numberOfSamples;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="numOfSamples">Number of samples we consider 
        /// (i.e. window size)</param>
        public AverageWindow(int numOfSamples)
        {
            data = new List<Point>();
            numberOfSamples = numOfSamples;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Smooth method. Call this method when we get a new
        /// sample and it will return the smoothed coordinates
        /// </summary>
        /// <param name="newPoint">New gazed point</param>
        /// <returns>Smoothed point</returns>
        public Point Smooth(Point newPoint)
        {
            Point smoothedPoint;

            if (data.Count < numberOfSamples)
                data.Add(newPoint);
            else
            {
                data.RemoveAt(0);
                data.Add(newPoint);
            }

            smoothedPoint = Mean(data.ToArray());

            return smoothedPoint;
        }

        /// <summary>
        /// Stop smoothing. Call this method when we detect a saccade.
        /// </summary>
        public void Stop()
        {
            data.Clear();
        }

        #endregion

        #region Mean

        /// <summary>
        /// Mean of an array of doubles
        /// </summary>
        /// <param name="num">Array of doubles</param>
        /// <returns>Mean of the array</returns>
        public static double Mean(double[] num)
        {
            double sum = 0;
            for (int i = 0; i < num.Length; i++)
            {
                sum = sum + num[i];
            }

            return sum/num.Length;
        }

        /// <summary>
        /// Mean GTPoint of an array of GTPoints
        /// </summary>
        /// <param name="points">Array of GTPoints</param>
        /// <returns>GTPoint containing the mean</returns>
        public static Point Mean(Point[] points)
        {
            double x = 0;
            double y = 0;

            for (int i = 0; i < points.Length; i++)
            {
                x = x + points[i].X;
                y = y + points[i].Y;
            }

            return new Point(x/points.Length, y/points.Length);
        }

        #endregion
    }

}
