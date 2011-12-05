﻿using System;
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
            if (Color[0] == '#')
                Color = Color.Remove(0, 1);

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
    /// Xml Wrapper class for the LinearShader class.
    /// </summary>
    [XmlType("Gradient")]
    public class XmlGradientShader
    {
        public XmlGradientShader()
        {}

        public XmlGradientShader(IGradientShader cs)
        {
            Name = cs.Name;
            GradientType = cs.GradientType;
            if (cs.Gradient == null) return;

            if (cs.Gradient[0] == cs.Gradient[1] || cs.Gradient.Length ==1)
            {
                ColorValue = cs.Gradient[0].Color.ToArgb().ToString("X8");
            }
            else
            {
                XmlGradient = new XmlGradientStop[cs.Gradient.Length];
                for (int i = 0; i < cs.Gradient.Length; i++)
                {
                    XmlGradient[i] = new XmlGradientStop(cs.Gradient[i]);
                }
            }
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

        public virtual LinearShader ToColorShader()
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
            else
            {
                gradientColors = new GradientStop[2];
                gradientColors[0] = new GradientStop(new Color4(Int32.Parse(ColorValue,NumberStyles.HexNumber)),0);
                gradientColors[1] = gradientColors[0];
            }
            Type shaderType;
            switch (GradientType)
            {
                case GradientType.Radial:
                    shaderType = typeof (RadialShader);
                    break;
                default:
                    shaderType = typeof (LinearShader);
                    break;
            }

            return new LinearShader
            {
                Name = Name,
                GradientType = GradientType,
                Method = (Shader)Delegate.CreateDelegate
                                (typeof(Shader), shaderType.GetMethod(GradientType.ToString())),
                Gradient = gradientColors,
            };
        }
    }
 
    [XmlType("Border")]
    public class XmlBorderShader : XmlGradientShader
    {
        public XmlBorderShader()
        {}

        public XmlBorderShader(IBorderShader borderShader)
            : base(borderShader)
        {
            Borders = borderShader.Borders;
        }

        public XmlBorderShader(IGradientShader cs) : base(cs)
        {}

        [XmlIgnore]
        public Borders? Borders { get; set; }

        [XmlAttribute("Borders")]
        public string DoBstring
        {
            get {
                return Borders.HasValue
                ? Borders.ToString()
                : string.Empty;
            }
            set
            {
                Borders borders;
                Borders = Enum.TryParse(value, out borders) ? borders : UserInterface.Borders.All;
            }
        }

        public override LinearShader ToColorShader()
        {
            LinearShader cs =  base.ToColorShader();
            BorderShader bs = new BorderShader
                                  {
                                          Borders = Borders.HasValue? Borders.Value : UserInterface.Borders.All,
                                          Gradient = cs.Gradient,
                                          GradientType = cs.GradientType,
                                          Method = cs.Method,
                                          Name = cs.Name
                                  };
            return bs;
        } 
    }

    [XmlType("Radial")]
    public class XmlRadialShader : XmlGradientShader
    {
        public XmlRadialShader()
        { }

        public XmlRadialShader(RadialShader radialShader)
            : base(radialShader)
        {
            Center = XmlCommon.EncodeVector2(radialShader.Center);
            GradientType = radialShader.GradientType;
            RadiusX = radialShader.RadiusX;
            RadiusY = radialShader.RadiusY;
        }


        [XmlAttribute]
        public string Center { get; set; }
        [XmlAttribute]
        public float RadiusX { get; set; }
        [XmlAttribute]
        public float RadiusY { get; set; }

        public override LinearShader ToColorShader()
        {
            const float defaultValue = 0.5f;
            Vector2 defaultCenter = new Vector2(defaultValue, defaultValue);
            LinearShader linearShader = base.ToColorShader();
            RadialShader radialShader = new RadialShader
            {
                Center = string.IsNullOrEmpty(Center) ? defaultCenter  : XmlCommon.DecodeFloatVector2(Center),
                RadiusX = RadiusX == 0 ? defaultValue : RadiusX,
                RadiusY = RadiusY == 0 ? defaultValue : RadiusY,
                Gradient = linearShader.Gradient,
                GradientType = linearShader.GradientType,
                Method = linearShader.Method,
                Name = linearShader.Name
            };
            return radialShader;
        }
    }
}

