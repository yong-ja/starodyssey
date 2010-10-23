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

        public XmlGradientStop(GradientStop gradientStop) :this()
        {
            Color = gradientStop.Color.ToArgb().ToString("X8");
            Offset = gradientStop.Offset;
        }

    }

    /// <summary>
    /// Xml Wrapper class for the ColorShader class.
    /// </summary>
    [XmlType("Gradient")]
    public class XmlColorShader
    {
        public XmlColorShader()
        {}

        public XmlColorShader(ColorShader cs)
        {
            Name = cs.Name;
            GradientType = cs.GradientType;
            if (cs.Gradient != null)
            {
                XmlGradient = new XmlGradientStop[cs.Gradient.Length];
                for (int i = 0; i < cs.Gradient.Length; i++)
                {
                    XmlGradient[i] = new XmlGradientStop(cs.Gradient[i]);
                }
            }
            ColorValue = cs.Color != default(Color4) ? cs.Color.ToArgb().ToString("X8"): string.Empty;
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public GradientType GradientType { get; set; }
        
        [XmlAttribute("Color")]
        public string ColorValue { get; set; }

        [XmlArray("Gradient")]
        [XmlArrayItem("GradientStop")]
        public XmlGradientStop[] XmlGradient { get; set; }

        public virtual ColorShader ToColorShader()
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
                Name = Name,
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
 
    [XmlType("Border")]
    public class XmlBorderShader : XmlColorShader
    {
        public XmlBorderShader()
        {}

        public XmlBorderShader(BorderShader borderShader)
            : base(borderShader)
        {
            Borders = borderShader.Borders;
        }

        public XmlBorderShader(ColorShader cs) : base(cs)
        {}

        [XmlAttribute]
        public Borders Borders { get; set; }

        public override ColorShader ToColorShader()
        {
            ColorShader cs =  base.ToColorShader();
            BorderShader bs = new BorderShader
                                  {
                                          Borders = Borders,
                                          Color = cs.Color,
                                          Gradient = cs.Gradient,
                                          GradientType = cs.GradientType,
                                          Method = cs.Method,
                                          Name = cs.Name
                                  };
            return bs;
        } 
    }
}

