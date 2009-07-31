#region Disclaimer

/* 
 * UICommon
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
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odyssey.UserInterface
{
    //public delegate void MouseEventHandler(object sender, MouseEventArgs e);
    //public delegate void KeyEventHandler(object sender, KeyEventArgs e);
    //public delegate void EventHandler(EventArgs e);
    public delegate int[] ShadingMode(Color color, ShadingValues values);

    public enum HorizontalAlignment
    {
        NotSet,
        Left,
        Center,
        Right,
    }

    public enum VerticalAlignment
    {
        NotSet,
        Top,
        Center,
        Bottom
    }

    public enum BorderStyle
    {
        NotSet,
        None,
        Flat,
        Raised,
        Sunken,
    }

    public enum Shape : int
    {
        None = 0,
        Custom,
        Rectangle,
        Circle,
        LeftTrapezoidUpside,
        LeftTrapezoidDownside,
        RightTrapezoidUpside,
        RightTrapezoidDownside
    }

    [Flags]
    public enum Border : int
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8,
        All = Top | Bottom | Left | Right
    }

    public class ControlEventArgs : EventArgs
    {
        IControl control;

        public ControlEventArgs(IControl control)
        {
            this.control = control;
        }


        public IControl Control
        {
            get { return control; }
            set { control = value; }
        }
    }

    public enum IntersectionLocation
    {
        None,
        Inner,
        CornerNW,
        Top,
        CornerNE,
        Right,
        CornerSE,
        Bottom,
        CornerSW,
        Left
    }
}