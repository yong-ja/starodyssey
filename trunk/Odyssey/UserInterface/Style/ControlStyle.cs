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

        #region Properties

        public string Name { get; set; }

        public BorderStyle BorderStyle { get; set; }

        public int BorderSize { get; set; }

        public Shape Shape { get; set; }

        public Size Size { get; set; }

        public Padding Padding { get; set; }

        public Shading Shading { get; set; }

        public ColorArray ColorArray { get; private set; }

        public string TextStyleClass { get; set; }

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
            this.Name = name;
            this.BorderStyle = borderStyle;
            this.BorderSize = borderSize;
            this.Shape = shape;
            this.Size = size;
            this.Padding = padding;
            this.Shading = shading;
            this.ColorArray = colorArray;
            this.TextStyleClass = textStyleClass;
        }
    }
}