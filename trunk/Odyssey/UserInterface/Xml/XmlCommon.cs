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
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface.Xml
{
    /// <summary>
    /// Xml wrapper class for the abstract BaseControl class.
    /// </summary>
    public abstract class XmlBaseControl
    {
        string controlStyleClass;
        string id;


        bool isEnabled;
        bool isVisible;
        string textStyleClass;
        string xmlPosition;
        string xmlSize;

        protected XmlBaseControl()
        {
            id = string.Empty;
            xmlPosition = string.Empty;
            xmlSize = string.Empty;
        }

        [XmlAttribute]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [XmlAttribute("Position")]
        public virtual string XmlPosition
        {
            get { return xmlPosition; }
            set { xmlPosition = value; }
        }

        [XmlAttribute("Size")]
        public string XmlSize
        {
            get { return xmlSize; }
            set { xmlSize = value; }
        }

        [XmlAttribute]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        [XmlAttribute]
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        [XmlAttribute]
        public string TextStyleClass
        {
            get { return textStyleClass; }
            set { textStyleClass = value; }
        }

        [XmlAttribute]
        public string ControlStyleClass
        {
            get { return controlStyleClass; }
            set { controlStyleClass = value; }
        }

        public virtual void FromControl(BaseControl control)
        {
            if (control == null)
                throw Error.InCreatingFromObject("control", GetType(), typeof (BaseControl));

            id = control.Id;
            xmlPosition = (control.Position.Equals(new Vector2())) ? XmlCommon.EncodeVector2(control.Position) : null;
            xmlSize = (control.Size != Size.Empty) ? XmlCommon.EncodeSize(control.Size) : null;
            isEnabled = control.IsEnabled;
            isVisible = control.IsVisible;
            textStyleClass = control.TextStyleClass;
            controlStyleClass = control.ControlStyleClass;
        }
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

        internal static Vector2 DecodeVector2(string s)
        {
            Regex regex = new Regex(@"X:\s?(?<x>\d+)\s?Y:\s?(?<y>\d+)");
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

        internal static string EncodeSize(Size Size)
        {
            return string.Format(CultureInfo.InvariantCulture, "Width:{0:D0} Height:{1:D0}", Size.Width, Size.Height);
        }

        internal static string EncodePadding(Padding padding)
        {
            return string.Format(CultureInfo.InvariantCulture,
                                 "{0} {1} {2} {3}", padding.Top, padding.Right, padding.Bottom, padding.Left);
        }

        internal static Padding DecodePadding(string xmlPadding)
        {
            Regex regex = new Regex(@"\s?(?<top>\d+)\s?(?<right>\d+)\s?(?<bottom>\d+)\s?(?<left>\d+)");
            Match m = regex.Match(xmlPadding);
            int top = Int16.Parse(m.Groups["top"].Value, CultureInfo.InvariantCulture);
            int right = Int16.Parse(m.Groups["right"].Value, CultureInfo.InvariantCulture);
            int bottom = Int16.Parse(m.Groups["bottom"].Value, CultureInfo.InvariantCulture);
            int left = Int16.Parse(m.Groups["left"].Value, CultureInfo.InvariantCulture);

            return new Padding(top, right, bottom, left);
        }
    }
}