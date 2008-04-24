#region Disclaimer

/* 
 * Padding
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

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public struct Padding
    {
        int bottom;
        int left;
        int right;
        bool symmetricPadding;
        int top;

        public Padding(int top, int right, int bottom, int left)
        {
            symmetricPadding = false;
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public Padding(int padding)
        {
            symmetricPadding = true;
            left = top = right = bottom = padding;
        }

        public int All
        {
            get
            {
                if (symmetricPadding)
                    return top;
                else
                    return -1;
            }
        }

        public int Vertical
        {
            get { return top + bottom; }
        }

        public int Horizontal
        {
            get { return left + right; }
        }

        public int Left
        {
            get { return left; }
        }

        public int Top
        {
            get { return top; }
        }

        public int Right
        {
            get { return right; }
        }

        public int Bottom
        {
            get { return bottom; }
        }

        public static Padding Empty
        {
            get { return new Padding(0); }
        }

        public static Padding Default
        {
            get { return new Padding(5); }
        }

        public bool IsEmpty
        {
            get
            {
                if (All == 0 || (top == 0 && right == 0 && bottom == 0 && left == 0))
                    return true;
                else
                    return false;
            }
        }


        public override bool Equals(object obj)
        {
            if (!(obj is Padding)) return false;
            Padding padding = (Padding) obj;
            if (top == padding.top && right == padding.right &&
                bottom == padding.bottom && left == padding.left)
                return true;
            else
                return false;
        }


        public override int GetHashCode()
        {
            int result = top ^ right ^ bottom ^ left;
            return result;
        }

        public static bool operator ==(Padding padding1, Padding padding2)
        {
            return padding1.Equals(padding2);
        }

        public static bool operator !=(Padding padding1, Padding padding2)
        {
            return !padding1.Equals(padding2);
        }
    }
}