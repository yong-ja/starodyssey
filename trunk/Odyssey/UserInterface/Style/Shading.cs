#region Disclaimer

/* 
 * Shading
 *
 * Created on 21 August 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey User Interface Library
 *
 * This source code is Intellectual property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in commercial
 * projects, without the express and written consent of
 * the Author.
 *
 */

#endregion

using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX.Direct3D;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public enum ShadingType
    {
        RectangleTopToBottom,
        RectangleBottomToTop,
        RectangleLeftToRight,
        RectangleRightToLeft,
        RectangleFlat
    }

    public struct ShadingValues
    {
        float darkFactor;
        float lightFactor;


        public ShadingValues(float lightFactor, float darkFactor)
        {
            this.lightFactor = lightFactor;
            this.darkFactor = darkFactor;
        }

        public float LightFactor
        {
            get { return lightFactor; }
        }

        public float DarkFactor
        {
            get { return darkFactor; }
        }

        public bool Equals(ShadingValues otherValues)
        {
            if (lightFactor == otherValues.lightFactor &&
                darkFactor == otherValues.darkFactor)
                return true;
            else
                return false;
        }

        public static bool operator ==(ShadingValues values1, ShadingValues values2)
        {
            return values1.Equals(values2);
        }

        public static bool operator !=(ShadingValues values1, ShadingValues values2)
        {
            return !(values1.Equals(values2));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ShadingValues)) return false;
            ShadingValues shadingValues = (ShadingValues) obj;
            return lightFactor == shadingValues.lightFactor && darkFactor == shadingValues.darkFactor;
        }

        public override int GetHashCode()
        {
            return lightFactor.GetHashCode() ^ darkFactor.GetHashCode();
        }
    }

    public struct Shading
    {
        public const float DefaultDarkFactor = 0.5f;
        public const float DefaultLightFactor = 1f;
        ShadingMode mode;
        ShadingType type;
        ShadingValues values;


        public Shading(ShadingType type)
            : this(type, DefaultLightFactor, DefaultDarkFactor)
        {
        }

        public Shading(ShadingType type, float lightFactor, float darkFactor)
        {
            this.type = type;
            values = new ShadingValues(lightFactor, darkFactor);

            switch (type)
            {
                case ShadingType.RectangleTopToBottom:
                    mode = TopToBottom;
                    break;

                case ShadingType.RectangleBottomToTop:
                    mode = BottomToTop;
                    break;

                case ShadingType.RectangleLeftToRight:
                    mode = LeftToRight;
                    break;

                case ShadingType.RectangleRightToLeft:
                    mode = RightToLeft;
                    break;

                default:
                case ShadingType.RectangleFlat:
                    mode = Flat;
                    break;
            }
        }

        #region Properties

        public ShadingType Type
        {
            get { return type; }
        }


        public ShadingMode Mode
        {
            get { return mode; }
        }

        public ShadingValues Values
        {
            get { return values; }
        }

        #endregion

        #region ShadingMode methods

        /// <summary>
        /// Shades a rectangle top to bottom using the specified color.
        /// </summary>
        /// <returns>An array of ints containing the color of each vertex (0: top left 1: top right 2: bottomLeft 3: bottom right</returns>
        public static int[] TopToBottom(Color mainColor, ShadingValues values)
        {
            Color lightColor = (values.LightFactor == DefaultLightFactor)
                                   ? mainColor
                                   : ColorOperator.Scale(mainColor, values.LightFactor);
            Color darkColor = ColorOperator.Scale(mainColor, values.DarkFactor);
            int colorTopLeft = lightColor.ToArgb();
            int colorTopRight = colorTopLeft;
            int colorBottomLeft = Color.FromArgb(mainColor.A, darkColor).ToArgb();
            int colorBottomRight = colorBottomLeft;

            return new int[] {colorTopLeft, colorTopRight, colorBottomLeft, colorBottomRight};
        }

        /// <summary>
        /// Shades a rectangle bottom to top using the specified color.
        /// </summary>
        /// <returns>An array of ints containing the color of each vertex (0: top left 1: top right 2: bottomLeft 3: bottom right</returns>
        public static int[] BottomToTop(Color mainColor, ShadingValues values)
        {
            Color lightColor = (values.LightFactor == DefaultLightFactor)
                                   ? mainColor
                                   : ColorOperator.Scale(mainColor, values.LightFactor);
            Color darkColor = ColorOperator.Scale(mainColor, values.DarkFactor);

            int colorTopLeft = Color.FromArgb(mainColor.A, darkColor).ToArgb();
            int colorTopRight = colorTopLeft;
            int colorBottomLeft = lightColor.ToArgb();
            int colorBottomRight = colorBottomLeft;

            return new int[] {colorTopLeft, colorTopRight, colorBottomLeft, colorBottomRight};
        }

        /// <summary>
        /// Shades a rectangle left to right using the specified color.
        /// </summary>
        /// <returns>An array of ints containing the color of each vertex (0: top left 1: top right 2: bottomLeft 3: bottom right</returns>
        public static int[] LeftToRight(Color mainColor, ShadingValues values)
        {
            Color lightColor = (values.LightFactor == DefaultLightFactor)
                                   ? mainColor
                                   : ColorOperator.Scale(mainColor, values.LightFactor);
            Color darkColor = ColorOperator.Scale(mainColor, values.DarkFactor);

            int colorTopLeft = lightColor.ToArgb();
            int colorTopRight = Color.FromArgb(mainColor.A, darkColor).ToArgb();
            int colorBottomLeft = colorTopLeft;
            int colorBottomRight = colorTopRight;

            return new int[] {colorTopLeft, colorTopRight, colorBottomLeft, colorBottomRight};
        }

        /// <summary>
        /// Shades a rectangle right to left using the specified color.
        /// </summary>
        /// <returns>An array of ints containing the color of each vertex (0: top left 1: top right 2: bottomLeft 3: bottom right</returns>
        public static int[] RightToLeft(Color mainColor, ShadingValues values)
        {
            Color lightColor = (values.LightFactor == DefaultLightFactor)
                                   ? mainColor
                                   : ColorOperator.Scale(mainColor, values.LightFactor);
            Color darkColor = ColorOperator.Scale(mainColor, values.DarkFactor);

            int colorTopLeft = Color.FromArgb(mainColor.A, darkColor).ToArgb();
            int colorTopRight = lightColor.ToArgb();
            int colorBottomLeft = colorTopLeft;
            int colorBottomRight = colorTopRight;

            return new int[] {colorTopLeft, colorTopRight, colorBottomLeft, colorBottomRight};
        }


        /// <summary>
        /// It assigns the same color for each of the four vertex.
        /// </summary>
        /// <returns>An array of ints containing the color of each vertex (0: top left 1: top right 2: bottomLeft 3: bottom right</returns>
        public static int[] Flat(Color mainColor, ShadingValues values)
        {
            int colorToArgb = mainColor.ToArgb();
            return new int[] {colorToArgb, colorToArgb, colorToArgb, colorToArgb};
        }

        #endregion

        public static bool operator ==(Shading shading1, Shading shading2)
        {
            return shading1.Equals(shading2);
        }

        public static bool operator !=(Shading shading1, Shading shading2)
        {
            return !(shading1.Equals(shading2));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Shading)) return false;
            Shading shading = (Shading) obj;
            return Equals(type, shading.type) && Equals(values, shading.values);
        }

        public override int GetHashCode()
        {
            return type.GetHashCode() ^ values.GetHashCode();
        }
    }
}