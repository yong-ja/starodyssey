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
    public enum StateIndex
    {
        Enabled = 0,
        Highlighted = 1,
        Pressed = 2,
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
            get { return this[StateIndex.Enabled]; }
            private set { this[StateIndex.Enabled] = value; }
        }

        public Color4 Highlighted
        {
            get { return this[StateIndex.Highlighted]; }
            private set { this[StateIndex.Highlighted] = value; }
        }

        public Color4 Clicked
        {
            get { return this[StateIndex.Pressed]; }
            private set { this[StateIndex.Pressed] = value; }
        }

        public Color4 Disabled
        {
            get { return this[StateIndex.Disabled]; }
            private set { this[StateIndex.Disabled] = value; }
        }

        public Color4 Focused
        {
            get { return this[StateIndex.Focused]; }
            private set { this[StateIndex.Focused] = value; }
        }

        public Color4 Selected
        {
            get { return this[StateIndex.Selected]; }
            private set { this[StateIndex.Selected] = value; }
        }

        public Color4 BorderEnabled
        {
            get { return this[StateIndex.BorderEnabled]; }
            private set { this[StateIndex.BorderEnabled] = value; }
        }

        public Color4 BorderHighlighted
        {
            get { return this[StateIndex.BorderHighlighted]; }
            private set { this[StateIndex.BorderHighlighted] = value; }
        }

        public Color4 this[StateIndex stateIndex]
        {
            get { return colorArray[(int) stateIndex]; }
            private set { colorArray[(int) stateIndex] = value; }
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
        //    Pressed = clicked;
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