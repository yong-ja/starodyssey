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

using System;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public struct Padding : IEquatable<Padding>
    {
        bool symmetricPadding;

        public Padding(int padding) : this()
        {
            symmetricPadding = true;
            Left = Top = Right = Bottom = padding;
        }

        public int All
        {
            get
            {
                if (symmetricPadding)
                    return Top;
                else
                    return -1;
            }
        }

        public int Vertical
        {
            get { return Top + Bottom; }
        }

        public int Horizontal
        {
            get { return Left + Right; }
        }

        public int Left { get; set; }

        public int Top { get; set; }

        public int Right { get; set; }

        public int Bottom { get; set; }

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
            get {
                return All == 0 || (Top == 0 && Right == 0 && Bottom == 0 && Left == 0);
            }
        }


        public bool Equals(Padding other)
        {
            return other.symmetricPadding.Equals(symmetricPadding) && other.Left == Left && other.Top == Top && other.Right == Right && other.Bottom == Bottom;
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                int result = symmetricPadding.GetHashCode();
                result = (result*397) ^ Left;
                result = (result*397) ^ Top;
                result = (result*397) ^ Right;
                result = (result*397) ^ Bottom;
                return result;
            }
        }

        public static bool operator ==(Padding padding1, Padding padding2)
        {
            return padding1.Equals(padding2);
        }

        public static bool operator !=(Padding padding1, Padding padding2)
        {
            return !padding1.Equals(padding2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Padding)) return false;
            return Equals((Padding) obj);
        }
    }
}