#region Disclaimer

/* 
 * DataViewerCellStyle
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

#region Using directives

using System;
using System.Drawing;
using System.Globalization;

#endregion

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public class DataViewerCellStyle : ICloneable
    {
        // [rgn] Private fields 

        Border borders;
        int borderSize;
        BorderStyle borderStyle;
        ColorArray cellColorArray;
        string format;
        IFormatProvider formatProvider;
        HorizontalAlignment horizontalAlignment;
        string name;
        Padding padding;
        TextStyle textStyle;
        VerticalAlignment verticalAlignment;

        #region [rgn] Public properties 

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return verticalAlignment; }
            set { verticalAlignment = value; }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set { horizontalAlignment = value; }
        }

        public TextStyle TextStyle
        {
            get { return textStyle; }
            set { textStyle = value; }
        }

        public string Format
        {
            get { return format; }
            set { format = value; }
        }

        public Padding Padding
        {
            get { return padding; }
            set { padding = value; }
        }

        public IFormatProvider FormatProvider
        {
            get { return formatProvider; }
            set { formatProvider = value; }
        }

        public ColorArray CellColorArray
        {
            get { return cellColorArray; }
            set { cellColorArray = value; }
        }

        public int BorderSize
        {
            get { return borderSize; }
            set { borderSize = value; }
        }

        public BorderStyle BorderStyle
        {
            get { return borderStyle; }
            set { borderStyle = value; }
        }

        public Border Borders
        {
            get { return borders; }
            set { borders = value; }
        }

        public bool IsFormatProviderDefault
        {
            get { return formatProvider == CultureInfo.CurrentUICulture; }
        }

        #endregion [rgn]

        #region [rgn] Constructors 

        public DataViewerCellStyle(DataViewerCellStyle dataViewerCellStyle)
        {
            name = dataViewerCellStyle.name;
            horizontalAlignment = dataViewerCellStyle.HorizontalAlignment;
            verticalAlignment = dataViewerCellStyle.VerticalAlignment;
            cellColorArray = dataViewerCellStyle.CellColorArray;
            format = dataViewerCellStyle.Format;
            formatProvider = dataViewerCellStyle.FormatProvider;
            textStyle = dataViewerCellStyle.TextStyle;
            padding = dataViewerCellStyle.Padding;
            borderSize = dataViewerCellStyle.BorderSize;
            borderStyle = dataViewerCellStyle.BorderStyle;
            borders = dataViewerCellStyle.Borders;
        }

        public DataViewerCellStyle()
        {
            formatProvider = CultureInfo.CurrentUICulture;
            cellColorArray = new ColorArray(
                Color.Empty,
                Color.Empty,
                Color.Empty,
                Color.Empty,
                Color.Empty,
                Color.Empty,
                Color.Gray,
                Color.Gray);

            borderStyle = BorderStyle.Flat;
            borderSize = 2;
            borders = Border.All;
            textStyle = TextStyle.DefaultStyle;
            padding = new Padding(2);
        }

        #endregion [rgn]

        #region ICloneable Members

        public object Clone()
        {
            return new DataViewerCellStyle(this);
        }

        #endregion

        public void MergeStyle(DataViewerCellStyle otherCellStyle)
        {
            if (horizontalAlignment == HorizontalAlignment.NotSet)
                horizontalAlignment = otherCellStyle.HorizontalAlignment;

            if (verticalAlignment == VerticalAlignment.NotSet)
                verticalAlignment = otherCellStyle.VerticalAlignment;

            if (string.IsNullOrEmpty(format))
                format = otherCellStyle.Format;

            if (TextStyle == null)
                textStyle = otherCellStyle.TextStyle;

            if (IsFormatProviderDefault)
                formatProvider = otherCellStyle.FormatProvider;

            if (padding.IsEmpty)
                padding = otherCellStyle.Padding;

            if (borderSize == 0)
                borderSize = otherCellStyle.BorderSize;

            if (borderStyle == BorderStyle.NotSet)
                borderStyle = otherCellStyle.BorderStyle;

            if (cellColorArray.IsEmpty)
                cellColorArray = otherCellStyle.CellColorArray;

            if (borderStyle != BorderStyle.None)
                borders |= otherCellStyle.Borders;
        }

        string GenerateIdentifier()
        {
            int result = borderStyle.GetHashCode() ^ borderSize.GetHashCode() ^ padding.GetHashCode() ^
                         cellColorArray.GetHashCode() ^ textStyle.GetHashCode();

            return string.Format(CultureInfo.InvariantCulture, "{0:x}", result);
        }


        internal ControlStyle ToControlStyle()
        {
            string identifier = GenerateIdentifier();
            if (StyleManager.ContainsControlStyle(GenerateIdentifier()))
                return StyleManager.GetControlStyle(identifier);
            else
            {
                ControlStyle newControlStyle = new ControlStyle(
                    identifier,
                    Size.Empty,
                    padding,
                    borderStyle,
                    borderSize,
                    Shape.Rectangle,
                    new Shading(ShadingType.RectangleFlat),
                    cellColorArray,
                    textStyle.Name);

                StyleManager.AddControlStyle(newControlStyle);
                if (!StyleManager.ContainsTextStyle(textStyle.Name))
                    StyleManager.AddTextStyle(textStyle);

                return newControlStyle;
            }
        }
    }
}