using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public class MathHelper
    {
        public const float Pi = (float)Math.PI;
        public const float PiOver2 = Pi/2;
        public const float TwoPi = Pi*2;

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
    }
}
