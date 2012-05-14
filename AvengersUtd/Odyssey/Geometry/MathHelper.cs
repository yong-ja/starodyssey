using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public class MathHelper
    {
        /// <summary>
        /// A value specifying the approximation of π which is 180 degrees.
        /// </summary>
        public const float Pi = 3.141592653589793239f;

        /// <summary>
        /// A value specifying the approximation of 2π which is 360 degrees.
        /// </summary>
        public const float TwoPi = 6.283185307179586477f;

        /// <summary>
        /// A value specifying the approximation of π/2 which is 90 degrees.
        /// </summary>
        public const float PiOver2 = 1.570796326794896619f;

        /// <summary>
        /// A value specifying the approximation of π/4 which is 45 degrees.
        /// </summary>
        public const float PiOverFour = 0.785398163397448310f;

        /// <summary>
        /// The value for which all absolute numbers smaller than are considered equal to zero.
        /// </summary>
        public const float Epsilon = 1e-6f;

        public const double EpsilonD = 1e-6d;

        public static float Scale(float value, float min, float max)
        {
            return min + (max - min)*value;
        }

        public static double Scale(double value, double min, double max)
        {
            return min + (max - min) * value;
        }

        public static double ConvertRange(
                double originalStart, double originalEnd, // original range
                double newStart, double newEnd, // desired range
                double value) // value to convert
        {
            double scale = (newEnd - newStart)/(originalEnd - originalStart);
            return (newStart + ((value - originalStart)*scale));
        }

        public static int ConvertRange( int originalStart, int originalEnd, // original range
                int newStart, int newEnd, // desired range
                int value) // value to convert
        {
            int scale = (newEnd - newStart) / (originalEnd - originalStart);
            return (newStart + ((value - originalStart) * scale));
        }

        public static float Clamp(float value, float min, float max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        public static float DegreesToRadians(float degrees)
        {
            return degrees*(Pi/180);
        }

        /// <summary>
        /// Determines if three vertices are collinear (ie. on a straight line)
        /// </summary>
        /// <param name="a">First vertex</param>
        /// <param name="b">Second vertex</param>
        /// <param name="c">Third vertex</param>
        /// <returns></returns>
        public static bool Collinear(ref Vector2D a, ref Vector2D b, ref Vector2D c)
        {
            return Collinear(ref a, ref b, ref c, 0);
        }

        public static bool Collinear(ref Vector2D a, ref Vector2D b, ref Vector2D c, double tolerance)
        {
            return DoubleInRange(Area(ref a, ref b, ref c), -tolerance, tolerance);
        }

        /// <summary>
        /// Checks if a floating point Value is within a specified
        /// range of values (inclusive).
        /// </summary>
        /// <param name="value">The Value to check.</param>
        /// <param name="min">The minimum Value.</param>
        /// <param name="max">The maximum Value.</param>
        /// <returns>True if the Value is within the range specified,
        /// false otherwise.</returns>
        public static bool DoubleInRange(double value, double min, double max)
        {
            return (value >= min && value <= max);
        }

        /// <summary>
        /// Returns a positive number if c is to the left of the line going from a to b.
        /// </summary>
        /// <returns>Positive number if point is left, negative if point is right, 
        /// and 0 if points are collinear.</returns>
        public static double Area(Vector2D a, Vector2D b, Vector2D c)
        {
            return Area(ref a, ref b, ref c);
        }

        /// <summary>
        /// Returns a positive number if c is to the left of the line going from a to b.
        /// </summary>
        /// <returns>Positive number if point is left, negative if point is right, 
        /// and 0 if points are collinear.</returns>
        public static double Area(ref Vector2D a, ref Vector2D b, ref Vector2D c)
        {
            return a.X*(b.Y - c.Y) + b.X*(c.Y - a.Y) + c.X*(a.Y - b.Y);
        }

        public static bool IsValid(double x)
        {
            if (double.IsNaN(x))
            {
                // NaN.
                return false;
            }

            return !double.IsInfinity(x);
        }
    }
}