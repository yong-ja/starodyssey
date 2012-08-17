using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfTest
{
    
    public static class Bezier
    {
        internal static Dispatcher Dispatcher;
        delegate Point[] CompareOperation(PathGeometry user, PathGeometry target, double progress);

        public static double ComputeLength(PathGeometry path)
        {
            PathGeometry flattenedPath = path.GetFlattenedPathGeometry();
            double length = 0;
            Point prev = path.Figures[0].StartPoint;
            foreach (PolyLineSegment segment in flattenedPath.Figures[0].Segments)
            {
                foreach (Point p in segment.Points)
                {
                    double d = Point.Subtract(p, prev).Length;
                    prev = p;
                    length += d;
                }
            }

            return length;
        }

        public static Point[] ComparePoints(PathGeometry user, PathGeometry target, double progress)
        {
            Point pU, pT1, pT2, tU, tT1, tT2;
            user.GetPointAtFractionLength(progress, out pU, out tU);
            target.GetPointAtFractionLength(progress, out pT1, out tT1);
            target.GetPointAtFractionLength(1-progress, out pT2, out tT2);
            return new[] {pU, pT1, pT2};
        }

        public static double FindError(PathGeometry user, PathGeometry target)
        {
            List<double> pForward = new List<double>();
            List<double> pBackward = new List<double>();

            for (int t=1; t<=100; t++)
            {
                //Dispatcher.BeginInvoke(CompareOperation, new object[]{user,target, (double)t/100 });

                Point[] points = ComparePoints(user, target, (double) t/100);
                Point pU = points[0];
                Point pT1 = points[1];
                Point pT2 = points[2];
                pForward.Add((pU-pT1).LengthSquared);
                pBackward.Add((pU-pT2).LengthSquared);
            }

            double value1 = pForward.Average();
            double value2 = pBackward.Average();
            double minValue = Math.Min(value1, value2);
            double sqrRoot = Math.Sqrt(minValue);
            
            return sqrRoot;
            //return minValue;
        }
    }
}
