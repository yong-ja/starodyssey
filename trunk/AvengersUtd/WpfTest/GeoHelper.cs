using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using System.Windows;
using AvengersUtd.Odyssey.Utils.Logging;

namespace WpfTest
{
    public static class GeoHelper
    {

        static Random rand = new Random();
        static int x;
        static int y;
        static Ellipse outerEllipse = new Ellipse(new Vector2D(960, 1208), 960, 1080);
        static int bottomOffset = 256;
        //static Circle innerCircle= new Circle(new Vector2D(960, 1080), 256);

        public static Point ChooseRandomPointWithinBounds(int xBound, int yBound)
        {
            double x = rand.NextDouble() * xBound;
            double y = rand.NextDouble() * yBound;
            return new Point(x, y);
        }


        public static Point ChooseRandomPointOnCircle(Point center, float radius, bool doTest=true)
        {
            bool test = false;
            double x = 0, y = 0;
            Vector2D p = Vector2D.Zero;

            while (!test)
            {
                double t = rand.NextDouble()*2;
                x = center.X + (radius*Math.Cos(t*Math.PI));
                y = center.Y + (radius*Math.Sin(t*Math.PI));
                p = new Vector2D(x, y);

                test = p.Y < (1080 - bottomOffset) && (p.X > 0 && p.X < 1920);
                if (doTest)
                    test = test && Intersection.EllipsePointTest(outerEllipse, p);
            }

            return new Point(x, y);
        }

        public static Point ChooseRandomPointInsideCircle(Point center, float radius, bool doTest=true)
        {
            bool test = false;
            double x = 0, y = 0;
            while (!test)
            {
                double t = rand.NextDouble() * 2;
                double randomRadius = radius / 2 + rand.Next((int)radius / 2);
                x = center.X + (randomRadius * Math.Cos(t * Math.PI));
                y = center.Y + (randomRadius * Math.Sin(t * Math.PI));
                Vector2D p = new Vector2D(x, y);
                if (doTest)
                    test = Intersection.EllipsePointTest(outerEllipse, p) && p.Y < (1080 - bottomOffset);
                else
                    test = true;
            }

            return new Point(x, y);
        }

        public static Point ChoosePointOnCircle(Point center, int radius, double angleRad)
        {
            bool test = false;
            double x = 0, y = 0;
            double t = angleRad;
            x = center.X + (radius * Math.Cos(t));
            y = center.Y + (radius * Math.Sin(t));
            Vector2D p = new Vector2D(x, y);
            //test = Intersection.EllipsePointTest(outerEllipse, p) && p.Y < (1080 - bottomOffset);
            
            //if (!test)
            //    return new Point(-1, -1);
            return new Point(x, y);
        }

        public static Point ChooseRandomPoint()
        {
            Vector2D p = new Vector2D();

            bool test = false;
            int index = 0;
            while (!test)
            {
                x = rand.Next(1920);
                y = rand.Next(1080);

                p = new Vector2D(x, y);

                test = Intersection.EllipsePointTest(outerEllipse, p) && p.Y < (1080 - bottomOffset);
                index++;

                if (index > 100)
                {
                    LogEvent.Engine.Write("More than 100 attempts!");
                    break;
                }
            }

            return new Point(p.X, p.Y);

        }

        public static Point[] ChooseTrianglePoints(Point center, float radius, float circleRadius)
        {
            Point[] points = new Point[3];
            points[0] = ChooseRandomPointInsideCircle(center, radius/4);
            bool test = false;
            while (!test)
            {
                points[1] = ChooseRandomPointOnCircle(points[0], radius, false);
                if (CircleTest(points[0], points[1], (double)circleRadius, (double)circleRadius))
                    continue;
                if (points[1].X < points[0].X && points[1].Y >= 256 && points[1].Y <= 840)
                    test = true;
            }
            test = false;
            while (!test)
            {
                points[2] = ChooseRandomPointOnCircle(points[0], radius, false);
                if (CircleTest(points[0], points[2], (double)circleRadius, (double)circleRadius) ||
                    CircleTest(points[1], points[2], (double)circleRadius, (double)circleRadius))
                    continue;
                if (points[2].X > points[0].X && points[2].Y >= 256 && points[2].Y <= 840)
                    test = true;
            }
            return points;
        }

        //public static Point[] ChooseTrianglePoints(Point center, int radius, int circleRadius)
        //{
        //    Point[] points = new Point[3];
        //    double initialAngle = rand.NextDouble() * 2 * Math.PI;
        //    points[0] = ChoosePointOnCircle(center, radius, initialAngle);
        //    points[1] = ChoosePointOnCircle(center, radius, initialAngle + 2 * Math.PI * 1 / 3);
        //    points[2] = ChoosePointOnCircle(center, radius, initialAngle + 2 * Math.PI * 2 / 3);

        //    points[0] = ChooseRandomPointOnCircle(points[0], radius/4);
        //    points[1] = ChooseRandomPointOnCircle(points[1], radius/4);
        //    points[2] = ChooseRandomPointOnCircle(points[2], radius/4);

        //    return points;
        //}

        private static bool CircleTest(Point c1, Point c2, double radius1, double radius2)
        {
            // Find the distance between the centers.
            double dx = c1.X - c2.X;
            double dy = c1.Y - c2.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);

            // See how manhym solutions there are.
            if (dist > radius1 + radius2)
            {
                // No solutions, the circles are too far apart.
                //intersection1 = new PointF(float.NaN, float.NaN);
                //intersection2 = new PointF(float.NaN, float.NaN);
                return false;
            }
            else if (dist < Math.Abs(radius1 - radius2))
            {
                // No solutions, one circle contains the other.
                //intersection1 = new PointF(float.NaN, float.NaN);
                //intersection2 = new PointF(float.NaN, float.NaN);
                return false;
            }
            else if ((dist == 0) && (radius1 == radius2))
            {
                // No solutions, the circles coincide.
                //intersection1 = new PointF(float.NaN, float.NaN);
                //intersection2 = new PointF(float.NaN, float.NaN);
                return false;
            }
            else
            {
                // Find a and h.
                //double a = (radius0 * radius0 -
                //    radius1 * radius1 + dist * dist) / (2 * dist);
                //double h = Math.Sqrt(radius0 * radius0 - a * a);

                //// Find P2.
                //double cx2 = cx0 + a * (cx1 - cx0) / dist;
                //double cy2 = cy0 + a * (cy1 - cy0) / dist;

                //// Get the points P3.
                //intersection1 = new PointF(
                //    (float)(cx2 + h * (cy1 - cy0) / dist),
                //    (float)(cy2 - h * (cx1 - cx0) / dist));
                //intersection2 = new PointF(
                //    (float)(cx2 - h * (cy1 - cy0) / dist),
                //    (float)(cy2 + h * (cx1 - cx0) / dist));

                // See if we have 1 or 2 solutions.
                //if (dist == radius0 + radius1) return 1;
                //return 2;

                return true;
            }
        }
    }
}
