#region Disclaimer

/* 
 * XmlControlStyle
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
using System.Globalization;
using System.Xml.Serialization;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odyssey.UserInterface.Xml
{
    /// <summary>
    /// Xml Wrapper class for the ShadingValues class.
    /// </summary>
    [Serializable]
    [XmlType(TypeName ="ShadingValues")]
    public class XmlShadingValues
    {
        float darkFactor;
        float lightFactor;

        public XmlShadingValues()
        {
            lightFactor = Shading.DefaultLightFactor;
            darkFactor = Shading.DefaultDarkFactor;
        }

        public XmlShadingValues(ShadingValues values)
        {
            lightFactor = values.LightFactor;
            darkFactor = values.DarkFactor;
        }

        [XmlAttribute]
        public float LightFactor
        {
            get { return lightFactor; }
            set { lightFactor = value; }
        }

        [XmlAttribute]
        public float DarkFactor
        {
            get { return darkFactor; }
            set { darkFactor = value; }
        }

        public ShadingValues ToShadingValues()
        {
            return new ShadingValues(lightFactor, darkFactor);
        }
    }

    /// <summary>
    /// Xml Wrapper class for the Shading class.
    /// </summary>
    [XmlType(TypeName="Shading")]
    [Serializable]
    public class XmlShading
    {
        ShadingType type;
        XmlShadingValues xmlValues;

        public XmlShading()
        {
            type = ShadingType.RectangleFlat;
            xmlValues = new XmlShadingValues();
        }

        public XmlShading(Shading shading)
        {
            type = shading.Type;
            xmlValues = new XmlShadingValues(shading.Values);
        }

        [XmlAttribute]
        public ShadingType Type
        {
            get { return type; }
            set { type = value; }
        }

        [XmlElement("Values")]
        public XmlShadingValues XmlValues
        {
            get { return xmlValues; }
            set { xmlValues = value; }
        }

        public Shading ToShading()
        {
            return new Shading(type, xmlValues.LightFactor, xmlValues.DarkFactor);
        }
    }


    /// <summary>
    /// Xml Wrapper class for the System.Drawing.Color class.
    /// </summary>
    [Serializable]
    [XmlType(TypeName = "Color")]
    public class XmlColor
    {
        string color;
        ColorIndex colorIndex;
        string name;

        public XmlColor()
        {
            colorIndex = ColorIndex.Enabled;
            color = string.Format("{0:X8}", Color.Transparent.ToArgb());
            name = "Unknown";
        }

        public XmlColor(ColorIndex colorIndex, Color color)
        {
            this.colorIndex = colorIndex;
            this.color = string.Format("{0:X8}", color.ToArgb());

            if (color.IsNamedColor)
                name = color.Name;
            else
                name = "Custom";
        }

        [XmlAttribute("Type")]
        public ColorIndex ColorIndex
        {
            get { return colorIndex; }
            set { colorIndex = value; }
        }

        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlAttribute("HexValue")]
        public string ColorValue
        {
            get { return color; }
            set { color = value; }
        }

        public Color ToColor()
        {
            int argbColor = Int32.Parse(color, NumberStyles.HexNumber);
            if (argbColor == 0)
                return Color.Empty;
            else
                return Color.FromArgb(argbColor);
        }
    }


    /// <summary>
    /// Xml Wrapper class for the ColorArray class.
    /// </summary>
    [Serializable]
    [XmlType(TypeName = "ColorArray")]
    public class XmlColorArray
    {
        XmlColor[] xmlColorArray;

        public XmlColorArray()
        {
            xmlColorArray = new XmlColor[ColorArray.ColorCount];
        }

        public XmlColorArray(ColorArray colorArray) :
            this()
        {
            for (int i = 0; i < ColorArray.ColorCount; i++)
            {
                xmlColorArray[i] = new XmlColor((ColorIndex) i, colorArray[(ColorIndex) i]);
            }
        }

        [XmlElement("Color")]
        public XmlColor[] XmlColorArrayList
        {
            get { return xmlColorArray; }
            set { xmlColorArray = value; }
        }

        public ColorArray ToColorArray()
        {
            Color[] colors = new Color[ColorArray.ColorCount];
            for (int i = 0; i < ColorArray.ColorCount; i++)
            {
                colors[i] = xmlColorArray[i].ToColor();
            }

            return new ColorArray(colors);
        }
    }

    /// <summary>
    /// Xml Wrapper class for the Control Style class.
    /// </summary>
    [Serializable]
    [XmlType(TypeName = "ControlStyle")]
    public class XmlControlStyle
    {
        int borderSize;
        BorderStyle borderStyle;
        string name;
        Shape shape;
        string textStyleClass;

        XmlColorArray xmlColorArray;
        string xmlPadding;
        XmlShading xmlShading;
        string xmlSize;


        public XmlControlStyle()
        {
            name = string.Empty;
            shape = Shape.None;
            borderStyle = BorderStyle.None;
            xmlSize = string.Empty;
            xmlColorArray = new XmlColorArray();
        }

        public XmlControlStyle(ControlStyle controlStyle) :
            this()
        {
            if (controlStyle == null)
                throw Error.InCreatingFromObject("controlStyle", GetType(), typeof (ControlStyle));

            name = controlStyle.Name;
            borderSize = controlStyle.BorderSize;
            borderStyle = controlStyle.BorderStyle;
            shape = controlStyle.Shape;
            xmlSize = XmlCommon.EncodeSize(controlStyle.Size);
            xmlPadding = XmlCommon.EncodePadding(controlStyle.Padding);
            textStyleClass = controlStyle.TextStyleClass;
            xmlColorArray = new XmlColorArray(controlStyle.ColorArray);

            xmlShading = new XmlShading(controlStyle.Shading);
        }

        #region Properties

        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlAttribute]
        public Shape Shape
        {
            get { return shape; }
            set { shape = value; }
        }

        [XmlAttribute("Size")]
        public string XmlSize
        {
            get { return xmlSize; }
            set { xmlSize = value; }
        }

        [XmlAttribute]
        public BorderStyle BorderStyle
        {
            get { return borderStyle; }
            set { borderStyle = value; }
        }

        [XmlAttribute]
        public int BorderSize
        {
            get { return borderSize; }
            set { borderSize = value; }
        }

        [XmlAttribute("Padding")]
        public string XmlPadding
        {
            get { return xmlPadding; }
            set { xmlPadding = value; }
        }

        [XmlAttribute]
        public string TextStyleClass
        {
            get { return textStyleClass; }
            set { textStyleClass = value; }
        }

        [XmlElement("Shading")]
        public XmlShading XmlShading
        {
            get { return xmlShading; }
            set { xmlShading = value; }
        }

        [XmlElement("ColorArray")]
        public XmlColorArray XmlColorArray
        {
            get { return xmlColorArray; }
            set { xmlColorArray = value; }
        }

        public ControlStyle ToControlStyle()
        {
            ControlStyle cStyle = new ControlStyle(
                name,
                String.IsNullOrEmpty(xmlSize) ? Size.Empty : XmlCommon.DecodeSize(xmlSize),
                String.IsNullOrEmpty(xmlPadding) ? Padding.Empty : XmlCommon.DecodePadding(xmlPadding),
                borderStyle,
                borderSize,
                shape,
                xmlShading.ToShading(),
                xmlColorArray.ToColorArray(),
                string.IsNullOrEmpty(textStyleClass) ? "Default" : textStyleClass);
            return cStyle;
        }

        #endregion
    }

    //[Serializable]
    //[XmlType(TypeName = "TableStyle")]
    /*
    public class XmlTableStyle : XmlControlStyle
    {
        string xmlCellPadding;
        int cellSpacingX;
        int cellSpacingY;
        Border tableBorders;

        public XmlTableStyle(TableStyle tableStyle):
            base(tableStyle)
        {
            xmlCellPadding = XmlCommon.EncodePadding(tableStyle.Cellpadding);
            cellSpacingX = tableStyle.CellSpacingX;
            cellSpacingY = tableStyle.CellSpacingY;
            tableBorders = tableStyle.TableBorders;
        }

        [XmlAttribute("CellPadding")]
        public string XmlCellPadding
        {
            get { return xmlCellPadding; }
            set { xmlCellPadding = value; }
        }

        [XmlAttribute]
        public int CellSpacingX
        {
            get { return cellSpacingX; }
            set { cellSpacingX = value; }
        }

        [XmlAttribute]
        public int CellSpacingY
        {
            get { return cellSpacingY; }
            set { cellSpacingY = value; }
        }

        public Border TableBorders
        {
            get { return tableBorders; }
            set { tableBorders = value; }
        }

        public TableStyle ToTableStyle()
        {
            TableStyle tableStyle = new TableStyle(
                Name,
                XmlCommon.DecodeSize(XmlSize),
                XmlCommon.DecodePadding(XmlPadding),
                XmlCommon.DecodePadding(xmlCellPadding),
                cellSpacingX,
                cellSpacingY,
                tableBorders,
                BorderStyle,
                BorderSize,
                XmlShading.ToShading(),
                XmlColorArray.ToColorArray());

            return tableStyle;
        }
    }
     * */
}