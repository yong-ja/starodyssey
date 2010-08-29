#region Disclaimer

/* 
 * ColorArray
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

using System;
using System.Drawing;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public enum ColorIndex
    {
        Enabled = 0,
        Highlighted = 1,
        Clicked = 2,
        Disabled = 3,
        Selected = 4,
        Focused = 5,
        BorderEnabled = 6,
        BorderHighlighted = 7
    }

    public struct ColorArray : IEquatable<ColorArray>
    {
        static readonly Color4 Zero = new Color4(0);
        internal const int ColorCount = 8;
        readonly Color4[] colorArray;

        #region Properties

        public static ColorArray Transparent
        {
            get
            {
                Color4[] colors = new Color4[ColorCount];
                for (int i = 0; i < colors.Length; i++)
                    colors[i] = Zero;

                return new ColorArray(colors);
            }
        }

        public Color4 Enabled
        {
            get { return this[ColorIndex.Enabled]; }
            private set { this[ColorIndex.Enabled] = value; }
        }

        public Color4 Highlighted
        {
            get { return this[ColorIndex.Highlighted]; }
            private set { this[ColorIndex.Highlighted] = value; }
        }

        public Color4 Clicked
        {
            get { return this[ColorIndex.Clicked]; }
            private set { this[ColorIndex.Clicked] = value; }
        }

        public Color4 Disabled
        {
            get { return this[ColorIndex.Disabled]; }
            private set { this[ColorIndex.Disabled] = value; }
        }

        public Color4 Focused
        {
            get { return this[ColorIndex.Focused]; }
            private set { this[ColorIndex.Focused] = value; }
        }

        public Color4 Selected
        {
            get { return this[ColorIndex.Selected]; }
            private set { this[ColorIndex.Selected] = value; }
        }

        public Color4 BorderEnabled
        {
            get { return this[ColorIndex.BorderEnabled]; }
            private set { this[ColorIndex.BorderEnabled] = value; }
        }

        public Color4 BorderHighlighted
        {
            get { return this[ColorIndex.BorderHighlighted]; }
            private set { this[ColorIndex.BorderHighlighted] = value; }
        }

        public Color4 this[ColorIndex colorIndex]
        {
            get { return colorArray[(int) colorIndex]; }
            private set { colorArray[(int) colorIndex] = value; }
        }

        public bool IsEmpty
        {
            get
            {
                bool result = false;
                for (int i = 0; i < ColorCount; i++)
                {
                    if (colorArray[i] != Zero) continue;
                    result = true;
                    break;
                }

                return result;
            }
        }

        #endregion

        public ColorArray(Color4[] colors)
        {
            if (colors.Length != ColorCount)
                throw Error.IndexNotPresentInArray("ColorArray", colors.Length);

            colorArray = colors;
        }

        //public ColorArray(Color4 enabled, Color4 highlighted, Color4 clicked, Color4 disabled, Color4 selected,
        //                  Color4 focused, Color4 borderEnabled, Color4 borderHighlighted)
        //{
        //    ColorArray = new Color4[Color44Count];
        //    Enabled = enabled;
        //    Highlighted = highlighted;
        //    Clicked = clicked;
        //    Disabled = disabled;
        //    Selected = selected;
        //    Focused = focused;
        //    BorderEnabled = borderEnabled;
        //    BorderHighlighted = borderHighlighted;
        //}


        #region IEquatable
        public static bool operator ==(ColorArray colorArray1, ColorArray colorArray2)
        {
            return colorArray1.Equals(colorArray2);
        }

        public static bool operator !=(ColorArray colorArray1, ColorArray colorArray2)
        {
            return !colorArray1.Equals(colorArray2);
        }

        public bool Equals(ColorArray other)
        {

            bool result = true;
            for (int i = 0; i < ColorCount; i++)
            {
                if (!colorArray[i].Equals(other.colorArray[i]))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(ColorArray)) return false;
            return Equals((ColorArray)obj);
        }

        public override int GetHashCode()
        {
            return (colorArray != null ? colorArray.GetHashCode() : 0);
        } 
        #endregion
    }
}