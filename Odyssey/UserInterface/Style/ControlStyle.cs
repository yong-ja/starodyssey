#region Disclaimer

/* 
 * ControlStyle
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

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public class ControlStyle
    {
        public const int DefaultBorderSize = 2;
        internal const string Error = "Error";
        internal const string Empty = "Empty";

        int borderSize;
        BorderStyle borderStyle;
        ColorArray colorArray;
        string name;
        Padding padding;
        Shading shading;
        Shape shape;
        Size size;
        string textStyleClass;

        #region Properties

        public string Name
        {
            get { return name; }
        }

        public BorderStyle BorderStyle
        {
            get { return borderStyle; }
        }

        public int BorderSize
        {
            get { return borderSize; }
        }

        public Shape Shape
        {
            get { return shape; }
        }

        public Size Size
        {
            get { return size; }
        }

        public Padding Padding
        {
            get { return padding; }
        }

        public Shading Shading
        {
            get { return shading; }
        }

        public ColorArray ColorArray
        {
            get { return colorArray; }
        }

        public string TextStyleClass
        {
            get { return textStyleClass; }
        }

        public static ControlStyle EmptyStyle
        {
            get
            {
                return new ControlStyle(Empty,
                                        Size.Empty,
                                        Padding.Empty,
                                        BorderStyle.NotSet,
                                        0,
                                        Shape.None,
                                        new Shading(ShadingType.RectangleFlat),
                                        ColorArray.Transparent,
                                        TextStyle.DefaultStyle.Name);
            }
        }

        #endregion

        public ControlStyle(string name, Size size, Padding padding, BorderStyle borderStyle, int borderSize,
                            Shape shape, Shading shading, ColorArray colorArray, string textStyleClass)
        {
            this.name = name;
            this.borderStyle = borderStyle;
            this.borderSize = borderSize;
            this.shape = shape;
            this.size = size;
            this.padding = padding;
            this.shading = shading;
            this.colorArray = colorArray;
            this.textStyleClass = textStyleClass;
        }
    }
}