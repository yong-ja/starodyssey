#region Disclaimer

/* 
 * XmlControlDescription
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
using AvengersUtd.Odyssey.UserInterface.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using System.Reflection;

namespace AvengersUtd.Odyssey.UserInterface.Xml
{

    public struct XmlGradientStop
    {
        [XmlAttribute]
        public string Color { get; set; }
        [XmlAttribute]
        public float Offset { get; set; }

        public GradientStop ToGradientStop()
        {
            int argbColor = Int32.Parse(Color, NumberStyles.HexNumber);
            return new GradientStop(new Color4(argbColor), Offset);
        }

    }

    /// <summary>
    /// Xml Wrapper class for the ColorShader class.
    /// </summary>
    [XmlType(TypeName = "FillShader")]
    [Serializable]
    public struct XmlColorShader
    {
        [XmlAttribute]
        public GradientType GradientType { get; set; }

        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute("Color")]
        public string ColorValue { get; set; }

        [XmlArray("Gradient")]
        [XmlArrayItem("GradientStop")]
        public XmlGradientStop[] XmlFillColors { get; set; }

        public ColorShader ToColorShader()
        {
            GradientStop[] gradientColors=null;

            if (XmlFillColors != null)
            {
                gradientColors = new GradientStop[XmlFillColors.Length];

                for (int i = 0; i < XmlFillColors.Length; i++)
                {
                    gradientColors[i] = XmlFillColors[i].ToGradientStop();
                }
            }
            return new ColorShader
                       {
                               GradientType = GradientType,
                               Method = (Shader) Delegate.CreateDelegate
                                               (typeof (Shader), typeof (ColorShader).GetMethod(GradientType.ToString())),
                               Color = ColorValue != null
                                               ? new Color4(Int32.Parse(ColorValue, NumberStyles.HexNumber))
                                               : default(Color4),
                               Gradient = gradientColors,
                       };
        }
    }


    /// <summary>
    /// Xml Wrapper class for the System.Drawing.Color class.
    /// </summary>
    [Serializable]
    [XmlType(TypeName = "Color")]
    public struct XmlColor
    {

        [XmlAttribute("Type")]
        public StateIndex StateIndex { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute("HexValue")]
        public string ColorValue { get; set; }

        public Color4 ToColor4()
        {
            int argbColor = Int32.Parse(ColorValue, NumberStyles.HexNumber);
            return new Color4(argbColor);
        }
    }


    /// <summary>
    /// Xml Wrapper class for the ColorArray class.
    /// </summary>
    [Serializable]
    [XmlType(TypeName = "ColorArray")]
    public struct XmlColorArray
    {
        XmlColor[] xmlColorArray;


        public XmlColorArray(ColorArray colorArray)
        {
            xmlColorArray = new XmlColor[ColorArray.ColorCount];
            for (int i = 0; i < ColorArray.ColorCount; i++)
            {
                xmlColorArray[i] = new XmlColor
                                       {
                                           StateIndex = (StateIndex) i,
                                           ColorValue = colorArray[(StateIndex) i].ToArgb().ToString("{0:X8}")
                                       };
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
            Color4[] colors = new Color4[ColorArray.ColorCount];
            for (int i = 0; i < ColorArray.ColorCount; i++)
            {
                colors[i] = xmlColorArray[i].ToColor4();
            }

            return new ColorArray(colors);
        }
    }

    /// <summary>
    /// Xml Wrapper class for the Control Style class.
    /// </summary>
    [Serializable]
    [XmlType(TypeName = "ControlDescription")]
    public struct XmlControlDescription
    {

        public XmlControlDescription(ControlDescription controlDescription) :
            this()
        {
            if (controlDescription == null)
                throw Error.InCreatingFromObject("ControlDescription", GetType(), typeof (ControlDescription));

            Name = controlDescription.Name;
            BorderSize = controlDescription.BorderSize;
            BorderStyle = controlDescription.BorderStyle;
            Shape = controlDescription.Shape;
            XmlSize = XmlCommon.EncodeSize(controlDescription.Size);
            XmlPadding = XmlCommon.EncodePadding(controlDescription.Padding);
            TextDescriptionClass = controlDescription.TextStyleClass;
            XmlColorArray = new XmlColorArray(controlDescription.ColorArray);
        }

        #region Properties

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public Shape Shape { get; set; }

        [XmlAttribute("Size")]
        public string XmlSize { get; set; }

        [XmlAttribute]
        public BorderStyle BorderStyle { get; set; }

        [XmlAttribute]
        public int BorderSize { get; set; }

        [XmlAttribute("Padding")]
        public string XmlPadding { get; set; }

        [XmlAttribute]
        public string TextDescriptionClass { get; set; }

        [XmlElement("Fill")]
        public XmlColorShader FillShader { get; set; }

        [XmlElement("BorderEnabled")]
        public XmlColorShader BorderEnabledShader { get; set; }

        [XmlElement("ColorArray")]
        public XmlColorArray XmlColorArray { get; set; }

        public ControlDescription ToControlDescription()
        {
            return new ControlDescription
                       {
                           Name = Name,
                           Shape = Shape,
                           Size = String.IsNullOrEmpty(XmlSize) ? Size.Empty : XmlCommon.DecodeSize(XmlSize),
                           Padding = String.IsNullOrEmpty(XmlPadding) ? Thickness.Empty : XmlCommon.DecodePadding(XmlPadding),
                           BorderStyle = BorderStyle,
                           BorderSize = BorderSize,
                           FillShader = FillShader.ToColorShader(),
                           BorderShader = BorderEnabledShader.ToColorShader(),
                           ColorArray =  XmlColorArray.ToColorArray(),
                           TextStyleClass = string.IsNullOrEmpty(TextDescriptionClass) ? "Default" : TextDescriptionClass
                       };
         }

        #endregion
    }

    //[Serializable]
    //[XmlType(TypeName = "TableStyle")]
    /*
    public class XmlTableStyle : XmlControlDescription
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
                XmlCommon.DecodeSize(Size),
                XmlCommon.DecodePadding(XmlPadding),
                XmlCommon.DecodePadding(xmlCellPadding),
                cellSpacingX,
                cellSpacingY,
                tableBorders,
                BorderStyle,
                BorderSize,
                XmlColorShader.ToShading(),
                XmlColorArray.ToColorArray());

            return tableStyle;
        }
    }
     * */
}