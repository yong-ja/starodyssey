using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AvengersUtd.Odyssey.UserInterface.Drawing;
using SlimDX;

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
    public class XmlColorShader
    {
        [XmlAttribute]
        public GradientType GradientType { get; set; }

        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute("Color")]
        public string ColorValue { get; set; }

        [XmlArray("XmlGradient")]
        [XmlArrayItem("GradientStop")]
        public XmlGradientStop[] XmlGradient { get; set; }

        public ColorShader ToColorShader()
        {
            GradientStop[] gradientColors = null;

            if (XmlGradient != null)
            {
                gradientColors = new GradientStop[XmlGradient.Length];

                for (int i = 0; i < XmlGradient.Length; i++)
                {
                    gradientColors[i] = XmlGradient[i].ToGradientStop();
                }
            }
            return new ColorShader
            {
                GradientType = GradientType,
                Method = (Shader)Delegate.CreateDelegate
                                (typeof(Shader), typeof(ColorShader).GetMethod(GradientType.ToString())),
                Color = ColorValue != null
                                ? new Color4(Int32.Parse(ColorValue, NumberStyles.HexNumber))
                                : default(Color4),
                Gradient = gradientColors,
            };
        }

    }
 
    public class XmlBorderShader : XmlColorShader
    {
        [XmlAttribute]
        public Border Borders { get; set; }

        public new BorderShader ToColorShader()
        {
            BorderShader borderShader = (BorderShader)base.ToColorShader();
            borderShader.Borders = Borders;
            return borderShader;
        } 
    }
}

