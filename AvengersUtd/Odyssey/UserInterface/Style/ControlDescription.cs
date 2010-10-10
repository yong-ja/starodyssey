#region #Disclaimer

// /* 
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  * 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */

#endregion

#region Using directives

using System;
using System.Drawing;

#endregion

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public class ControlDescription : IEquatable<ControlDescription>
    {
        public const int DefaultBorderSize = 2;
        internal const string Error = "Error";
        internal const string Empty = "Empty";

        #region Properties

        public string Name { get; internal set; }
        public string TextStyleClass { get; internal set; }
        public BorderStyle BorderStyle { get; internal set; }
        public int BorderSize { get; set; }
        public Size Size { get; internal set; }
        public Padding Padding { get; internal set; }
        public ColorArray ColorArray { get; set; }
        public Shape Shape { get; set; }
        public ColorShader ColorShader { get; set; }

        
        public static ControlDescription EmptyDescription
        {
            get
            {
                return new ControlDescription
                           {
                               BorderSize = 0,
                               BorderStyle = BorderStyle.None,
                               ColorShader = new ColorShader(),
                               ColorArray = ColorArray.Transparent,
                               Name = Empty,
                               Padding = Padding.Empty,
                               Size = Size.Empty,
                               TextStyleClass = "Default"
                           };
            }
        }

        #endregion

        #region IEquatable<ControlDescription
       

        public static bool operator ==(ControlDescription cDesc1, ControlDescription cDesc2)
        {
            return cDesc1.Equals(cDesc2);
        }

        public static bool operator !=(ControlDescription cDesc1, ControlDescription cDesc2)
        {
            return !(cDesc1 == cDesc2);
        }


        public bool Equals(ControlDescription other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && Equals(other.TextStyleClass, TextStyleClass) && Equals(other.BorderStyle, BorderStyle) && other.BorderSize == BorderSize && other.Size.Equals(Size) && other.Padding.Equals(Padding) && other.ColorArray.Equals(ColorArray) && Equals(other.Shape, Shape) && other.ColorShader.Equals(ColorShader);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ControlDescription)) return false;
            return Equals((ControlDescription) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Name != null ? Name.GetHashCode() : 0);
                result = (result*397) ^ (TextStyleClass != null ? TextStyleClass.GetHashCode() : 0);
                result = (result*397) ^ BorderStyle.GetHashCode();
                result = (result*397) ^ BorderSize;
                result = (result*397) ^ Size.GetHashCode();
                result = (result*397) ^ Padding.GetHashCode();
                result = (result*397) ^ ColorArray.GetHashCode();
                result = (result*397) ^ Shape.GetHashCode();
                result = (result*397) ^ ColorShader.GetHashCode();
                return result;
            }
        }

        #endregion
    }
}