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
    public class XmlColorShader
    {
        public XmlColorShader()
        {}

        public XmlColorShader(LinearShader cs)
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
            return new LinearShader
            {
                Name = Name,
                GradientType = GradientType,
                Method = (Shader)Delegate.CreateDelegate
                                (typeof(Shader), typeof(LinearShader).GetMethod(GradientType.ToString())),
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

        public XmlBorderShader(LinearShader cs) : base(cs)
        {}

        [XmlAttribute]
        public Borders Borders { get; set; }

        public override LinearShader ToColorShader()
        {
            LinearShader cs =  base.ToColorShader();
            BorderShader bs = new BorderShader
                                  {
                                          Borders = Borders,
                                          Gradient = cs.Gradient,
                                          GradientType = cs.GradientType,
                                          Method = cs.Method,
                                          Name = cs.Name
                                  };
            return bs;
        } 
    }
}

