#if (SlimDX)

using System;
using System.Drawing;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public static class ColorOperator
    {

        /// <summary> 
        /// Multiply a color with a factor 
        /// </summary> 
        /// <param name="color">The color</param> 
        /// <param name="factor">The factor</param> 
        /// <returns>Return color</returns> 
        public static Color Scale(Color color, float factor)
        {
            if (factor == 1.0f) return color;

            // Multiply everything except alpha using the factor 
            return Color.FromArgb(
                color.A,
                (byte)(Clamp(color.R * factor, 0, 255)),
                (byte)(Clamp(color.G * factor, 0, 255)),
                (byte)(Clamp(color.B * factor, 0, 255)));
        }

        /// <summary> 
        /// Makes a value stay within a certain range 
        /// </summary> 
        /// <param name="val">The number to clamp</param> 
        /// <param name="min">The minimum number to return</param> 
        /// <param name="max">The maximum number to return</param> 
        /// <returns>The value if it was in range, other wise min or max</returns> 
        public static float Clamp(float val, float min, float max)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }
    }

    public static class Geometry
    {
        public static float DegreeToRadian(float degrees)
        {
            return (float)((Math.PI / 180) * degrees);
        }
    }
}
#endif