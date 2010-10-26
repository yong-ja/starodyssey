#region Disclaimer

/* 
 * XmlCommon
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
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;


namespace AvengersUtd.Odyssey.UserInterface.Xml
{
    /// <summary>
    /// Xml wrapper class for the abstract BaseControl class.
    /// </summary>
    public abstract class XmlBaseControl
    {
        protected XmlBaseControl()
        {
            Id = string.Empty;
            PositionString = string.Empty;
            SizeString = string.Empty;
        }

        protected XmlBaseControl(BaseControl control)
        {
            if (control == null)
                throw Error.InCreatingFromObject("control", GetType(), typeof (BaseControl));

            Id = control.Id;
            Position = control.Position;
            Size = control.Size;
            PositionString = (control.Position != Vector2.Zero) ? XmlCommon.EncodeVector2(control.Position) : string.Empty;
            SizeString = (control.Size != Size.Empty) ? XmlCommon.EncodeSize(control.Size) : string.Empty;

            IsEnabled = control.IsEnabled;
            IsVisible = control.IsVisible;
            TextStyleClass = control.TextDescriptionClass;
            ControlDescriptionClass = control.ControlDescriptionClass;

        }

        [XmlAttribute]
        [Category("Design")]
        public string Id { get; set; }

        [XmlAttribute("Position")]
        [Category("Layout")]
        public string PositionString { get; set; }

        [XmlAttribute("Size")]
        [Category("Layout")]
        public string SizeString { get; set; }

        [XmlIgnore]
        internal Vector2 Position { get; private set; }
        [XmlIgnore]
        internal Size Size { get; private set;}


        [XmlAttribute]
        [Category("Design")]
        public bool IsEnabled { get; set; }

        [XmlAttribute]
        [Category("Design")]
        public bool IsVisible { get; set; }

        [XmlAttribute]
        [Category("Appearance")]
        public string TextStyleClass { get; set; }

        [XmlAttribute]
        [Category("Appearance")]
        public string ControlDescriptionClass { get; set; }


        public virtual void WriteCSharpCode(StringBuilder sb)
        {
            Type ctlType = UIParser.GetControlTypeForWrapper(GetType());
            string typeName = ctlType.Name;
            sb.AppendFormat("{0} {1} = new {0}\n", typeName, Id);
            sb.Append("\t{\n");
            sb.AppendFormat("\t\tId = \"{0}\",\n", Id);
            sb.AppendFormat("\t\tControlDescriptionClass = \"{0}\",\n", ControlDescriptionClass);
            if (Position != Vector2.Zero)
                sb.AppendFormat("\t\tPosition = new Vector2({0},{1}),\n", Position.X, Position.Y);
            if (Size != Size.Empty)
                sb.AppendFormat("\t\tSize = new Size({0},{1}),\n", Size.Width, Size.Height);

            WriteCustomCsCode(sb);

            sb.Append("\t};\n\n");
        }

        protected abstract void WriteCustomCsCode(StringBuilder sb);
    }

    /// <summary>
    /// Contains static methods needed for the serialization/deserialization
    /// process.
    /// </summary>
    internal static class XmlCommon
    {
        internal static void Parse(string text, out int val1, out int val2)
        {
            string[] values = text.Split(',');
            val1 = Int16.Parse(values[0]);
            val2 = Int16.Parse(values[1]);
        }

        internal static Vector3 DecodeVector3(string s)
        {
            Regex regex = new Regex(@"X:\s?(?<x>\d+)\s?Y:\s?(?<y>\d+)\s?Z:\s?(?<z>\d+)\s?");
            Match m = regex.Match(s);
            int x = Int16.Parse(m.Groups["x"].Value, CultureInfo.InvariantCulture);
            int y = Int16.Parse(m.Groups["y"].Value, CultureInfo.InvariantCulture);
            int z = Int16.Parse(m.Groups["z"].Value, CultureInfo.InvariantCulture);

            return new Vector3(x, y,z);
        }

        internal static Vector2 DecodeVector2(string s)
        {
            Regex regex = new Regex(@"(?<x>\d+),(?<y>\d+)");
            Match m = regex.Match(s);
            int x = Int16.Parse(m.Groups["x"].Value, CultureInfo.InvariantCulture);
            int y = Int16.Parse(m.Groups["y"].Value, CultureInfo.InvariantCulture);

            return new Vector2(x, y);
        }

        internal static Size DecodeSize(string s)
        {
            Regex regex = new Regex(@"Width:\s?(?<width>\d+)\s?Height:\s?(?<height>\d+)");
            Match m = regex.Match(s);
            int width = Int16.Parse(m.Groups["width"].Value, CultureInfo.InvariantCulture);
            int height = Int16.Parse(m.Groups["height"].Value, CultureInfo.InvariantCulture);

            return new Size(width, height);
        }

        internal static string EncodeVector2(Vector2 v)
        {
                return string.Format(CultureInfo.InvariantCulture, "X:{0:F0} Y:{1:F0}", v.X, v.Y);
        }

        internal static string EncodeVector3(Vector3 v)
        {
                return string.Format(CultureInfo.InvariantCulture, "X:{0:F0} Y:{1:F0} Z:{2:F0}", v.X, v.Y, v.Z);
        }

        internal static string EncodeSize(Size size)
        {
            return string.Format(CultureInfo.InvariantCulture, "Width:{0:D0} Height:{1:D0}", size.Width, size.Height);
        }

        internal static string EncodeThickness(Thickness padding)
        {
            return string.Format(CultureInfo.InvariantCulture,
                                 "{0} {1} {2} {3}", padding.Top, padding.Right, padding.Bottom, padding.Left);
        }

        internal static Thickness DecodeThickness(string xmlPadding)
        {
            int value;
            
            if (Int32.TryParse(xmlPadding, out value))
                return new Thickness(value);

            Regex regex = new Regex(@"\s?(?<top>\d+)\s?(?<right>\d+)\s?(?<bottom>\d+)\s?(?<left>\d+)");
            Match m = regex.Match(xmlPadding);
            int top = Int16.Parse(m.Groups["top"].Value, CultureInfo.InvariantCulture);
            int right = Int16.Parse(m.Groups["right"].Value, CultureInfo.InvariantCulture);
            int bottom = Int16.Parse(m.Groups["bottom"].Value, CultureInfo.InvariantCulture);
            int left = Int16.Parse(m.Groups["left"].Value, CultureInfo.InvariantCulture);
            
            return new Thickness
                       {
                           Bottom = bottom,
                           Left = left,
                           Right = right,
                           Top = top
                       }; 
        }
    }
}