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

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public enum ColorIndex : int
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

    public struct ColorArray
    {
        internal const int ColorCount = 8;
        Color[] colorArray;

        #region Properties

        public static ColorArray Transparent
        {
            get
            {
                Color[] colors = new Color[ColorCount];
                for (int i = 0; i < colors.Length; i++)
                    colors[i] = Color.Transparent;

                return new ColorArray(colors);
            }
        }

        public Color Enabled
        {
            get { return this[ColorIndex.Enabled]; }
        }

        public Color Highlighted
        {
            get { return this[ColorIndex.Highlighted]; }
        }

        public Color Clicked
        {
            get { return this[ColorIndex.Clicked]; }
        }

        public Color Disabled
        {
            get { return this[ColorIndex.Disabled]; }
        }

        public Color Focused
        {
            get { return this[ColorIndex.Focused]; }
        }

        public Color Selected
        {
            get { return this[ColorIndex.Selected]; }
        }

        public Color BorderEnabled
        {
            get { return this[ColorIndex.BorderEnabled]; }
        }

        public Color BorderHighlighted
        {
            get { return this[ColorIndex.BorderHighlighted]; }
        }

        public Color this[ColorIndex colorIndex]
        {
            get { return colorArray[(int) colorIndex]; }
            internal set { colorArray[(int) colorIndex] = value; }
        }

        public bool IsEmpty
        {
            get
            {
                bool result = false;
                for (int i = 0; i < ColorCount; i++)
                {
                    if (colorArray[i].IsEmpty)
                    {
                        result = true;
                        break;
                    }
                }

                return result;
            }
        }

        #endregion

        public ColorArray(Color[] colors)
        {
            if (colors.Length != ColorCount)
                throw new ArgumentException(string.Format("The color array does not hold {0} elements.", ColorCount));

            colorArray = colors;
        }

        public ColorArray(Color enabled, Color highlighted, Color clicked, Color disabled, Color selected,
                          Color focused, Color borderEnabled, Color borderHighlighted)
        {
            colorArray = new Color[ColorCount];
            this[ColorIndex.Enabled] = enabled;
            this[ColorIndex.Highlighted] = highlighted;
            this[ColorIndex.Clicked] = clicked;
            this[ColorIndex.Disabled] = disabled;
            this[ColorIndex.Selected] = selected;
            this[ColorIndex.Focused] = focused;
            this[ColorIndex.BorderEnabled] = borderEnabled;
            this[ColorIndex.BorderHighlighted] = borderHighlighted;
        }

        public override int GetHashCode()
        {
            int result = 0;
            for (int i = 0; i < ColorCount; i++)
            {
                result ^= colorArray[i].GetHashCode();
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ColorArray))
                return false;

            ColorArray otherColorArray = (ColorArray) obj;


            bool result = true;
            for (int i = 0; i < ColorCount; i++)
            {
                if (!colorArray[i].Equals(otherColorArray.colorArray[i]))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public static bool operator ==(ColorArray colorArray1, ColorArray colorArray2)
        {
            return colorArray1.Equals(colorArray2);
        }

        public static bool operator !=(ColorArray colorArray1, ColorArray colorArray2)
        {
            return !colorArray1.Equals(colorArray2);
        }
    }
}