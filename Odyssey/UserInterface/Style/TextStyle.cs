#region Disclaimer

/* 
 * TextStyle
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
using AvengersUtd.Odyssey.UserInterface.Properties;
#if !(SlimDX)
    using Microsoft.DirectX;
    using Microsoft.DirectX.Direct3D;
    using Font=Microsoft.DirectX.Direct3D.Font;
#else
using SlimDX.Direct3D9;
using Font = SlimDX.Direct3D9.Font;
#endif


namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public class TextStyle : ICloneable
    {
        const int DefaultFontSize = 20;

        bool applyHighlight;
        bool applyShadowing;
        Font font;
        string fontName;
        Color highlightedColor;
        HorizontalAlignment horizontalAlignment;
        bool ignoreBounds;
        bool isBold;
        bool isItalic;
        string name;
        Color selectedColor;
        int size;
        Color standardColor;
        VerticalAlignment verticalAlignment;

        #region Properties

        public string Name
        {
            get { return name; }
        }

        public bool IsBold
        {
            get { return isBold; }
        }

        public bool IsItalic
        {
            get { return isItalic; }
        }

        public bool ApplyShadowing
        {
            get { return applyShadowing; }
        }

        public Color StandardColor
        {
            get { return standardColor; }
        }

        public Color HighlightedColor
        {
            get { return highlightedColor; }
        }

        public Color SelectedColor
        {
            get { return selectedColor; }
        }

        public int Size
        {
            get { return size; }
        }

        public string FontName
        {
            get { return fontName; }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return verticalAlignment; }
        }

        public bool IgnoreBounds
        {
            get { return ignoreBounds; }
        }

        public bool ApplyHighlight
        {
            get { return applyHighlight; }
        }

        public static TextStyle DefaultStyle
        {
            get { return StyleManager.GetTextStyle("Default"); }
        }

        #endregion

        #region Constructors

        public TextStyle()
        {
            standardColor = Color.White;
            highlightedColor = Color.White;
            selectedColor = Color.White;
            size = DefaultFontSize;
            fontName = FontManager.DefaultFontName;
            horizontalAlignment = HorizontalAlignment.Left;
            verticalAlignment = VerticalAlignment.Top;
            name = GenerateId();
        }

        public TextStyle(bool bold, bool italic, int fontSize, Color color)
        {
            standardColor = highlightedColor = color;
            size = fontSize;
            fontName = FontManager.DefaultFontName;
            horizontalAlignment = HorizontalAlignment.Left;
            verticalAlignment = VerticalAlignment.Top;
            isBold = bold;
            isItalic = italic;

            name = GenerateId();
        }

        public TextStyle(string styleInfo)
            : this()
        {
            ParseMarkup(styleInfo);
            name = GenerateId();
        }

        public TextStyle(Font font)
        {
            if (font == null)
                throw new ArgumentNullException(Font.GetType().Name, Exceptions.TextStyle_FontCannotBeNull);
            FontDescription description = font.Description;
            isBold = (description.Weight != FontWeight.Normal) ? true : false;
#if (!SlimDX)
            isItalic = description.IsItalic;
#else
            isItalic = description.Italic;
#endif
            standardColor = highlightedColor = selectedColor = Color.White;
            size = description.Height;
            fontName = description.FaceName;

            name = GenerateId();
        }

        public TextStyle(string name, bool bold, bool italic, bool isShadowed, bool ignoreBounds, Color standardColor,
                         Color highlightedColor,
                         int size, string fontName, HorizontalAlignment horizontalHorizontalAlignment,
                         VerticalAlignment verticalAlignment)
        {
            this.name = name;
            isBold = bold;
            isItalic = italic;
            applyShadowing = isShadowed;
            this.ignoreBounds = ignoreBounds;
            applyHighlight = (standardColor != highlightedColor) ? true : false;
            this.standardColor = standardColor;
            this.highlightedColor = highlightedColor;
            this.size = size;
            this.fontName = fontName;
            horizontalAlignment = horizontalHorizontalAlignment;
            this.verticalAlignment = verticalAlignment;
        }

        #endregion

        public Font Font
        {
            get
            {
                if (font != null)
                    return font;
                else
                    return font = FontManager.CreateFont(fontName,
                                                         Size,
                                                         isBold ? FontWeight.Bold : FontWeight.Normal,
                                                         isItalic);
            }
            set { font = value; }
        }

        #region ICloneable Members

        public object Clone()
        {
            return new TextStyle(null, isBold, isItalic, applyShadowing, ignoreBounds, standardColor, highlightedColor,
                                 size, fontName, horizontalAlignment, verticalAlignment);
        }

        #endregion

        void ParseMarkup(string markup)
        {
            string[] commandList = markup.Split(',');

            foreach (string s in commandList)
            {
                if (s.Length == 1)
                    ParseSimpleCommand(s.ToLower(CultureInfo.InvariantCulture));
                else
                    ParseComplexCommand(s);
            }
        }

        void ParseSimpleCommand(string command)
        {
            switch (command)
            {
                case "b":
                    isBold = true;
                    break;

                case "i":
                    isItalic = true;
                    break;

                case "s":
                    applyShadowing = true;
                    break;

                default:
                    break;
            }
        }

        void ParseComplexCommand(string command)
        {
            Regex regex = new Regex("(?<param>.+)=\"(?<value>.+)\"");
            Match m = regex.Match(command);
            string param = m.Groups["param"].Value;
            string value = m.Groups["value"].Value;

            switch (param)
            {
                case "Color":
                case "COLOR":
                case "color":
                case "C":
                case "c":
                    try
                    {
                        // Try parsing it as hex value
                        standardColor =
                            Color.FromArgb(Int32.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture));
                    }
                    catch (FormatException)
                    {
                        // Try parsing it as a string value
                        standardColor = Color.FromName(value);
                    }
                    break;

                case "Hover":
                case "HOVER":
                case "hover":
                case "H":
                case "h":
                    try
                    {
                        // Try parsing it as hex value
                        highlightedColor =
                            Color.FromArgb(Int32.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture));
                    }
                    catch (FormatException)
                    {
                        // Try parsing it as a string value
                        highlightedColor = Color.FromName(value);
                    }
                    finally
                    {
                        applyHighlight = true;
                    }
                    break;
            }
        }

        string GenerateId()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", fontName, Size,
                                 ((isBold) ? "Bold" : "Regular"),
                                 isItalic ? "Italic" : string.Empty);
        }

        public override string ToString()
        {
            return GenerateId();
        }

        public static TextStyle FromDefault(Color color)
        {
            TextStyle style = DefaultStyle;
            style.standardColor = color;
            return style;
        }


        public TextStyle ApplyMarkup(string markup)
        {
            TextStyle newStyle = (TextStyle) Clone();
            newStyle.ParseMarkup(markup);
            return newStyle;
        }
    }
}