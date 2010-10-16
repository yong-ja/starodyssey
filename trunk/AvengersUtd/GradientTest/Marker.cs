#region Disclaimer
// /* 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com/blog
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

#region Using Directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#endregion

namespace AvengersUtd.GradientTest
{
    public class Marker
    {
        public Marker(Color color, float offset)
        {
            Color = color;
            Offset = offset;
            Selected = false;
        }

        public Color Color { get; set; }
        public float Offset { get; set; }
        public bool Selected { get; set; }

        public override string ToString()
        {
            return string.Format("{0} Offset: {1:f3}", Color, Offset);
        }

    }

    public class MarkerEventArgs : EventArgs
    {
        public MarkerEventArgs(Marker marker)
        {
            Marker = marker;
        }

        /// <summary>
        /// Gets the currently selected <see cref="GradientTest.Marker"/>.
        /// </summary>
        public Marker Marker { get; private set; }
    }
}