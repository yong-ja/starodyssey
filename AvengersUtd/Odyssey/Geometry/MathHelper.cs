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

        public static float Scale(float value, float min, float max)
        {
            return (value/(max - min));
        }

        public static float Clamp(float value, float min, float max)
        {
            return (value > max) ? max
                           : (value < min) ? min
                                     : value;
        }

        public static float DegreesToRadians(float degrees)
        {
            return degrees*(Pi/180);
        }

    }
}
